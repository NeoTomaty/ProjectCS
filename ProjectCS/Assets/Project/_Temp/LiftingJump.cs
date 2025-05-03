//======================================================
// [LiftingJump]
// 作成者：荒井修
// 最終更新日：05/03
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
// 05/02　荒井　リフティングジャンプ中にターゲットの動きを止める処理を追加
// 05/03　荒井　ジャンプ時の移動をAddForceからtransformに変更
// 05/03　荒井　スローモーションの制御方法をtimeScaleからPlayerSpeedManagerに変更
// 05/03　荒井　スローモーションが開始された時に落ちていくようになるバグを修正
// 05/03　荒井　リフティングジャンプ中に左右移動や減速等の操作を無効にする処理を追加
//======================================================
using UnityEngine;
using UnityEngine.InputSystem;

// ジャンプ操作で目標地点にぶっ飛んでいく挙動のテスト用のスクリプト
// プレイヤーにアタッチ
public class LiftingJump : MonoBehaviour
{
    [SerializeField] GameObject TargetObject;                   // 目標地点
    [SerializeField] private GaugeController GaugeController;   // ゲージコントローラーの参照
    private MovePlayer MovePlayer;                              // プレイヤーの移動スクリプトの参照
    private PlayerSpeedManager PlayerSpeedManager;              // プレイヤーの移動速度を管理するスクリプトの参照
    private ObjectGravity ObjectGravityScript;                  // 重力スクリプトの参照
    private PlayerInput PlayerInput;                            // プレイヤーの入力を管理するcomponent

    [SerializeField] private float BaseJumpPower = 10f; // 基となるジャンプの速度

    private float JumpPower = 0f;
    public float GetJumpPower => JumpPower; // ジャンプ力の取得

    private float BaseSpeed = 0f; // 元の移動速度

    [SerializeField] private float BaseForce = 10f; // 衝突後の力
    public float GetForce => BaseForce * GaugeController.GetGaugeValue;

    [SerializeField] private float SlowMotionFactor = 0.1f; //スローモーションの度合い
    [SerializeField] private float SlowMotionDistance = 1f; // スローモーションへ移行する距離

    private bool IgnoreNonTargetCollisions = false;    // ターゲット以外との衝突を無視するかどうか（上手く動作しなくなったためSerializeFieldから除外）

    private Collider[] AllColliders;    // 全オブジェクトの当たり判定

    private bool IsJumping = false;
    public bool IsLiftingPart => IsJumping; // リフティングジャンプ中かどうか

    private bool IsNearTargetLast = false; // ターゲットに近づいたかどうか

    // スローモーションのオンオフを切り替える関数
    private void SetSlowMotion(bool Enabled)
    {
        if (Enabled)
        {
            // スローモーションを開始
            PlayerSpeedManager.SetOverSpeed((BaseJumpPower * JumpPower) * SlowMotionFactor); // スローモーション中の移動速度を設定
        }
        else
        {
            // スローモーションを終了
            PlayerSpeedManager.SetOverSpeed(BaseJumpPower * JumpPower); // 移動速度を戻す
        }
    }

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
        PlayerInput.actions.Disable(); // 入力を無効にする

        if (IgnoreNonTargetCollisions)   // すり抜け有効時
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

        // ターゲットの動きを止める
        TargetObject.GetComponent<Rigidbody>().isKinematic = true;

        ObjectGravityScript.IsActive = false;
        GetComponent<Rigidbody>().Sleep();  // 自分のRigidbodyをスリープ状態にする

        IsJumping = true;

        // プレイヤーから目標地点へのベクトルを計算
        Vector3 JumpDirection = (TargetObject.transform.position - transform.position);
        MovePlayer.SetMoveDirection(JumpDirection.normalized);

        // プレイヤーを加速させる
        BaseSpeed = PlayerSpeedManager.GetPlayerSpeed; // 元の移動速度を保存
        PlayerSpeedManager.SetOverSpeed(BaseJumpPower * JumpPower);
    }

    // リフティングジャンプを停止する関数
    public void FinishLiftingJump()
    {
        PlayerInput.actions.Enable(); // 入力を有効にする

        if (IgnoreNonTargetCollisions)  // すり抜け有効時
        {
            Collider SelfCollider = GetComponent<Collider>();   // 自分のコライダーを取得

            foreach (Collider col in AllColliders)
            {
                // 自分のコライダーと全てのコライダーの当たり判定を有効にする
                Physics.IgnoreCollision(SelfCollider, col, false);
            }
        }

        // ターゲットの動きを止めたのを解除
        TargetObject.GetComponent<Rigidbody>().isKinematic = false;

        ObjectGravityScript.IsActive = true;

        IsJumping = false;

        // ゲージを停止
        GaugeController.Stop();

        // 上昇を止める
        Vector3 MoveDirection = MovePlayer.GetMoveDirection;    // 現在の移動方向を取得
        MoveDirection.y = 0;                                    // Y軸の移動を無効にする
        MovePlayer.SetMoveDirection(MoveDirection.normalized);  // 移動方向を設定

        // 移動速度を元に戻す
        PlayerSpeedManager.SetSpeed(BaseSpeed);
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

    private void Awake()
    {
        // 自分にアタッチされているPlayerInputを取得
        PlayerInput = GetComponent<PlayerInput>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MovePlayer = GetComponent<MovePlayer>();
        PlayerSpeedManager = GetComponent<PlayerSpeedManager>();
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
            // 一定距離までターゲットに近づいたらスローモーションを開始
            if (IsNearTargetEnter())
            {
                // スローモーションを開始
                SetSlowMotion(true);

                // ゲージを表示
                GaugeController.Play();
            }else if(GaugeController.IsFinishEnter())
            {
                // スローモーションを終了
                SetSlowMotion(false);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IgnoreNonTargetCollisions) return;

        if (!IsJumping) return;

        // ターゲット以外のオブジェクトに衝突した場合
        if (collision.gameObject != TargetObject)
        {
            // リフティングジャンプを終了
            FinishLiftingJump();
        }
    }
}
