using RaceGame.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace RaceGame.View
{
    /// <summary>
    /// Attach this to the Image object
    /// This script will stop running after it is inactivated.
    /// </summary>
    public class CountDownView : MonoBehaviour
    {
        [SerializeField] private Sprite[] numbers;

        private Image _countDownImage;

        private void Start()
        {
            _countDownImage = GetComponent<Image>();
            
            RaceManager.Instance.OnCountDownTimerChanged += UpdateCountDown;
        }

        private void UpdateCountDown(int time)
        {
            if (time == 0)
            {
                _countDownImage.gameObject.SetActive(false);
            }

            _countDownImage.sprite = numbers[time - 1];
        }
    }
}