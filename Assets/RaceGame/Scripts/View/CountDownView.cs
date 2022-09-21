using UnityEngine;
using UnityEngine.UI;

using RaceGame.Manager;

namespace RaceGame.View
{
    /// <summary>
    /// レース開始前のカウントダウンを表示する。シーン内のCountDownImageオブジェクトにアタッチ
    /// </summary>
    public class CountDownView : MonoBehaviour
    {
        [SerializeField] private Sprite[] numbers;

        private Image _countDownImage;

        private void Start()
        {
            _countDownImage = GetComponent<Image>();

            // RaceManagerからカウントダウンのイベントを受け取る
            RaceManager.Instance.OnCountDownTimerChanged += UpdateCountDown;
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