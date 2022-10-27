using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;

namespace RaceGame.Title
{
    public class ReadQR : MonoBehaviour
    {
        /// <summary>
        /// QRコードの読み取りを行う
        /// <summary>

        public GameObject cam_target;

        private int width = 1920;
        private int height = 1080;
        private int fps = 30;
        public int cam_number = 0;
        WebCamTexture webcamTexture;

        // 結果保存用変数
        private ZXing.Result _result, _pre_result;
        public int result;
        private int count = 0;
        [SerializeField] private static int check_count = 5; 

        void Start()
        {
            try
            {
                //使用可能なデバイスの取得
                WebCamDevice[] devices = WebCamTexture.devices;
                //オブジェクト生成
                webcamTexture = new WebCamTexture(devices[cam_number].name, this.width, this.height, this.fps);
                //テクスチャ反映
                cam_target.GetComponent<RawImage>().texture = webcamTexture;
                //カメラの起動
                webcamTexture.Play();
            }
            catch
            {
                Debug.LogError("カメラを正常に起動できませんでした");
            }
            // 未読み取りなら-1
            result = -1;
        }

        void Update()
        {
            // カメラが動いていなければスルー
            if(webcamTexture == null || !(webcamTexture.isPlaying)) return;
            // QQコード読み取り
            var reader = new BarcodeReader();
            int w = webcamTexture.width;
            int h = webcamTexture.height;
            var rawRGB = webcamTexture.GetPixels32();
            _result = reader.Decode(rawRGB, w, h);
            if(_result != null)
            {
                // 5回連続で同じ結果であれば終了
                if(count < check_count)
                    count += (count == 0 ?  1 : (_result.Text == _pre_result.Text ? 1 : -count));
                else
                {
                    // カメラを停止して結果を保存
                    webcamTexture.Stop();
                    result = int.Parse(_result.Text);
                }
                Debug.Log(_result.Text);
                _pre_result = _result;
            }
        }
    }
}