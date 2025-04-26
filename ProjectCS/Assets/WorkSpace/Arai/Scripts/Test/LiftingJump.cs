//======================================================
// [LiftingJump]
// 作成者：荒井修
// 最終更新日：04/26
// 
// [Log]
// 04/26　荒井　キーを入力したらターゲットに向かってぶっ飛んでいくように実装
//======================================================
using UnityEngine;
using UnityEngine.InputSystem;

// ジャンプ操作で目標地点にぶっ飛んでいく挙動のテスト用のスクリプト
public class LiftingJump : MonoBehaviour
{
    [SerializeField] private float JumpImpact = 10f; // ジャンプのインパクト

    [SerializeField] private KeyCode JumpKey = KeyCode.Space; // ジャンプキー

    [SerializeField] GameObject TargetObject; // 目標地点

    [SerializeField] TestMove TestMove; // TestMoveスクリプトの参照

    [SerializeField ]private SlowMotionController SlowMotionController; // スローモーションコントローラーの参照

    private float DefaultSpeed = 0.0f; // デフォルトの移動速度

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TargetObject == null) return;

        // ジャンプキーが押されたとき
        if (Input.GetKeyDown(JumpKey))
        {
            // Rigidbodyの重力を無効化
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.useGravity = false;

            // 目標地点の位置を取得
            Vector3 TargetPosition = TargetObject.transform.position;

            // プレイヤーの位置を取得
            Vector3 PlayerPosition = transform.position;

            // プレイヤーから目標地点へのベクトルを計算
            Vector3 JumpDirection = (TargetPosition - PlayerPosition);

            // 移動ベクトルを設定
            TestMove.SetMoveVector(JumpDirection);

            // 移動速度を設定
            TestMove.SetMoveSpeed(JumpImpact);

            // スローモーションを開始
            SlowMotionController.StartSlowMotion();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Snack")
        {
            // Rigidbodyの重力を有効化
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.useGravity = true;

            // 移動ベクトルと移動速度をリセット
            TestMove.SetMoveVector(Vector3.zero);
            TestMove.SetMoveSpeed(DefaultSpeed);

            // スローモーションを停止
            SlowMotionController.StopSlowMotion();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Snack")
        {
            // Rigidbodyの重力を有効化
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.useGravity = true;

            // 移動ベクトルと移動速度をリセット
            TestMove.SetMoveVector(Vector3.zero);
            TestMove.SetMoveSpeed(DefaultSpeed);

            // スローモーションを停止
            SlowMotionController.StopSlowMotion();
        }
    }
}
