using System.Collections;
using System.Collections.Generic;
using RaceGame.Race.Network;
using UnityEngine;
using json;

namespace ReadQR
{
    public class Manager : MonoBehaviour
    {
        /// <summary>
        /// QRコードの読み取りからテクスチャのダウンロード、反映までを統括する
        /// <summary>
        [SerializeField] private GameObject cam_target;
        private GetImage _getImage;
        private ReadQR _readQR;
        public ReadQRstate state;
        private json.Jsondata data;

        public List<Texture2D> player_texture;
        public List<Texture2D> cpu_textures;

        public string player_id = null;
        public int cpu_num = 4;

        private PlayerLookType playerlooktype;

        void Awake()
        {
            // カメラの画面を表示するオブジェクトをReadQRに渡す
            _readQR = GetComponent<ReadQR>();
            _readQR.cam_target = cam_target;
        }
        void Update()
        {
            // QRコードの読み取りが終わっていたら
            if (state == ReadQRstate.ReadedQR)
            {
                state = ReadQRstate.DownloadingImg;
                // プレイヤーの画像ダウンロード
                _getImage = GetComponent<GetImage>();

                _getImage.DownloadPlayerImage (player_id, player_texture);
                playerlooktype = int.Parse(player_id) / 10 == 0 ? PlayerLookType.Horse : (int.Parse(player_id) / 10 == 1 ? PlayerLookType.Car : PlayerLookType.Human);
                
                state = ReadQRstate.Downloaded_PlayerTexture;

                // プレイヤー以外の画像ダウンロード
                _getImage.DownloadCPUImage(player_id, cpu_num, cpu_textures);

                state = ReadQRstate.Downloaded_CPUTexture;

                state = ReadQRstate.Finished;
            }
        }
    }
}
