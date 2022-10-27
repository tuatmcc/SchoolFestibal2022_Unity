using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using RaceGame.Race.Network;
using json;

namespace RaceGame.Title
{
    public class GetImage : MonoBehaviour
    {
        /// <summary>
        /// テクスチャをダウンロードする
        /// <summary>

        //[SerializeField] private GameObject Target;
        [SerializeField] private const string ImageGet_URL = "http://158.101.138.60/player_image/";
        [SerializeField] private const string ListGet_URL = "http://158.101.138.60/random_image/"; 
        public json.Jsondata json_obj;

        // 指定されたIDの画像をダウンロードして情報をtexturesに追加
        public void DownloadPlayerImage(int id, List<TextureData> textures)
        {
            StartCoroutine(GetPlayerImage(id, textures));
        }
        // CPUの画像を指定枚数分ダウンロードして情報をtexturesに追加
        public void DownloadCPUImage(int id, int num, List<TextureData> textures)
        {
            StartCoroutine(GetCPUImage(id, num, textures));
        }
        public IEnumerator GetPlayerImage(int id, List<TextureData> textures)
        {
            while (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Debug.Log("ネットワークに接続されていません");
                yield return new WaitForSeconds(5f);
            }

            UnityWebRequest wr = new UnityWebRequest(ImageGet_URL + id.ToString());
            DownloadHandlerTexture texDl = new DownloadHandlerTexture(true);
            wr.downloadHandler = texDl;
            
            yield return wr.SendWebRequest();

            if(wr.result == UnityWebRequest.Result.ConnectionError)
                Debug.Log(wr.error);
            else
            {
                textures.Add(new TextureData(){ID = id, texture = texDl.texture, playerLookType = looktype(id)}); 
                Debug.Log("ダウンロードした ID:" + id.ToString());
            }
        }
        public IEnumerator GetCPUImage(int id, int num, List<TextureData> textures)
        {
            while (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Debug.Log("ネットワークに接続されていません");
                yield return new WaitForSeconds(5f);
            }

            UnityWebRequest wr = new UnityWebRequest(ListGet_URL + "?" + "player_id=" + id.ToString() + "&" + "num=" + num.ToString());
            wr.downloadHandler = new DownloadHandlerBuffer();

            yield return wr.SendWebRequest();

            // json形式から構造体に変換
            var json_obj = JsonUtility.FromJson<json.Jsondata>(wr.downloadHandler.text);

            Debug.Log(wr.downloadHandler.text.ToString());
            foreach (var _ in json_obj.data)
            {
                DownloadPlayerImage(_.id, textures);
            }
        }
        private PlayerLookType looktype(int id)
        {
            return id % 10 == 0 ? PlayerLookType.Horse : (id % 10 == 1 ? PlayerLookType.Car : PlayerLookType.Human);
        }
    }
}