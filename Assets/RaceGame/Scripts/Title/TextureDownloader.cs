using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using RaceGame.Race.Network;
using json;

namespace RaceGame.Title
{
    /// <summary>
    /// テクスチャをダウンロードする
    /// </summary>
    public class TextureDownloader
    {
        private const string ImageGetURL = "http://158.101.138.60/player_image/";
        private const string ListGetURL = "http://158.101.138.60/random_image/"; 
        
        private const int NetworkRetryWaitSeconds = 5;
        
        public Jsondata JsonObj;

        /// <summary>
        /// 指定されたIDの画像をダウンロードして情報をtexturesに追加
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        public async UniTask<TextureData> DownloadPlayerImage(int id, CancellationToken cancellationToken)
        {
            await WaitForOnline(cancellationToken);

            var request = new UnityWebRequest($"{ImageGetURL}{id.ToString()}");
            var handler = new DownloadHandlerTexture(true);
            request.downloadHandler = handler;
            
            await request.SendWebRequest().ToUniTask(cancellationToken: cancellationToken);

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("ダウンロードした ID:" + id.ToString());
                return new TextureData() { ID = id, Texture = handler.texture, PlayerLookType = GetLookType(id) };
            }
            else
            {
                Debug.LogError(request.error);
            }

            return null;
        }
        
        /// <summary>
        /// CPUの画像を指定枚数分ダウンロードして情報をtexturesに追加
        /// </summary>
        /// <param name="id"></param>
        /// <param name="num"></param>
        /// <param name="textures"></param>
        public async UniTask<List<TextureData>> DownloadCPUImage(int id, int num, CancellationToken cancellationToken)
        {
            await WaitForOnline(cancellationToken);

            var request = new UnityWebRequest($"{ListGetURL}?player_id={id}&num={num}");
            request.downloadHandler = new DownloadHandlerBuffer();

            await request.SendWebRequest().ToUniTask(cancellationToken: cancellationToken);

            // json形式から構造体に変換
            var jsonObj = JsonUtility.FromJson<Jsondata>(request.downloadHandler.text);

            Debug.Log(request.downloadHandler.text);
            
            var textures = new List<TextureData>();
            foreach (var _ in jsonObj.data)
            {
                textures.Add(await DownloadPlayerImage(id, cancellationToken));
            }

            return textures;
        }
        
        private async UniTask WaitForOnline(CancellationToken cancellationToken)
        {
            while (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Debug.LogError($"ネットワークに接続されていません。{NetworkRetryWaitSeconds}秒後に再試行します。");
                await UniTask.Delay(TimeSpan.FromSeconds(NetworkRetryWaitSeconds), cancellationToken: cancellationToken);
            }
        }
        
        private PlayerLookType GetLookType(int id)
        {
            switch (id % 10)
            {
                case 0:
                    return PlayerLookType.Horse;
                case 1:
                    return PlayerLookType.Car;
                default:
                    return PlayerLookType.Human;
            }
        }
    }
}