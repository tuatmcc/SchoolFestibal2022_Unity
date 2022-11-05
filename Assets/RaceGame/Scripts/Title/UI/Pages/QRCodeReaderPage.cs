using System;
using RaceGame.Core.UI;
using UnityEngine;
using UnityEngine.UI;
using ZXing;

namespace RaceGame.Title.UI.Pages
{
    /// <summary>
    /// QRコードの読み取りを行う
    /// </summary>
    public class QRCodeReaderPage : MonoBehaviour, IPage
    {
        [SerializeField] private RawImage cameraImage;
        [SerializeField] private int cameraIndex;
        
        /// <summary>
        /// QRコードが読み終えたら読み取った値を返す
        /// </summary>
        public event Action<int> OnReadQRCode;
        
        private int _width = 1920;
        private int _height = 1080;
        private int _fps = 30;

        // 結果保存用変数
        private Result _result;
        private Result _preResult;
        
        private const int MaxSameValueCount = 5;
        private int _currentSameValueCount;

        private WebCamTexture _webCamTexture;

        private void Start()
        {
            try
            {
                // 使用可能なデバイスの取得
                var devices = WebCamTexture.devices;
                // オブジェクト生成
                _webCamTexture = new WebCamTexture(devices[cameraIndex].name, _width, _height, _fps);
                // テクスチャ反映
                cameraImage.texture = _webCamTexture;
                // カメラの起動
                _webCamTexture.Play();
            }
            catch(Exception e)
            {
                Debug.LogError(e);
                Debug.LogError("カメラを正常に起動できませんでした");
            }
        }

        private void Update()
        {
            // カメラが動いていなければスルー
            if (_webCamTexture == null || !_webCamTexture.isPlaying) return;
            
            // QQコード読み取り
            var reader = new BarcodeReader();
            var width = _webCamTexture.width;
            var height = _webCamTexture.height;
            var rawRGB = _webCamTexture.GetPixels32();
            
            _result = reader.Decode(rawRGB, width, height);
            
            if(_result != null)
            {
                Debug.Log($"QRCode Read : {_result.Text}");
                // 5回連続で同じ結果であれば終了
                if (_currentSameValueCount < MaxSameValueCount)
                {
                    if (_currentSameValueCount == 0 || _preResult.Text == _result.Text)
                    {
                        _currentSameValueCount++;
                    }
                    else
                    {
                        _currentSameValueCount = 0;
                    }
                }
                else
                {
                    // カメラを停止して結果を保存
                    _webCamTexture.Stop();
                    OnReadQRCode?.Invoke(int.Parse(_result.Text));
                }

                _preResult = _result;
            }
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}