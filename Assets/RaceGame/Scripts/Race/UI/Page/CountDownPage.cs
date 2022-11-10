using RaceGame.Core.UI;
using RaceGame.Race.Interface;
using RaceGame.Race.Sound;
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
        [SerializeField] private Image countDownImage;
        [SerializeField] private Sprite[] numbers;
        
        [Inject] private IRaceManager _raceManager;

        private CountDownSePlayer _sePlayer;

        private void Start()
        {
            // RaceManagerからカウントダウンのイベントを受け取る
            _raceManager.OnCountDownTimerChanged += UpdateCountDown;

            _sePlayer = GetComponent<CountDownSePlayer>();
        }

        private void UpdateCountDown(int time)
        {
            if (time <= numbers.Length)
            {
                countDownImage.sprite = numbers[time - 1];
                _sePlayer.PlayFirstSE();
            }
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}