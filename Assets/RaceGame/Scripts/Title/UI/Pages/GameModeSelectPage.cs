using System.Net;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
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
        [SerializeField] private SelectableButton soloStartButton;
        [SerializeField] private SelectableButton multiStartButton;
        
        [SerializeField] private TitleModelRenderer titleModelRenderer;
        
        [Inject] private IGameSetting _gameSetting;

        private void Start()
        {
            soloStartButton.OnClicked += () => StartSolo();
            multiStartButton.OnClicked += () => StartMulti();
            
            soloStartButton.OnSelected += SoloStartButtonSelected;
            multiStartButton.OnSelected += MultiStartButtonSelected;
        }

        private void SoloStartButtonSelected()
        {
            titleModelRenderer.CurrentModelType = TitleModelRenderer.ModelType.Solo;
        }

        private void MultiStartButtonSelected()
        {
            titleModelRenderer.CurrentModelType = TitleModelRenderer.ModelType.Multi;
        }

        private async Task StartSolo()
        {
            await Animation();
            _gameSetting.PlayType = PlayType.Solo;
            NetworkManager.singleton.StartHost();
        }

        private async UniTask Animation()
        {
            titleModelRenderer.speed = 5f;
            await UniTask.Delay(500);
            titleModelRenderer.speed = 1f;
            await UniTask.Delay(100);
        }

        private async Task StartMulti()
        {
            await Animation();
            _gameSetting.PlayType = PlayType.Multi;

            if (IsHostComputer())
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
            else
            {
                NetworkManager.singleton.StartClient();
            }
        }

        private bool IsHostComputer()
        {
            var hostname = Dns.GetHostName();

            var adrList = Dns.GetHostAddresses(hostname);
            foreach (var address in adrList)
            {
                if (address.ToString() == NetworkManager.singleton.networkAddress) return true;
            }

            return false;
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}
