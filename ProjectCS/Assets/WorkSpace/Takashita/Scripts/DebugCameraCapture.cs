using UnityEngine;
using System.IO;

public class DebugCameraCapture : MonoBehaviour
{
    public Camera targetCamera; // 撮影するカメラ
    public int resolutionWidth = 1920;
    public int resolutionHeight = 1080;

    public string outputFileName = "Capture.png";

    [ContextMenu("Capture PNG")]
    public void CaptureToPNG()
    {
        // RenderTextureの用意
        RenderTexture rt = new RenderTexture(resolutionWidth, resolutionHeight, 24);
        targetCamera.targetTexture = rt;

        // カメラを手動で描画
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = rt;

        targetCamera.Render();

        // Texture2Dに書き出し
        Texture2D image = new Texture2D(resolutionWidth, resolutionHeight, TextureFormat.RGB24, false);
        image.ReadPixels(new Rect(0, 0, resolutionWidth, resolutionHeight), 0, 0);
        image.Apply();

        // 後片付け
        targetCamera.targetTexture = null;
        RenderTexture.active = currentRT;
        Destroy(rt);

        // PNGとして保存
        byte[] bytes = image.EncodeToPNG();
        string path = Path.Combine(Application.dataPath, outputFileName);
        File.WriteAllBytes(path, bytes);

        Debug.Log("キャプチャ保存完了: " + path);
    }
}
