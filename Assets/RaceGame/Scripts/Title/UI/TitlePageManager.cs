using RaceGame.Core.Interface;
using RaceGame.Title.UI.Pages;
using UnityEngine;
using Zenject;

namespace RaceGame.Title.UI
{
    /// <summary>
    /// TitleSceneを動かし、プレイヤー名を受け取る。
    /// </summary>
    public class TitlePageManager : MonoBehaviour
    {
        [SerializeField] private LogoPage logoPage;
        [SerializeField] private QRCodeReaderPage qrCodeReaderPage;
        [SerializeField] private GameModeSelectPage gameModeSelectPage;

        [Inject] private IGameSetting _gameSetting;
        
        private void Start()
        {
            _gameSetting.StartFromTitle = true;

            SetActivePages(false, true, false);

            qrCodeReaderPage.OnReadQRCode += OnReadQRCode;
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