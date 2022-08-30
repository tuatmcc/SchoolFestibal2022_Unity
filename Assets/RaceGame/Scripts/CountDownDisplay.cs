using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Attach this to the Image object
// This script will stop running after it is disactivated.

public class CountDownDisplay : MonoBehaviour
{
    [SerializeField] private Sprite number1;
    [SerializeField] private Sprite number2;
    [SerializeField] private Sprite number3;

    private RaceManager _rManager;
    private Image _countDownImage;


    private void Start()
    {
        SceneManager.GetSceneByName(SceneNames.ManagerScene).GetRootGameObjects()[0].TryGetComponent(out _rManager);
        TryGetComponent(out _countDownImage);
    }

    private void Update()
    {
        if (_rManager.CountDownTimer <= 0) _countDownImage.gameObject.SetActive(false);
        else if (_rManager.CountDownTimer <= 1) _countDownImage.sprite = number1;
        else if (_rManager.CountDownTimer <= 2) _countDownImage.sprite = number2;
        else if (_rManager.CountDownTimer <= 3) _countDownImage.sprite = number3;
    }
}
