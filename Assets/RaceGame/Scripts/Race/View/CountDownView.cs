using RaceGame.Race.Interface;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RaceGame.Race.View
{
    /// <summary>
    /// レース開始前のカウントダウンを表示する。シーン内のCountDownImageオブジェクトにアタッチ
    /// </summary>
    public class CountDownView : MonoBehaviour
    {
        [SerializeField] private Sprite[] numbers;
        
        [Inject] private IRaceManager _raceManager;

        private Image _countDownImage;

        private void Start()
        {
            _countDownImage = GetComponent<Image>();

            // RaceManagerからカウントダウンのイベントを受け取る
            _raceManager.OnCountDownTimerChanged += UpdateCountDown;
        }

        private void UpdateCountDown(int time)
        {
            if (time == 0)
            {
                // 非アクティブになった時点でこのスクリプトが止まる
                gameObject.SetActive(false);
            }
            else if (time <= numbers.Length)
            {
                _countDownImage.sprite = numbers[time - 1];
            }
        }
    }
}