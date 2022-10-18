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
            
            soloStartButton.onClick.AddListener(StartSolo);
            multiStartButton.onClick.AddListener(StartMulti);
        }

        private void StartSolo()
        {
            _gameSetting.PlayType = PlayType.Solo;
            NetworkManager.singleton.StartHost();
        }

        private void StartMulti()
        {
            _gameSetting.PlayType = PlayType.Multi;
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