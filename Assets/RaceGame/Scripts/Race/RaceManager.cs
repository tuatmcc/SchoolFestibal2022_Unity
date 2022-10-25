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
using UnityEngine;
using Zenject;

namespace RaceGame.Race
{
    public class RaceManager : NetworkBehaviour, IRaceManager
    {
        public bool StartFromTitle { get; set; }
        
        public event Action OnRaceFinished;
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
            RaceState = RaceState.Finished;
            Players.Add(player);
            if (player.isLocalPlayer)
            {
                LocalPlayer = player;
                player.playerID = _gameSetting.LocalPlayerID;
            }

            if (_gameSetting.PlayType == PlayType.Solo || Players.Count >= 5)
            {
                CmdGameStart();
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

            // レース開始までのカウントダウン
            for (var i = 5; i > 0; i--)
            {
                OnCountDownTimerChanged?.Invoke(i);
                await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token);
            }

            OnCountDownTimerChanged?.Invoke(0);

            RaceState = RaceState.Racing;

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
                _orderedPlayers[i].rank = i + 1;
            }

            // 全員がゴールしたとき
            if (_orderedPlayers.Last().Position >= path.PathLength)
            {
                RaceState = RaceState.Finished;
                OnRaceFinished?.Invoke();
            }
        }
    }
}