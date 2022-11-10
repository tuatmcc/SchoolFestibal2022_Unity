using RaceGame.Core.UI;
using RaceGame.Race.Interface;
using RaceGame.Race.Sound;
using RaceGame.Scripts.Race.UI.Parts;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RaceGame.Race.UI.Page
{
    /// <summary>
    /// レース開始前のカウントダウンを表示する。シーン内のCountDownImageオブジェクトにアタッチ
    /// </summary>
    public class CountDownPage : MonoBehaviour, IPage
    {
        [SerializeField] private Sprite[] numbers;
        [SerializeField] private CountDownCube countDownCube;
        
        [Inject] private IRaceManager _raceManager;

        private CountDownSePlayer _sePlayer;

        private void Start()
        {
            // RaceManagerからカウントダウンのイベントを受け取る
            _raceManager.OnCountDownTimerChanged += UpdateCountDown;
            _raceManager.OnCountDownStart += OnCountDownStart;

            _sePlayer = GetComponent<CountDownSePlayer>();
            countDownCube.gameObject.SetActive(false);
        }

        private void OnCountDownStart()
        {
            countDownCube.gameObject.SetActive(true);
            countDownCube.Play();
        }

        private void UpdateCountDown(int time)
        {
            if (time <= numbers.Length)
            {
                _sePlayer.PlayFirstSE();
            }
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}