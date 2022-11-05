using Mirror;
using RaceGame.Core;
using RaceGame.Core.Interface;
using RaceGame.Core.UI;
using RaceGame.Title.View;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RaceGame.Title.UI.Pages
{
    public class GameModeSelectPage : MonoBehaviour, IPage
    {
        [SerializeField] private Button soloStartButton;
        [SerializeField] private Button multiStartButton;
        
        [SerializeField] private TitleModelRenderer titleModelRenderer;
        
        [Inject] private IGameSetting _gameSetting;

        private void Start()
        {
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

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}
