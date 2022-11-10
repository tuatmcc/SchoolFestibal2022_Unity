using Mirror;
using RaceGame.Core.UI;
using RaceGame.Race.Interface;
using UnityEngine;
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

        private void Start()
        {
            retryButton.OnClicked += Retry;
            backToTitleButton.OnClicked += BackToTitle;
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
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}