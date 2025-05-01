//====================================================
// スクリプト名：MiniMapCamera
// 作成者：竹内
// 内容：特定のオブジェクトをミニマップに常に表示させる
// 　　　ミニマップとなるカメラにアタッチする
//
// [Log]
// 04/16 竹内 スクリプト作成
// 04/26 竹内 スクリプトを根本的に仕様変更
// 
//====================================================
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    [Header("表示対象")]
    [SerializeField] private Transform target;                          // 追従対象
    [Header("カメラの相対距離")]
    [SerializeField] private Vector3 offset = new Vector3(0, 10, 0);    // 追従時の相対位置
    [Header("ミニマップを表示するまでの高さ")]
    [SerializeField] private float toggleHeight = 100;    // カメラを表示させる高さ

    private Camera miniMapCamera;    // このスクリプトがアタッチされているミニマップカメラ

    // 初期化
    void Start()
    {
        // ターゲットがアタッチされていなければ
        if (target == null)
        {
            Debug.LogError("MiniMapCamera：Targetが未設定です");
            enabled = false;
            return;
        }

        // ミニマップとなるカメラがなければ
        miniMapCamera = GetComponent<Camera>();
        if (miniMapCamera == null)
        {
            Debug.LogError("MiniMapCamera：Cameraコンポーネントがありません");
            enabled = false;
            return;
        }
    }

    // 最後に呼び出されるUpdate処理
    void LateUpdate()
    {
        FollowTarget();
        ToggleMiniMap();
    }

    // ターゲットを追従する
    private void FollowTarget()
    {
        transform.position = target.position + offset;
        transform.rotation = Quaternion.Euler(0f, 90f, 0f); // 真横からの角度
    }

    // 条件に応じてミニマップをON/OFFする
    private void ToggleMiniMap()
    {
        if (target.position.y > toggleHeight)
        {
            miniMapCamera.enabled = true;
        }
        else
        {
            miniMapCamera.enabled = false;
        }
    }
}