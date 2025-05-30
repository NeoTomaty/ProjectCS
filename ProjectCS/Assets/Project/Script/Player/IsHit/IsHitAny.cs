//======================================================
// [PlayerHitWall]
// 作成者：荒井修
// 最終更新日：05/30
// 
// 内容：プレイヤーが何かにあたった際の処理
// 　　　プレイヤーオブジェクトにアタッチする
// 
// [Log]
// 03/31　荒井　プレイヤーが壁に衝突した際の挙動を作成
// 03/31　荒井　移動の仮スクリプトを自作し動作を確認
// 04/01　荒井　Playerオブジェクトの本スクリプトに対応
// 04/07　荒井　Tooltip記述をコメントに変更
// 04/08　髙下　壁反射の仕様を変更
// 04/11　中町　GoalWallに衝突したときにTestResultSceneにシーン遷移する処理追加
// 04/23　竹内　プレイヤーがPlayerタグに触れたときも加速するように対応
// 04/24　竹内　スクリプト名を改名
// 04/27　荒井　リフティングパートに関する処理を追加
// 04/30　竹内　Playerタグ及びGoalWallタグに当たった際の挙動を削除
// 04/30　竹内　RespawnAreaタグに当たった際の挙動を追加
// 05/30　中町　リスポーンSE実装
//======================================================

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement; // シーン管理を追加

public class IsHitAny : MonoBehaviour
{
    // 壁に衝突した際の加速量
    [Header("壁に衝突したときの加速度")]
    [SerializeField] private float Acceleration = 1.0f;

    //Y方向の速度制限(壁反射時の力の調整に使用)
    [Header("Y方向の速度")]
    [SerializeField]
    private float MaxVelocityY = 50.0f;  // Y軸最大の速さ
    [SerializeField]
    private float MinVelocityY = -50.0f; // Y軸最小の速さ

    //壁に当たった後にボタン入力で加速できるかどうか
    [Header("ボタン入力による加速を実行するかどうか")]
    [SerializeField]
    private bool IsInputEnabled = true;

    //入力受付時間の設定(壁に当たってから何秒間入力を受け付けるか)
    [Header("ボタン入力まわりの設定")]
    [SerializeField]
    private float InputAcceptDuration = 1.0f;// 壁に衝突後の入力受付時間
    private float InputAcceptTimer = 0.0f;   // 入力受付の経過時間


    // ワープ先の座標
    [Header("リスポーン先の座標")]
    [SerializeField]
    public Vector3 warpPosition = new Vector3(0, 0, 0);


    private bool IsJumpReflect = false;  // ジャンプ時の壁反射で力を加えるかどうか
    private bool IsHitWall = false;      // 壁に衝突したかどうか

    private Rigidbody Rb;    // プレイヤーのRigidbody

    private LiftingJump LiftingJumpScript; // リフティングジャンプのスクリプト


    // プレイヤーの移動方向と速度にアクセスするための変数
    // 同じオブジェクトにアタッチされているスクリプトであるという想定での実装
    MovePlayer MovePlayerScript;    //実際のスクリプト

    //リスポーン時に再生するSE(サウンドエフェクト)
    [Header("リスポーン時のSE")]
    [SerializeField] private AudioClip RespawnSE;

    //SEを再生するためのAudioSource
    private AudioSource audioSource;

    void Start()
    {
        MovePlayerScript = GetComponent<MovePlayer>();
        Rb = GetComponent<Rigidbody>(); // Rigidbodyを取得
        LiftingJumpScript = GetComponent<LiftingJump>(); // LiftingJumpを取得

        if (MovePlayerScript == null)
        {
            Debug.LogError("プレイヤー >> MovePlayerスクリプトが見つかりません");
        }

        //AudioSourceがアタッチされていなければ追加
        audioSource = GetComponent<AudioSource>();

        if(audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (!IsHitWall) return;

        if (InputAcceptTimer > 0.0f)
        {
            InputAcceptTimer -= Time.deltaTime;

            // ジャンプの入力チェック
            bool AccelerationInputDetected = false;

            // ゲームパッドが接続されている場合、Bボタン（仮）を優先
            if (Gamepad.current != null)
            {
                AccelerationInputDetected = Input.GetKeyDown(KeyCode.JoystickButton1);
            }
            // ゲームパッドが接続されていない場合、エンターキー（仮）を使用
            else
            {
                AccelerationInputDetected = Input.GetKeyDown(KeyCode.Return);
            }

            // 加速処理
            if(AccelerationInputDetected)
            {
                // プレイヤーを加速
                MovePlayerScript.PlayerSpeedManager.SetAccelerationValue(Acceleration);
                Debug.Log("加速成功");
            }
        }
        else
        {
            IsHitWall = false;
        }
    }

    // 衝突した際の処理
    private void OnCollisionEnter(Collision collision)
    {
        if (MovePlayerScript == null) return;

        // 衝突したオブジェクトのタグをチェック
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "BrokenWall")
        {
            Debug.Log("プレイヤー >> 壁に当たりました");
            
            // 入力による加速をしないのなら、そのまま加速処理を実行
            if (!IsInputEnabled)
            {
                // プレイヤーを加速
                MovePlayerScript.PlayerSpeedManager.SetAccelerationValue(Acceleration);
            }
            else
            {
                IsHitWall = true;
                InputAcceptTimer = InputAcceptDuration;
            }

            // プレイヤーの移動ベクトルを取得
            Vector3 PlayerMoveDirection = MovePlayerScript.GetMoveDirection;

            // 壁の接触面の法線ベクトルを取得
            Vector3 Normal = collision.contacts[0].normal;

            // 反射ベクトルを計算
            Vector3 Reflect = Vector3.Reflect(PlayerMoveDirection, Normal).normalized;

            // 反射ベクトルをプレイヤーに適用
            MovePlayerScript.SetMoveDirection(Reflect);

            // ヒットストップ実行
            MovePlayerScript.StartHitStop();

            if (!IsJumpReflect) return;

            // 壁反射後の力の方向をVelocityに応じて決定する
            Reflect = new Vector3(0.0f, Mathf.Clamp(Rb.linearVelocity.y, MinVelocityY, MaxVelocityY), 0.0f);

            // 壁反射後に一定の力を加える
            Rb.AddForce(Reflect, ForceMode.Impulse);

            // 壁反射時のAddForceを無効にする
            IsJumpReflect = false;
        }
        // 地面に着いた場合は変化したMoveDirectionYを0にする
        else if (collision.gameObject.tag == "Ground")
        {
            // プレイヤーの移動ベクトルを取得
            Vector3 PlayerMoveDirection = MovePlayerScript.GetMoveDirection;

            // PlayerMoveDirection.yを0にリセット
            PlayerMoveDirection.y = 0.0f;

            // PlayerMoveDirectionの更新
            MovePlayerScript.SetMoveDirection(PlayerMoveDirection);

            // 壁反射時のAddForceを有効にする
            IsJumpReflect = true;
        }

    }

    // 衝突した際の処理（is Trigger）
    private void OnTriggerEnter(Collider other)
    {
        // 衝突したオブジェクトのタグをチェック
        if (other.gameObject.tag == "Respawn")
        {
            // オブジェクトの位置をwarpPositionに変更する
            transform.position = warpPosition;

            //リスポーンSEを再生
            if(RespawnSE != null && audioSource != null)
            {
                audioSource.PlayOneShot(RespawnSE);
            }
        }
    }

}
