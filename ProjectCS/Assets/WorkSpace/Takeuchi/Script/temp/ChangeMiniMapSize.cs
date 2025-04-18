//====================================================
// スクリプト名：ChangeMiniMapSize
// 作成者：竹内
// 内容：ミニカメラにアタッチすることで
// 　　　キー入力でミニマップのサイズを変えられる
// 最終更新日：04/16
// 
// [Log]
// 04/16 竹内 スクリプト作成
//====================================================
using UnityEngine;

public class ChangeMiniMapSize : MonoBehaviour
{
    private Camera miniMapCamera;
    private float[] sizes = { 20f, 40f, 60f };  // サイズ候補
    private int currentSizeIndex = 0;          // 現在のインデックス
    private float targetSize;                  // 目標サイズ
    [SerializeField] private float smoothSpeed = 5f; // 補間速度（調整可）

    void Start()
    {
        miniMapCamera = GetComponent<Camera>();
        if (miniMapCamera == null)
        {
            Debug.LogError("MiniMapCameraがアタッチされていません。");
            return;
        }

        targetSize = sizes[currentSizeIndex];
        miniMapCamera.orthographicSize = targetSize;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 次のインデックスに移動（ループ）
            currentSizeIndex = (currentSizeIndex + 1) % sizes.Length;
            targetSize = sizes[currentSizeIndex];
        }

        // 滑らかにサイズを補間
        miniMapCamera.orthographicSize = Mathf.Lerp(
            miniMapCamera.orthographicSize,
            targetSize,
            Time.deltaTime * smoothSpeed
        );
    }
}
