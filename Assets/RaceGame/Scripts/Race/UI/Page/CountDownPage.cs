using RaceGame.Race.Interface;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RaceGame.Race.View
{
    /// <summary>
    /// レース開始前のカウントダウンを表示する。シーン内のCountDownImageオブジェクトにアタッチ
    /// </summary>
    public class CountDownPage : MonoBehaviour
    {
        [SerializeField] private Image countDownImage;
        [SerializeField] private Sprite[] numbers;
        
        [Inject] private IRaceManager _raceManager;

        private void Start()
        {
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
                countDownImage.sprite = numbers[time - 1];
            }
        }
    }
}