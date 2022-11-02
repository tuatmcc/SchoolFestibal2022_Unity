using System;
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
        public TextureData PlayerTexture;
        public List<TextureData> CPUTextures = new();
        
        [SerializeField] public int cpuNum = 4;
        
        [SerializeField] private QRCodeReader qrCodeReader;
        [SerializeField] private TextureDownloader _textureDownloader;
        
        [SerializeField] private Button soloStartButton;
        [SerializeField] private Button multiStartButton;
        [SerializeField] private GameObject titleBackGround;
        [SerializeField] private GameObject qrCodeBackGround;

        [Inject] private IGameSetting _gameSetting;
        
        private CustomInputAction _customInput;
        
        private void Start()
        {
            _gameSetting.StartFromTitle = true;
            
            _customInput = new CustomInputAction();
            _customInput.Enable();
            
            soloStartButton.onClick.AddListener(StartSolo);
            multiStartButton.onClick.AddListener(StartMulti);

            titleBackGround.SetActive(false);
            qrCodeBackGround.SetActive(true);

            qrCodeReader.OnReadQRCode+= OnReadQRCode;
        }

        private void OnReadQRCode(int result)
        {
            _gameSetting.LocalPlayerID = result;
            
            DownloadTextures(this.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTaskVoid DownloadTextures(CancellationToken cancellationToken)
        {
            PlayerTexture = await _textureDownloader.DownloadPlayerImage(_gameSetting.LocalPlayerID, cancellationToken);
            CPUTextures = await _textureDownloader.DownloadCPUImage(_gameSetting.LocalPlayerID, cpuNum, cancellationToken);
            
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