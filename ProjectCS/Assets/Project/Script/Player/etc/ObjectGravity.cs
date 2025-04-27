//====================================================
// スクリプト名：ObjectGravity
// 作成者：高下
// 内容：オブジェクトの重力を管理するクラス
// 最終更新日：04/21
// 
// [Log]
// 04/21 高下 スクリプト作成 
// 04/27 荒井 アクティブフラグを追加 
// 
//====================================================
using UnityEngine;

public class ObjectGravity : MonoBehaviour
{
    [SerializeField]
    private Vector3 GravityScale = new Vector3(0.0f, -9.8f, 0.0f);     // 重力の大きさ

    private Rigidbody Rb;    // オブジェクトのRigidbody

    public bool IsActive = true;

    void Start()
    {
        Rb = GetComponent<Rigidbody>(); // Rigidbodyを取得
    }

    void Update()
    {
        if (!IsActive) return;
        // 重力方向に加速させる
        Rb.AddForce(GravityScale, ForceMode.Acceleration);
    }
}
