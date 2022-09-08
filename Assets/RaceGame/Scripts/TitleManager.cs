using RaceGame.Scripts;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using RaceGame.Scripts.Players;

namespace RaceGame.Scripts
{
    public class TitleManager : MonoBehaviour
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
        }
        

        private void ConfirmPlayerName(InputAction.CallbackContext context)
        {
            PlayerInfo.Instance.displayName = playerNameInputField.text;
            confirmNameButton.SetActive(false);
            
            // ここでメインシーンへの遷移イベントを追加
            _customInput.UI.ToMainScene.canceled += ToMainScene;
        }

        private void ToMainScene(InputAction.CallbackContext context)
        {
            SceneLoader.Instance.ToMainScene();
        }
    }
}