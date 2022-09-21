using RaceGame.Manager;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

using RaceGame.Scene;
using RaceGame.Players;

namespace RaceGame.View
{
    /// <summary>
    /// TitleSceneを動かし、プレイヤー名を受け取る。
    /// </summary>
    public class TitleView : MonoBehaviour
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
            RaceManager.Instance.PlayerName = playerNameInputField.text;
            confirmNameButton.SetActive(false);
            
            // メインシーンへの遷移イベントを追加
            _customInput.UI.ToMainScene.canceled += ToMainScene;
        }

        private void ToMainScene(InputAction.CallbackContext context)
        {
            SceneLoader.Instance.ToMainScene();
        }
    }
}