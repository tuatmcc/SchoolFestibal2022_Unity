using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

namespace RaceGame.Scripts
{
    public class RaceManager : MonoBehaviour
    {
        [SerializeField] private CinemachineSmoothPath path;
        public string PlayerDisplayName { get; set; } = "Nameless";
        public float CountDownTimer { get; private set; } = 5f;

        public RaceStates CurrentRaceState { get; private set; } = RaceStates.StandingBy;
        public List<Character> Characters { get; set; } = new List<Character>();

        public enum RaceStates
        {
            StandingBy,
            Started,
            Ended
        }

        // シングルトン？
        public static RaceManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }


        private void Start()
        {
        }

        private void Update()
        {
            if (SceneManager.GetActiveScene().name != SceneNames.MainScene) return;

            switch (CurrentRaceState)
            {
                case RaceStates.StandingBy:
                {
                    CountDownTimer -= Time.deltaTime;
                    CurrentRaceState = CountDownTimer <= 0 ? RaceStates.Started : CurrentRaceState;
                    break;
                }
                case RaceStates.Started:
                {
                    UpdateCurrentRanking();
                    break;
                }
                case RaceStates.Ended:
                    break;
                default:
                    break;
            }
        }

        private void UpdateCurrentRanking()
        {
            // 位置の差を小数点以下2桁までで比較する
            Characters.Sort((a, b) => (b.position * 100).CompareTo(a.position * 100));

            for (var i = 0; i < Characters.Count; i++)
            {
                Characters[i].rank = i + 1;
            }

            if (Characters[Characters.Count - 1].position == path.PathLength)
            {
                CurrentRaceState = RaceStates.Ended;
                SceneLoader.Instance.ToResultScene();
            }
        }
    }
}