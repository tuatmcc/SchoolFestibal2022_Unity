using RaceGame.Core.Interface;
using RaceGame.Title.UI.Pages;
using RaceGame.Title.Sound;
using UnityEngine;
using Zenject;

namespace RaceGame.Title.UI
{
    /// <summary>
    /// TitleSceneを動かし、プレイヤー名を受け取る。
    /// </summary>
    public class TitlePageManager : MonoBehaviour
    {
        [SerializeField] private bool skipQRCode;
        
        [SerializeField] private LogoPage logoPage;
        [SerializeField] private QRCodeReaderPage qrCodeReaderPage;
        [SerializeField] private GameModeSelectPage gameModeSelectPage;
        private TitleBGMPlayer titleBGMPlayer;

        [Inject] private IGameSetting _gameSetting;
        
        private void Start()
        {
            titleBGMPlayer = GetComponent<TitleBGMPlayer>();

            _gameSetting.StartFromTitle = true;

            SetActivePages(false, true, false);
            if (skipQRCode)
            {
                OnReadQRCode(_gameSetting.LocalPlayerID);
            }

            qrCodeReaderPage.OnReadQRCode += OnReadQRCode;

            titleBGMPlayer.PlayTitleBGM();
        }

        private void OnReadQRCode(int result)
        {
            _gameSetting.LocalPlayerID = result;
            SetActivePages(false, false, true);
        }

        private void SetActivePages(bool logo, bool qrCode, bool gameMode)
        {
            logoPage.SetActive(logo);
            qrCodeReaderPage.SetActive(qrCode);
            gameModeSelectPage.SetActive(gameMode);
        }
    }
}