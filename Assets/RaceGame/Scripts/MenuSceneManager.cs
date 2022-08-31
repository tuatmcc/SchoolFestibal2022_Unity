using RaceGame.Scripts;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

namespace RaceGame.Scripts
{
    public class MenuSceneManager : MonoBehaviour
    {
        public TMP_InputField playerNameInputField;
        public GameObject startGameButton;
        public GameObject confirmNameButton;

        private CustomInputAction _customInput;

        private void Start()
        {
            _customInput = new CustomInputAction();
            _customInput.Enable();
            _customInput.UI.Confirm.canceled += ConfirmPlayerName;
            _customInput.UI.ToMainScene.canceled += ToMainScene;
        }
        

        private void ConfirmPlayerName(InputAction.CallbackContext context)
        {
            RaceManager.Instance.PlayerDisplayName = playerNameInputField.text;
            confirmNameButton.SetActive(false);
        }

        private void ToMainScene(InputAction.CallbackContext context)
        {
            SceneLoader.Instance.ToMainScene();
        }
    }
}