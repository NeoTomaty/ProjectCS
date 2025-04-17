//====================================================
// スクリプト名：JumpPlayer
// 作成者：高下
// 内容：プレイヤーのジャンプ処理
// 最終更新日：04/01
// 
// [Log]
// 04/01 高下 スクリプト作成 
// 
// 
//====================================================
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpPlayer : MonoBehaviour
{
    [SerializeField]
    private float JumpForce = 10.0f;        // ジャンプ力
    [SerializeField]
    private float GroundCheckRadius = 0.2f; // 地面判定半径
    [SerializeField]
    private Vector3 GravityScale = new Vector3(0.0f, -9.8f, 0.0f);     // 重力の大きさ

    private Rigidbody Rb;    // プレイヤーのRigidbody
    private bool IsGrounded; // 地面に接しているかどうか

    // 地面を確認するためのタグ
    private string GroundTag = "Ground";

    void Start()
    {
        Rb = GetComponent<Rigidbody>(); // Rigidbodyを取得
    }
   
    void Update()
    {
        // 重力方向に加速させる
        Rb.AddForce(GravityScale, ForceMode.Acceleration);

        // 地面に接しているか確認
        IsGrounded = CheckIfGrounded();

        // ジャンプの入力チェック
        bool jumpInputDetected = false;

        // ゲームパッドが接続されている場合、Aボタンを優先
        if (Gamepad.current != null)
        {
            jumpInputDetected = Input.GetKeyDown(KeyCode.JoystickButton0);
        }
        // ゲームパッドが接続されていない場合、スペースキーを使用
        else
        {
            jumpInputDetected = Input.GetKeyDown(KeyCode.Space);
        }

        // ジャンプ処理
        if (jumpInputDetected && IsGrounded)
        {
            Jump();
        }
    }

    // 地面判定
    bool CheckIfGrounded()
    {
        // 地面のタグを持つオブジェクトに接しているか確認
        // プレイヤーの足元から少し下にチェックを行い、半径内で地面のタグがあるか確認
        Collider[] colliders = Physics.OverlapSphere(transform.position - Vector3.up * 0.1f, GroundCheckRadius);

        foreach (var collider in colliders)
        {
            if (collider.CompareTag(GroundTag))
            {
                return true; // 地面に接している場合
            }
        }
        return false; // 地面に接していない場合
    }

    // ジャンプ処理
    void Jump()
    {
        // ジャンプ力を加える
        Rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        Debug.Log("ジャンプ処理成功");
    }
}
