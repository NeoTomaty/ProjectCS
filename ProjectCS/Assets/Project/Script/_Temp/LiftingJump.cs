//======================================================
// [LiftingJump]
// 作成者：荒井修
// 最終更新日：05/29
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
// 05/07　荒井　すり抜けモードが有効なのにオブジェクトをすり抜けられないバグを修正
// 05/07　荒井　クリアカウントで多段ヒット扱いされる挙動を修正
// 05/15　荒井　移動速度の変化の操作先をPlayerSpeedManagerからMovePlayerに変更
// 05/28　荒井　IsNearTargetNextFrame関数を追加
// 05/28　荒井　リフティングジャンプでスピードが速いとQTEが発動しないことがあるバグを修正
// 05/29　宮林　ポーズ画面表示ボタンの停止
// 05/29　荒井　スクリプト実行順の優先度を設定
// 05/29　森脇　モデルのフラグ変化
// 05/29　荒井　QTE廃止
// 06/05　荒井　高さを条件としてリフティングジャンプを強制終了する処理を追加
//======================================================
using UnityEngine;
using UnityEngine.InputSystem;

// ジャンプ操作で目標地点にぶっ飛んでいく挙動のテスト用のスクリプト
// プレイヤーにアタッチ
public class LiftingJump : MonoBehaviour
{
    [SerializeField] private GameObject TargetObject;                   // 目標地点
    private MovePlayer MovePlayer;                              // プレイヤーの移動スクリプトの参照
    private ObjectGravity ObjectGravityScript;                  // 重力スクリプトの参照
    private PlayerInput PlayerInput;                            // プレイヤーの入力を管理するcomponent
    public PlayerInput PauseInput;                              //ポーズ画面の操作受け取り

    [SerializeField] private float JumpSpeed = 2f;  // ジャンプ時の移動速度補正

    private float JumpPower = 0f;
    public float GetJumpPower => JumpPower; // ジャンプ力の取得

    [SerializeField] private bool IgnoreNonTargetCollisions = false;    // ターゲット以外との衝突を無視するかどうか
    public bool IsIgnore => IgnoreNonTargetCollisions && IsJumping;

    private Collider[] AllColliders;    // 全オブジェクトの当たり判定

    private bool IsJumping = false;
    public bool IsLiftingPart => IsJumping; // リフティングジャンプ中かどうか

    private float OnStartedPlayerHeight = 0f; // リフティングジャンプ開始時のプレイヤーの高さ
    private float TerminateHeight = 200f; // リフティングジャンプを強制的に終了させる高度

    //[SerializeField] private PlayerAnimationController playerAnimController;

    public void SetJumpPower(float Power)
    {
        JumpPower = Power;
    }

    // リフティングジャンプを開始する関数
    public void StartLiftingJump()
    {
        PlayerInput.actions.Disable(); // 入力を無効にする
        PauseInput.actions.Disable(); // 入力を無効にする
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
        TargetObject.GetComponent<ObjectGravity>().IsActive = false;
        TargetObject.GetComponent<Rigidbody>().Sleep(); // ターゲットのRigidbodyをスリープ状態にする（isKinematicだと多段ヒット扱いでヒットカウントが進みすぎる）

        ObjectGravityScript.IsActive = false;
        GetComponent<Rigidbody>().Sleep();  // 自分のRigidbodyをスリープ状態にする（スローモーション中に落ちていく対策）

        IsJumping = true;

        // プレイヤーの高さを保存
        OnStartedPlayerHeight=transform.position.y;

        // プレイヤーから目標地点へのベクトルを計算
        Vector3 JumpDirection = (TargetObject.transform.position - transform.position);
        MovePlayer.SetMoveDirection(JumpDirection.normalized);

        // プレイヤーを加速させる
        MovePlayer.MoveSpeedMultiplier = JumpSpeed * JumpPower;
    }

    // リフティングジャンプを停止する関数
    public void FinishLiftingJump()
    {
        PlayerInput.actions.Enable(); // 入力を有効にする
        PauseInput.actions.Enable(); // 入力を有効にする
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
        TargetObject.GetComponent<ObjectGravity>().IsActive = true;

        ObjectGravityScript.IsActive = true;

        IsJumping = false;

        // 上昇を止める
        Vector3 MoveDirection = MovePlayer.GetMoveDirection;    // 現在の移動方向を取得
        MoveDirection.y = 0;                                    // Y軸の移動を無効にする
        MovePlayer.SetMoveDirection(MoveDirection.normalized);  // 移動方向を設定

        // 移動速度を元に戻す
        MovePlayer.MoveSpeedMultiplier = 1f;

        //playerAnimController.PlayRandomAnimation();
    }

    private void Awake()
    {
        // 自分にアタッチされているPlayerInputを取得
        PlayerInput = GetComponent<PlayerInput>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        MovePlayer = GetComponent<MovePlayer>();
        ObjectGravityScript = GetComponent<ObjectGravity>();

        // 全オブジェクトの当たり判定を取得
        AllColliders = FindObjectsByType<Collider>(FindObjectsSortMode.None);
    }

    // Update is called once per frame
    private void Update()
    {
        if (IsJumping)
        {
            // プレイヤーの高さが強制終了高度を超えたらリフティングジャンプ終了
            if (transform.position.y > OnStartedPlayerHeight + TerminateHeight)
            {
                FinishLiftingJump();
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