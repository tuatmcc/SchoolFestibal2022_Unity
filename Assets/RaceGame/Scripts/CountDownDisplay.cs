using UnityEngine;
using UnityEngine.UI;

// Attach this to the Image object
// This script will stop running after it is inactivated.

namespace RaceGame.Scripts
{
    public class CountDownDisplay : MonoBehaviour
    {
        [SerializeField] private Sprite number1;
        [SerializeField] private Sprite number2;
        [SerializeField] private Sprite number3;

        private Image _countDownImage;

        private void Start()
        {
            _countDownImage = GetComponent<Image>();
        }

        private void Update()
        {
            if (RaceManager.Instance.CountDownTimer <= 0) _countDownImage.gameObject.SetActive(false);
            else if (RaceManager.Instance.CountDownTimer <= 1) _countDownImage.sprite = number1;
            else if (RaceManager.Instance.CountDownTimer <= 2) _countDownImage.sprite = number2;
            else if (RaceManager.Instance.CountDownTimer <= 3) _countDownImage.sprite = number3;
        }
    }
}