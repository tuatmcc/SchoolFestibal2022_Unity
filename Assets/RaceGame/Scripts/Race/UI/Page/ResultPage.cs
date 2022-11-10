using Mirror;
using RaceGame.Core;
using RaceGame.Core.Interface;
using RaceGame.Core.Network;
using RaceGame.Core.UI;
using RaceGame.Race.Interface;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RaceGame.Race.UI.Page
{
    /// <summary>
    /// ResultSceneのUIを動かす。RaceMangerから順位を取得する。
    /// </summary>
    public class ResultPage : MonoBehaviour, IPage
    {
        [SerializeField] private SelectableButton retryButton;
        [SerializeField] private SelectableButton backToTitleButton;

        [Inject] private IRaceManager _raceManager;
        [Inject] private IGameSetting _gameSetting;

        private void Start()
        {
            retryButton.OnClicked += Retry;
            backToTitleButton.OnClicked += BackToTitle;
            retryButton.GetComponent<Button>().interactable = _gameSetting.PlayType == PlayType.Solo;
        }

        private void BackToTitle()
        {
            if (_raceManager.LocalPlayer.isServer)
            {
                NetworkManager.singleton.StopHost();
            }
            else
            {
                NetworkManager.singleton.StopClient();
            }
        }

        private void Retry()
        {
            if (_gameSetting.PlayType == PlayType.Solo)
            {
                NetworkManager.singleton.ServerChangeScene(NetworkManager.singleton.onlineScene);
            }
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}