using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cinemachine;
using Cysharp.Threading.Tasks;
using Mirror;
using RaceGame.Core;
using RaceGame.Core.Interface;
using RaceGame.Race.Interface;
using RaceGame.Race.Network;
using RaceGame.Title;
using UnityEngine;
using Zenject;

namespace RaceGame.Race
{
    /// <summary>
    /// 順位や位置などは全てサーバー側で管理する
    /// </summary>
    public class RaceManager : NetworkBehaviour, IRaceManager
    {
        public bool StartFromTitle { get; set; }
        public event Action OnRaceStandby;
        public event Action OnRaceStart;
        public event Action OnRaceFinish;
        public event Action OnCountDownStart;
        public event Action<int> OnCountDownTimerChanged;
        public event Action<List<Player>> OnPlayerOrderChanged;

        public RaceState RaceState { get; private set; } = RaceState.NonInitialized;
        public Player[] Players => Enemies.Concat(_playersWithoutEnemies).ToArray();
        private List<Player> _playersWithoutEnemies = new();

        public Player LocalPlayer { get; private set; }
        
        [SerializeField] private CinemachineSmoothPath path;
        [SerializeField] private List<Player> enemies;
        private List<Player> Enemies => enemies.Where(x => x.gameObject.activeSelf).ToList();

        [Inject] private IGameSetting _gameSetting;
        
        private List<Player> _orderedPlayers;
        
        private void SetOrderedPlayers(List<Player> orderedPlayers)
        {
            if (orderedPlayers.Count != _orderedPlayers.Count)
            {
                OnPlayerOrderChanged?.Invoke(_orderedPlayers);
            }
            else
            {
                for (var i = 0; i < orderedPlayers.Count; i++)
                {
                    if (orderedPlayers[i] != _orderedPlayers[i])
                    {
                        OnPlayerOrderChanged?.Invoke(_orderedPlayers);
                        break;
                    }
                }
            }

            _orderedPlayers = orderedPlayers;
        }

        [Command(requiresAuthority = false)]
        private void CmdSetLocalPlayerID(Player localPlayer, long id)
        {
            localPlayer.PlayerID = id;
        }

        public void AddPlayer(Player player)
        {
            if (player.isLocalPlayer)
            {
                LocalPlayer = player;
                CmdSetLocalPlayerID(player, _gameSetting.LocalPlayerID);
            }

            if (player.GetComponent<EnemyPlayerController>() == null)
            {
                _playersWithoutEnemies.Add(player);
            }

            if (LocalPlayer == null) return;
            if (_gameSetting.PlayType == PlayType.Solo || _playersWithoutEnemies.Count >= 2)
            {
                if (RaceState == RaceState.NonInitialized)
                {
                    GameStart(this.GetCancellationTokenOnDestroy()).Forget();
                    RaceState = RaceState.StandingBy;
                }
            }
        }

        private async UniTaskVoid GameStart(CancellationToken cancellationToken)
        {
            GetEnemiesList(cancellationToken).Forget();
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            CmdGameStart();
        }

        private async UniTask GetEnemiesList(CancellationToken cancellationToken)
        {
            var list = await TextureDownloader.DownloadCPUList(_playersWithoutEnemies.Select(x=>x.PlayerID).ToList(), cancellationToken);
            for (var i = 0; i < Enemies.Count; i++)
            {
                Enemies[i].PlayerID = list[i];
            }
        }

        private void Start()
        {
            _orderedPlayers = Players.ToList();
            if (_gameSetting.PlayType == PlayType.Multi)
            {
                var lastEnemy = enemies.Last();
                lastEnemy.gameObject.SetActive(false);
            }
            else
            {
                foreach (var enemy in enemies)
                {
                    enemy.gameObject.SetActive(true);
                }
            }
        }

        [Server]
        [Command(requiresAuthority = false)]
        private void CmdGameStart()
        {
            RpcGameStart();
        }

        [ClientRpc]
        private void RpcGameStart()
        {
            RaceLogic(this.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTask RaceLogic(CancellationToken token)
        {
            RaceState = RaceState.StandingBy;
            OnRaceStandby?.Invoke();

            await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token);
            // レース開始までのカウントダウン
            for (var i = 3; i > 0; i--)
            {
                OnCountDownStart?.Invoke();
                OnCountDownTimerChanged?.Invoke(i);
                await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token);
            }

            RaceState = RaceState.Racing;
            OnRaceStart?.Invoke();

            while (RaceState == RaceState.Racing)
            {
                UpdateCurrentRanking();
                await UniTask.Yield(cancellationToken: token);
            }
        }

        [Server]
        private void UpdateCurrentRanking()
        {
            // 降順に並び替え
            var orderedPlayers = Players.Where(x => x.IsGoal)
                .Concat(Players.Where(x => !x.IsGoal).OrderByDescending(x => x.Position)).ToList();
            SetOrderedPlayers(orderedPlayers);
            
            for (var i = 0; i < _orderedPlayers.Count; i++)
            {
                var player = _orderedPlayers[i];
                if (!player.IsGoal)
                {
                    player.rank = i + 1;
                    if(player.Position >= path.PathLength)
                    {
                        player.Goal();
                    }
                }
            }

            // 全員がゴールしたとき
            if (_orderedPlayers.All(x=>x.IsGoal))
            {
                RaceState = RaceState.Finished;
                CmdRaceFinish();
            }
        }
        
        [Server]
        [Command(requiresAuthority = false)]
        private void CmdRaceFinish()
        {
            RpcRaceFinish();
        }

        [ClientRpc]
        private void RpcRaceFinish()
        {
            RaceState = RaceState.Finished;
            OnRaceFinish?.Invoke();
        }
    }
}