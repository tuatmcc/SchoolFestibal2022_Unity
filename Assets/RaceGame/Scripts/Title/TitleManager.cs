using Mirror;
using RaceGame.Core.Interface;
using RaceGame.Race.Network;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace RaceGame.Title
{
    /// <summary>
    /// TitleSceneを動かし、プレイヤー名を受け取る。
    /// </summary>
    public class TitleManager : MonoBehaviour
    {
        [SerializeField] private TMP_InputField playerNameInputField;
        
        [SerializeField] private GameObject startGameButton;
        [SerializeField] private GameObject confirmNameButton;

        private CustomInputAction _customInput;

        [Inject] private ISceneManager _sceneManager;

        private void Start()
        {
            _customInput = new CustomInputAction();
            _customInput.Enable();
            
            _customInput.UI.Confirm.canceled += ConfirmPlayerName;
        }
        
        private void ConfirmPlayerName(InputAction.CallbackContext context)
        {
            var localPlayer = NetworkClient.localPlayer.GetComponent<Player>();
            localPlayer.playerName = playerNameInputField.text;
            confirmNameButton.SetActive(false);
            
            // メインシーンへの遷移イベントを追加
            _customInput.UI.ToMainScene.canceled += ToRaceScene;
        }

        private void ToRaceScene(InputAction.CallbackContext context)
        {
            _sceneManager.ToRace();
        }
    }
}