//====================================================
// スクリプト名：JumpPlayer
// 作成者：高下
// 内容：プレイヤーのジャンプ処理
// 最終更新日：04/01
// 
// [Log]
// 04/01 高下 スクリプト作成 
// 04/16 竹内 ジャンプ回数を制御する処理を追加
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

    [SerializeField]
    private int MaxJumpNum = 1;     // ジャンプの最大回数

    [SerializeField]
    private int CurrentJumpNum = 0; // 現在のジャンプ可能回数

    private Rigidbody Rb;    // プレイヤーのRigidbody
    private bool IsGrounded; // 地面に接しているかどうか

    // 地面を確認するためのタグ
    private string GroundTag = "Ground";

    void Start()
    {
        Rb = GetComponent<Rigidbody>(); // Rigidbodyを取得
        CurrentJumpNum = MaxJumpNum;    // ジャンプ回数を指定
    }
   
    void Update()
    {
        // 重力方向に加速させる
        Rb.AddForce(GravityScale, ForceMode.Acceleration);


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

        // ジャンプキー及び地面に接していて、ジャンプ回数が残っていれば
        if (jumpInputDetected && IsGrounded && (CurrentJumpNum > 0))
        {
            // デバッグログ
            Debug.Log("ジャンプ消費");
            // ジャンプ処理
            Jump();
            // ジャンプ回数を減らす
            CurrentJumpNum--;
        }
    }

    // 接触したとき
    void OnCollisionEnter(Collision collision)
    {
        // 地面と接触したときのみジャンプ回数を回復
        if (collision.gameObject.CompareTag(GroundTag))
        {
            Debug.Log("ジャンプ回数回復");
            CurrentJumpNum = MaxJumpNum;
            IsGrounded = true;
        }
    }

    // 接触しなくなった瞬間
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(GroundTag))
        {
            IsGrounded = false;
        }
    }


    // ジャンプ処理
    void Jump()
    {
        // ジャンプ力を加える
        Rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        // デバッグログ
        Debug.Log("ジャンプ処理成功");
    }
}
