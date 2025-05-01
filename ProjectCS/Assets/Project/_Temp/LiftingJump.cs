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
// 05/01　荒井　ターゲット以外のオブジェクトに衝突したらリフティングジャンプを中止する処理を追加
// 05/01　荒井　ターゲット以外のオブジェクトをすり抜ける処理を追加
// 05/01　荒井　中止とすり抜けを切り替えるパラメータを追加
//======================================================
using UnityEngine;
using UnityEngine.InputSystem;

// ジャンプ操作で目標地点にぶっ飛んでいく挙動のテスト用のスクリプト
// プレイヤーにアタッチ
public class LiftingJump : MonoBehaviour
{
    [SerializeField] GameObject TargetObject;   // 目標地点

    private MovePlayer MovePlayer;              // プレイヤーの移動スクリプトの参照

    private ObjectGravity ObjectGravityScript;  // 重力スクリプトの参照

    [SerializeField] private GaugeController GaugeController;   // ゲージコントローラーの参照

    [SerializeField] private float BaseJumpPower = 10f; // 基となるジャンプの速度

    private float JumpPower = 0f;
    public float GetJumpPower => JumpPower; // ジャンプ力の取得

    //[SerializeField] private float BaseSpeed = 20f; // 衝突後の移動速度

    [SerializeField] private float BaseForce = 10f; // 衝突後の力
    public float GetForce => BaseForce * GaugeController.GetGaugeValue;

    [SerializeField] private float SlowMotionFactor = 0.1f; //スローモーションの度合い

    [SerializeField] private float SlowMotionDistance = 1f; // スローモーションへ移行する距離

    [SerializeField] private bool IgnoreNonTargetCollisions = false;    // ターゲット以外との衝突を無視するかどうか

    private Collider[] AllColliders;    // 全オブジェクトの当たり判定

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
        if(IgnoreNonTargetCollisions)   // すり抜け有効時
        {
            Collider SelfCollider = GetComponent<Collider>();                   // 自分のコライダーを取得
            Collider TargetCollider = TargetObject.GetComponent<Collider>();    // ターゲットのコライダーを取得

            foreach (Collider col in AllColliders)
            {
                // 自分のコライダーと全てのコライダーの当たり判定を無視
                Physics.IgnoreCollision(SelfCollider, col, true);
            }

            // 自分のコライダーとターゲットのコライダーの当たり判定だけ有効
            Physics.IgnoreCollision(SelfCollider, TargetCollider, false);
        }

        ObjectGravityScript.IsActive = false;

        // プレイヤーから目標地点へのベクトルを計算
        Vector3 JumpDirection = (TargetObject.transform.position - transform.position);

        GetComponent<Rigidbody>().AddForce(JumpDirection.normalized * BaseJumpPower * JumpPower, ForceMode.Impulse);

        IsJumping = true;
    }

    // リフティングジャンプを停止する関数
    public void FinishLiftingJump()
    {
        if (IgnoreNonTargetCollisions)  // すり抜け有効時
        {
            Collider SelfCollider = GetComponent<Collider>();   // 自分のコライダーを取得

            foreach (Collider col in AllColliders)
            {
                // 自分のコライダーと全てのコライダーの当たり判定を有効にする
                Physics.IgnoreCollision(SelfCollider, col, false);
            }
        }

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
        MovePlayer = GetComponent<MovePlayer>();
        ObjectGravityScript = GetComponent<ObjectGravity>();

        // 全オブジェクトの当たり判定を取得
        AllColliders = FindObjectsByType<Collider>(FindObjectsSortMode.None);
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

    private void OnCollisionEnter(Collision collision)
    {
        if (IgnoreNonTargetCollisions) return;

        // ターゲット以外のオブジェクトに衝突した場合
        if (collision.gameObject != TargetObject)
        {
            // リフティングジャンプを終了
            FinishLiftingJump();
        }
    }
}
