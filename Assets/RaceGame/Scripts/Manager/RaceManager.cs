using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using RaceGame.Constant;
using RaceGame.Extension;
using RaceGame.Players;
using RaceGame.Scene;
using UnityEngine;

namespace RaceGame.Manager
{
    public class RaceManager : SingletonMonoBehaviour<RaceManager>
    {
        [SerializeField] private Character[] characters;
        
        public RaceState CurrentRaceState { get; private set; } = RaceState.StandingBy;
        
        /// <summary>
        /// カウントダウンタイマーが変わったタイミング
        /// </summary>
        public event Action<int> OnCountDownTimerChanged;
        
        public List<Character> OrderedCharacters { get; private set; } = new();

        private CinemachineSmoothPath _path;

        private void Start()
        {
            OrderedCharacters = characters.ToList();
            _path = GameObject.FindGameObjectWithTag("Path").GetComponent<CinemachineSmoothPath>();
            
            StartCoroutine(RaceLogic());
        }

        private IEnumerator RaceLogic()
        {
            CurrentRaceState = RaceState.StandingBy;

            for (var i = 5; i > 0; i--)
            {
                OnCountDownTimerChanged?.Invoke(i);
                yield return new WaitForSeconds(1);
            }
            OnCountDownTimerChanged?.Invoke(0);

            CurrentRaceState = RaceState.Started;
            
            while (CurrentRaceState == RaceState.Started)
            {
                UpdateCurrentRanking();
            }
        }

        private void UpdateCurrentRanking()
        {
            // 降順に並び替え
            OrderedCharacters = characters.OrderByDescending(x => x.position).ToList();
            
            for (var i = 0; i < OrderedCharacters.Count; i++)
            {
                OrderedCharacters[i].rank = i + 1;
            }

            if (OrderedCharacters.Last().position >= _path.PathLength)
            {
                CurrentRaceState = RaceState.Ended;
                SceneLoader.Instance.ToResultScene();
            }
        }
    }
}