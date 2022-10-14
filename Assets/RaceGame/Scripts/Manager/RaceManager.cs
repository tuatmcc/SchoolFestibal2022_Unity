using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cinemachine;
using Cysharp.Threading.Tasks;
using Mirror;
using RaceGame.Extension;
using RaceGame.Players;
using RaceGame.Scene;
using UnityEngine;

namespace RaceGame.Manager
{
    public class RaceManager : SingletonNetworkBehaviour<RaceManager>
    {
        private List<Player> _players = new();
        public List<Player> Players => _players;

        public bool startFromTitle;

        public CinemachineSmoothPath path;
        public event Action<int> OnCountDownTimerChanged;

        public RaceState CurrentRaceState { get; private set; } = RaceState.StandingBy;

        public List<Player> OrderedCharacters { get; private set; } = new();

        public void AddPlayer(Player player)
        {
            _players.Add(player);
            Debug.Log($"AddPlayer: {_players.Count}");
            if (_players.Count >= 3)
            {
                CmdGameStart();
            }
        }
        
        public void Start()
        {
            OrderedCharacters = _players.ToList();
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
            CurrentRaceState = RaceState.StandingBy;

            // レース開始までのカウントダウン
            for (var i = 5; i > 0; i--)
            {
                OnCountDownTimerChanged?.Invoke(i);
                await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token);
            }

            OnCountDownTimerChanged?.Invoke(0);

            CurrentRaceState = RaceState.Racing;

            while (CurrentRaceState == RaceState.Racing)
            {
                UpdateCurrentRanking();
                await UniTask.Yield(cancellationToken: token);
            }
        }

        private void UpdateCurrentRanking()
        {
            // 降順に並び替え
            OrderedCharacters = _players.OrderByDescending(x => x.Position).ToList();
            
            for (var i = 0; i < OrderedCharacters.Count; i++)
            {
                OrderedCharacters[i].rank = i + 1;
            }

            // 全員がゴールしたとき
            if (OrderedCharacters.Last().Position >= path.PathLength)
            {
                CurrentRaceState = RaceState.Ended;

                ((RaceGameNetworkManager)NetworkManager.singleton).ToResultScene();
            }
        }
    }
}