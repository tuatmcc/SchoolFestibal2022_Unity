using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;

public class ReadQR : MonoBehaviour
{
    private int width = 1920;
    private int height = 1080;
    private int fps = 30;
    public int cam_number = 0;
    WebCamTexture webcamTexture;

    void Start()
    {
        try
        {
            //使用可能なデバイスの取得
            WebCamDevice[] devices = WebCamTexture.devices;
            //オブジェクト生成
            webcamTexture = new WebCamTexture(devices[cam_number].name, this.width, this.height, this.fps);
            //テクスチャ反映
            GetComponent<Renderer> ().material.mainTexture = webcamTexture;
            //カメラの起動
            webcamTexture.Play();
        }
        catch
        {
            Debug.LogError("カメラを正常に起動できませんでした");
        }
    }

    void Update()
    {
        if(webcamTexture == null) return;
        var reader = new BarcodeReader();
        int w = webcamTexture.width;
        int h = webcamTexture.height;
        var rawRGB = webcamTexture.GetPixels32();
        var result = reader.Decode(rawRGB, w, h);
        if(result != null)
        {
            Debug.Log(result.Text);
            result = null;
        }
    }
}
