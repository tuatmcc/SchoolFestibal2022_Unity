using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using RaceGame.Race.Network;
using Random = UnityEngine.Random;

namespace RaceGame.Title
{
    /// <summary>
    /// テクスチャをダウンロードする
    /// </summary>
    public static class TextureDownloader
    {
        private const string ServerAddress = "104.198.125.126";
        private static readonly string ImageGetURL = $"http://{ServerAddress}/player_image/";
        private static readonly string ListGetURL = $"http://{ServerAddress}/image_list/";

        private const int NetworkRetryWaitSeconds = 5;
        

        /// <summary>
        /// 指定されたIDの画像をダウンロードして情報をtexturesに追加
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        public static async UniTask<TextureData> DownloadPlayerTexture(long id, CancellationToken cancellationToken)
        {
            await WaitForOnline(cancellationToken);

            var request = new UnityWebRequest($"{ImageGetURL}{id}");
            var handler = new DownloadHandlerTexture(true);
            request.downloadHandler = handler;

            try
            {
                await request.SendWebRequest().ToUniTask(cancellationToken: cancellationToken);
            }
            catch
            {
                Debug.LogError(id);
            }

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"ダウンロードした ID:{id}");
                return new TextureData() { ID = id, Texture = handler.texture, PlayerLookType = GetLookType(id) };
            }
            else
            {
                Debug.LogError(request.error);
            }

            return null;
        }

        public static async UniTask<List<long>> DownloadCPUList(List<long> playerIDs, CancellationToken cancellationToken)
        {
            await WaitForOnline(cancellationToken);

            var request = new UnityWebRequest($"{ListGetURL}");
            request.downloadHandler = new DownloadHandlerBuffer();

            await request.SendWebRequest().ToUniTask(cancellationToken: cancellationToken);

            // json形式から構造体に変換
            var jsonObj = JsonUtility.FromJson<JsonData>(request.downloadHandler.text);
            var listAll = jsonObj.data.Select(x => x.id).ToList();
            var list = jsonObj.data
                .Select(x => x.id)
                .Where(x => !playerIDs.Contains(x))
                .OrderBy(x => Random.value)
                .ToList();
            return list;
        }

        /// <summary>
        /// CPUの画像を指定枚数分ダウンロードして情報をtexturesに追加
        /// </summary>
        /// <param name="id"></param>
        /// <param name="num"></param>
        /// <param name="cancellationToken"></param>
        public static async UniTask<List<TextureData>> DownloadCPUImageTextures(int id, int num, CancellationToken cancellationToken)
        {
            await WaitForOnline(cancellationToken);

            var request = new UnityWebRequest($"{ListGetURL}?player_id={id}&num={num}");
            request.downloadHandler = new DownloadHandlerBuffer();

            await request.SendWebRequest().ToUniTask(cancellationToken: cancellationToken);

            // json形式から構造体に変換
            var jsonObj = JsonUtility.FromJson<JsonData>(request.downloadHandler.text);

            Debug.Log(request.downloadHandler.text);
            
            var textures = new List<TextureData>();
            foreach (var _ in jsonObj.data)
            {
                textures.Add(await DownloadPlayerTexture(id, cancellationToken));
            }

            return textures;
        }
        
        private static async UniTask WaitForOnline(CancellationToken cancellationToken)
        {
            while (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Debug.LogError($"ネットワークに接続されていません。{NetworkRetryWaitSeconds}秒後に再試行します。");
                await UniTask.Delay(TimeSpan.FromSeconds(NetworkRetryWaitSeconds), cancellationToken: cancellationToken);
            }
        }
        
        private static PlayerLookType GetLookType(long id)
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