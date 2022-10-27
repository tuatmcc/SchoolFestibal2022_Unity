using System.Collections.Generic;
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
        private GameObject _titleBackGround;
        private GameObject _qrCodeBackGround;

        public List<TextureData> player_texture = new List<TextureData>();
        public List<TextureData> cpu_textures = new List<TextureData>();

        [SerializeField] private Button soloStartButton;
        [SerializeField] private Button multiStartButton;

        [Inject] private IGameSetting _gameSetting;
        [SerializeField] public int cpu_num = 4;
        [SerializeField] private bool ReadedQR = false;
        
        private CustomInputAction _customInput;
        private ReadQR _readQR;
        private GetImage _getImage;
        
        private void Start()
        {
            _gameSetting.StartFromTitle = true;
            
            _customInput = new CustomInputAction();
            _customInput.Enable();
            
            soloStartButton.onClick.AddListener(StartSolo);
            multiStartButton.onClick.AddListener(StartMulti);

            _titleBackGround = transform.Find("TitleBackGround").gameObject;
            _titleBackGround.SetActive(false);

            _qrCodeBackGround = transform.Find("QRCodeBackGround").gameObject;

            _getImage = GetComponentInChildren(typeof(GetImage)) as GetImage;
            _readQR = GetComponentInChildren(typeof(ReadQR)) as ReadQR;
        }

        private void Update() 
        {
            // 1度のみ実行QRコードの読み取りが終わっていたら画像ダウンロード
            if(!ReadedQR && _readQR.result != -1) 
            {
                ReadedQR = true;
                
                Debug.Log(_readQR.result);
                _gameSetting.LocalPlayerID = _readQR.result;

                _getImage.DownloadPlayerImage(_gameSetting.LocalPlayerID, player_texture);
                _getImage.DownloadCPUImage(_gameSetting.LocalPlayerID, cpu_num, cpu_textures);
            }
            // 画像ダウンロードが終わっていたらUI切り替え
            if(player_texture != null && cpu_textures.Count == cpu_num && !_titleBackGround.activeSelf)
            {
                _qrCodeBackGround.SetActive(false);
                _titleBackGround.SetActive(true);
            }
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