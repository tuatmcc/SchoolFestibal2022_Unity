using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
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
        public TextureData PlayerTexture { get; private set; }
        public List<TextureData> CPUTextures { get; private set; }

        [SerializeField] public int cpuCount = 4;
        [SerializeField] private QRCodeReader qrCodeReader;
        
        [SerializeField] private Button soloStartButton;
        [SerializeField] private Button multiStartButton;
        [SerializeField] private GameObject titleBackGround;
        [SerializeField] private GameObject qrCodeBackGround;

        [Inject] private IGameSetting _gameSetting;
        
        private void Start()
        {
            _gameSetting.StartFromTitle = true;
            
            soloStartButton.onClick.AddListener(StartSolo);
            multiStartButton.onClick.AddListener(StartMulti);

            titleBackGround.SetActive(false);
            qrCodeBackGround.SetActive(true);

            qrCodeReader.OnReadQRCode += OnReadQRCode;
        }

        private void OnReadQRCode(int result)
        {
            DownloadTextures(result, this.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTaskVoid DownloadTextures(int localPlayerID, CancellationToken cancellationToken)
        {
            _gameSetting.LocalPlayerID = localPlayerID;
            
            var downloader = new TextureDownloader();
            PlayerTexture = await downloader.DownloadPlayerTexture(localPlayerID, cancellationToken);
            CPUTextures = await downloader.DownloadCPUImageTextures(localPlayerID, cpuCount, cancellationToken);
            
            qrCodeBackGround.SetActive(false);
            titleBackGround.SetActive(true);
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