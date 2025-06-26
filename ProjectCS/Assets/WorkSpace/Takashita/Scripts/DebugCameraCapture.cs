using UnityEngine;
using System.IO;

public class DebugCameraCapture : MonoBehaviour
{
    public Camera targetCamera; // �B�e����J����
    public int resolutionWidth = 1920;
    public int resolutionHeight = 1080;

    public string outputFileName = "Capture.png";

    [ContextMenu("Capture PNG")]
    public void CaptureToPNG()
    {
        // RenderTexture�̗p��
        RenderTexture rt = new RenderTexture(resolutionWidth, resolutionHeight, 24);
        targetCamera.targetTexture = rt;

        // �J�������蓮�ŕ`��
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = rt;

        targetCamera.Render();

        // Texture2D�ɏ����o��
        Texture2D image = new Texture2D(resolutionWidth, resolutionHeight, TextureFormat.RGB24, false);
        image.ReadPixels(new Rect(0, 0, resolutionWidth, resolutionHeight), 0, 0);
        image.Apply();

        // ��Еt��
        targetCamera.targetTexture = null;
        RenderTexture.active = currentRT;
        Destroy(rt);

        // PNG�Ƃ��ĕۑ�
        byte[] bytes = image.EncodeToPNG();
        string path = Path.Combine(Application.dataPath, outputFileName);
        File.WriteAllBytes(path, bytes);

        Debug.Log("�L���v�`���ۑ�����: " + path);
    }
}
