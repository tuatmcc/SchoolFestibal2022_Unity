using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

namespace RaceGame.Scripts
{
    public class RaceManager : MonoBehaviour
    {
        [SerializeField] private Character[] characters;
        public float CountDownTimer { get; private set; } = 5f;
        

        public RaceStates CurrentRaceState { get; private set; } = RaceStates.StandingBy;
        public List<Character> Characters { get; private set; } = new List<Character>();

        private CinemachineSmoothPath _path;
        

        // シングルトン？
        public static RaceManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            
            Characters = characters.ToList();
        }


        private void Start()
        {

            _path =  GameObject.FindGameObjectWithTag("Path").GetComponent<CinemachineSmoothPath>();
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

            if ((int)Characters[Characters.Count - 1].position == (int)_path.PathLength)
            {
                CurrentRaceState = RaceStates.Ended;
                SceneLoader.Instance.ToResultScene();
            }
        }

        private IEnumerator CountDown()
        {
            while (CountDownTimer > 0)
            {
                CountDownTimer--;
                yield return new WaitForSeconds(1);
            }

            CurrentRaceState = RaceStates.Started;
        }
    }
}