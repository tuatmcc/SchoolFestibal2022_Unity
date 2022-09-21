using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using Cysharp.Threading.Tasks;
using RaceGame.Constant;
using RaceGame.Misc;
using RaceGame.Players;
using RaceGame.Scene;
using UnityEditor.Callbacks;

namespace RaceGame.Manager
{
    public class RaceManager
    {
        // Characterをインスペクターからアタッチできないし、GameObject.FindObjectsOfTypeできない
        // PathをGameObject.FindGameObjectWithTagできない :(
        // なのでMainScene上のRaceManagerTriggerから受け取る
        public Character[] Characters;
        public CinemachineSmoothPath Path;

        public string PlayerName;
        public int PlayerId;
        public Texture PlayerCustomTexture;

        public event Action<int> OnCountDownTimerChanged;

        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public RaceState CurrentRaceState { get; private set; } = RaceState.StandingBy;

        public List<Character> OrderedCharacters { get; private set; } = new List<Character>();


        private static RaceManager _instance;
        public static RaceManager Instance
        {
            get
            {
                _instance = _instance ?? new RaceManager();
                return _instance;
            }
        }
        
        public void StandByForRace()
        {
            
            OrderedCharacters = Characters.ToList();

            RaceLogic(_cancellationTokenSource.Token).Forget();
        }

        private async UniTask RaceLogic(CancellationToken token)
        {
            
            CurrentRaceState = RaceState.StandingBy;

            for (var i = 5; i > 0; i--)
            {
                OnCountDownTimerChanged?.Invoke(i);
                await UniTask.Delay(1000, false, PlayerLoopTiming.Update, token);
            }

            OnCountDownTimerChanged?.Invoke(0);

            CurrentRaceState = RaceState.Started;

            while (CurrentRaceState == RaceState.Started)
            {
                UpdateCurrentRanking();
                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }
        }
        

        private void UpdateCurrentRanking()
        {
            // 降順に並び替え
            OrderedCharacters = OrderedCharacters.OrderByDescending(x => x.position).ToList();
            
            for (var i = 0; i < OrderedCharacters.Count; i++)
            {
                OrderedCharacters[i].rank = i + 1;
            }

            // 全員がゴールしたとき
            if (OrderedCharacters.Last().position == Path.PathLength)
            {
                CurrentRaceState = RaceState.Ended;
                
                // UniTaskをキャンセル
                _cancellationTokenSource.Cancel();
                SceneLoader.Instance.ToResultScene();
            }
        }
    }
}