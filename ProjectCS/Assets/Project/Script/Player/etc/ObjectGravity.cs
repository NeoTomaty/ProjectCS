//====================================================
// スクリプト名：ObjectGravity
// 作成者：高下
// 内容：オブジェクトの重力を管理するクラス
// 最終更新日：05/09
// 
// [Log]
// 04/21 高下 スクリプト作成 
// 04/27 荒井 アクティブフラグを追加 
// 05/09 高下 落下速度の制御を追加
//====================================================
using UnityEngine;

public class ObjectGravity : MonoBehaviour
{
    [SerializeField]
    private Vector3 GravityScale = new Vector3(0.0f, -9.8f, 0.0f);     // 重力の大きさ

    [SerializeField]
    private float MaxFallSpeed = 20.0f; // 最大落下速度（負の値）

    private Rigidbody Rb;    // オブジェクトのRigidbody

    public bool IsActive = true;

    void Start()
    {
        Rb = GetComponent<Rigidbody>(); // Rigidbodyを取得
    }

    void Update()
    {
        if (!IsActive) return;
        // 重力
        Rb.AddForce(GravityScale, ForceMode.Acceleration);

        // 落下速度を制限
        if (Rb.linearVelocity.y < -MaxFallSpeed)
        {
            Vector3 clampedVelocity = Rb.linearVelocity;
            clampedVelocity.y = -MaxFallSpeed;
            Rb.linearVelocity = clampedVelocity;
        }
    }
}
