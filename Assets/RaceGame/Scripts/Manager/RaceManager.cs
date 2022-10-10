using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using Cinemachine;
using Cysharp.Threading.Tasks;
using Mirror;
using RaceGame.Extension;
using RaceGame.Players;
using RaceGame.Scene;

namespace RaceGame.Manager
{
    public class RaceManager : SingletonMonoBehaviour<RaceManager>
    {
        /// <summary>
        /// 最初から置いてあるCPU Player
        /// Playerの数によって減らす
        /// </summary>
        [SerializeField]
        private Player[] enemies;
        
        private List<Player> _players = new();
        
        private List<Player> AllPlayers => _players.Concat(enemies.Where(x=>x.gameObject.activeSelf)).ToList();

        public CinemachineSmoothPath path;
        public event Action<int> OnCountDownTimerChanged;

        public RaceState CurrentRaceState { get; private set; } = RaceState.StandingBy;

        public List<Player> OrderedCharacters { get; private set; } = new();

        public void AddPlayer(Player player)
        {
        }
        
        public void Start()
        {
            OrderedCharacters = AllPlayers.ToList();

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
            OrderedCharacters = OrderedCharacters.OrderByDescending(x => x.Position).ToList();
            
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