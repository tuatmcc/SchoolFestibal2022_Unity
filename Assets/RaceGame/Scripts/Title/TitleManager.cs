using Mirror;
using RaceGame.Core;
using RaceGame.Core.Interface;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RaceGame.Title
{
    /// <summary>
    /// TitleSceneを動かし、プレイヤー名を受け取る。
    /// </summary>
    public class TitleManager : MonoBehaviour
    {
        [SerializeField] private Button soloStartButton;
        [SerializeField] private Button multiStartButton;

        [Inject] private IGameSetting _gameSetting;
        
        private CustomInputAction _customInput;
        
        private void Start()
        {
            _gameSetting.StartFromTitle = true;
            
            _customInput = new CustomInputAction();
            _customInput.Enable();
            
            // NetworkManager.singleton.StartHost();
            soloStartButton.onClick.AddListener(OnSoloStartButtonClick);
            multiStartButton.onClick.AddListener(OnMultiStartButtonClick);
        }

        private void OnSoloStartButtonClick()
        {
            _gameSetting.PlayType = PlayType.Solo;
            NetworkManager.singleton.StartHost();
        }

        private void OnMultiStartButtonClick()
        {
            _gameSetting.PlayType = PlayType.Multi;
            StartMulti();
        }

        private void StartMulti()
        {
            if (!ParrelSync.ClonesManager.IsClone())
            {
                NetworkManager.singleton.StartHost();
            }
            else
            {
                NetworkManager.singleton.StartClient();
            }
        }
    }
}