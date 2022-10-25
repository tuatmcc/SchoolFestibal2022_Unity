using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using json;

public class GetImage : MonoBehaviour
{
    /// <summary>
    /// テクスチャをダウンロードする
    /// <summary>

    [SerializeField] private GameObject Target;
    [SerializeField] private const string ImageGet_URL = "http://158.101.138.60/player_image/";
    [SerializeField] private const string ListGet_URL = "http://158.101.138.60/random_image/"; 
    public json.Jsondata json_obj;

    public void DownloadPlayerImage(string player_id, List<Texture2D> textures)
    {
        StartCoroutine(GetPlayerImage(player_id, textures));
    }
    public void DownloadCPUImage(string player_id, int num, List<Texture2D> textures)
    {
        StartCoroutine(GetCPUImage(player_id, num, textures));
    }
    public IEnumerator GetPlayerImage(string player_id, List<Texture2D> textures)
    {
        while (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("ネットワークに接続されていません");
            yield return new WaitForSeconds(5f);
        }

        UnityWebRequest wr = new UnityWebRequest(ImageGet_URL + player_id);
        DownloadHandlerTexture texDl = new DownloadHandlerTexture(true);
        wr.downloadHandler = texDl;
        
        yield return wr.SendWebRequest();

        if(wr.result == UnityWebRequest.Result.ConnectionError)
            Debug.Log(wr.error);
        else
        {
            textures.Add(texDl.texture); 
            //Target.GetComponent<Renderer> ().material.mainTexture = pleyer_textrue;
        }
    }
    public IEnumerator GetCPUImage(string player_id, int num, List<Texture2D> textures)
    {
        while (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("ネットワークに接続されていません");
            yield return new WaitForSeconds(5f);
        }

        UnityWebRequest wr = new UnityWebRequest(ListGet_URL + "?" + "player_id=" + player_id + "&" + "num=" + num.ToString());
        wr.downloadHandler = new DownloadHandlerBuffer();

        yield return wr.SendWebRequest();

        // json形式から構造体に変換
        var json_obj = JsonUtility.FromJson<json.Jsondata>(wr.downloadHandler.text);

        Debug.Log(wr.downloadHandler.text.ToString());
        foreach (var _ in json_obj.data)
        {
            Debug.Log(_.id);
            DownloadPlayerImage(_.id.ToString(), textures);
            Debug.Log(_.URL);
        }
    }
}