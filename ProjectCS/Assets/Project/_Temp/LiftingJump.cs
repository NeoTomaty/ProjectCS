//======================================================
// [LiftingJump]
// 作成者：荒井修
// 最終更新日：04/29
// 
// [Log]
// 04/26　荒井　キーを入力したらターゲットに向かってぶっ飛んでいくように実装
// 04/27　荒井　ターゲットに近づいたらスローモーションが始まるように実装
// 04/27　荒井　他スクリプトと合わせて動作するように修正
// 04/28　荒井　ジャンプ時の移動をtransformからAddForceに変更
// 04/29　荒井　チャージジャンプによるジャンプのパワー補正を追加
//======================================================
using UnityEngine;
using UnityEngine.InputSystem;

// ジャンプ操作で目標地点にぶっ飛んでいく挙動のテスト用のスクリプト
// プレイヤーにアタッチ
public class LiftingJump : MonoBehaviour
{
    [SerializeField] GameObject TargetObject; // 目標地点

    private MovePlayer MovePlayer; // プレイヤーの移動スクリプトの参照

    private ObjectGravity ObjectGravityScript;

    [SerializeField] private GaugeController GaugeController; // ゲージコントローラーの参照

    [SerializeField] private float BaseJumpPower = 10f; // 基となるジャンプの速度

    private float JumpPower = 0f;
    public float GetJumpPower => JumpPower; // ジャンプ力の取得

    //[SerializeField] private float BaseSpeed = 20f; // 衝突後の移動速度

    [SerializeField] private float BaseForce = 10f; // 衝突後の力
    public float GetForce => BaseForce * GaugeController.GetGaugeValue;

    [SerializeField] private float SlowMotionFactor = 0.1f; //スローモーションの度合い

    [SerializeField] private float SlowMotionDistance = 1f; // スローモーションへ移行する距離

    private bool IsJumping = false;

    private bool IsNearTargetLast = false; // ターゲットに近づいたかどうか

    public void ResetGaugeValue()
    {
        GaugeController.SetGaugeValue(0f);
    }

    public void SetJumpPower(float Power)
    {
        JumpPower = Power;
    }

    // リフティングジャンプを開始する関数
    public void StartLiftingJump()
    {
        ObjectGravityScript.IsActive = false;

        // プレイヤーから目標地点へのベクトルを計算
        Vector3 JumpDirection = (TargetObject.transform.position - transform.position);

        GetComponent<Rigidbody>().AddForce(JumpDirection.normalized * BaseJumpPower * JumpPower, ForceMode.Impulse);

        IsJumping = true;
    }

    // リフティングジャンプを停止する関数
    public void FinishLiftingJump()
    {
        ObjectGravityScript.IsActive = true;

        IsJumping = false;

        // ゲージを停止
        GaugeController.Stop();

        // スローモーションを停止
        Time.timeScale = 1.0f;
    }

    // ターゲットに近づいた瞬間を判定する関数
    private bool IsNearTargetEnter()
    {
        // ターゲットとの距離を計算
        float distance = Vector3.Distance(transform.position, TargetObject.transform.position);

        if (distance < SlowMotionDistance)
        {
            if (!IsNearTargetLast)
            {
                IsNearTargetLast = true;
                return true;
            }
        }
        else
        {
            IsNearTargetLast = false;
        }

        return false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MovePlayer=GetComponent<MovePlayer>();
        ObjectGravityScript = GetComponent<ObjectGravity>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TargetObject == null) return;

        // 上昇中
        if (IsJumping)
        {
            // プレイヤーから目標地点へのベクトルを計算
            Vector3 JumpDirection = (TargetObject.transform.position - transform.position);
            // 移動ベクトルを設定
            MovePlayer.SetMoveDirection(JumpDirection.normalized);

            // 一定距離までターゲットに近づいたらスローモーションを開始
            if (IsNearTargetEnter())
            {
                // スローモーションを開始
                Time.timeScale = SlowMotionFactor;

                // ゲージを表示
                GaugeController.Play();
            }
        }
    }
}
