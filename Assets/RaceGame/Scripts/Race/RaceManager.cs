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
    public class RaceManager : NetworkBehaviour, IRaceManager
    {
        public bool StartFromTitle { get; set; }

        public event Action OnRaceStandby;
        public event Action OnRaceStart;
        public event Action OnRaceFinish;
        public event Action<int> OnCountDownTimerChanged;
        public event Action<List<Player>> OnPlayerOrderChanged;

        public RaceState RaceState { get; private set; } = RaceState.StandingBy;
        public List<Player> Players { get; private set; } = new();

        public Player LocalPlayer { get; private set; }
        
        [SerializeField] private CinemachineSmoothPath path;
        
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

        public void AddPlayer(Player player)
        {
            Players.Add(player);
            if (player.isLocalPlayer)
            {
                LocalPlayer = player;
                player.playerID = _gameSetting.LocalPlayerID;
            }

            if (_gameSetting.PlayType == PlayType.Solo || Players.Count >= 5)
            {
                GameStart(_gameSetting.LocalPlayerID, this.GetCancellationTokenOnDestroy()).Forget();
            }
        }

        private async UniTaskVoid GameStart(int localPlayerID, CancellationToken cancellationToken)
        {
            GetEnemiesList(localPlayerID, cancellationToken).Forget();
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            CmdGameStart();
        }

        private async UniTask GetEnemiesList(int localPlayerID, CancellationToken cancellationToken)
        {
            var enemies = Players.Where(player => player.GetComponent<EnemyPlayerController>() != null).ToList();
            var list = await TextureDownloader.DownloadCPUList(localPlayerID, enemies.Count, cancellationToken);
            for (var i = 0; i < list.Count; i++)
            {
                enemies[i].playerID = list[i];
            }
        }

        private void Start()
        {
            _orderedPlayers = Players.ToList();
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

            // レース開始までのカウントダウン
            for (var i = 3; i > 0; i--)
            {
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

        private void UpdateCurrentRanking()
        {
            // 降順に並び替え
            SetOrderedPlayers(Players.OrderByDescending(x => x.Position).ToList());
            
            for (var i = 0; i < _orderedPlayers.Count; i++)
            {
                var player = _orderedPlayers[i];
                player.rank = i + 1;
                if (!player.IsGoal)
                {
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
                OnRaceFinish?.Invoke();
            }
        }
    }
}