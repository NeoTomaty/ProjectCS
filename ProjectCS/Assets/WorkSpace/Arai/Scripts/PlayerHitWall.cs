//======================================================
// [PlayerHitWall]
// 作成者：荒井修
<<<<<<< HEAD:ProjectCS/Assets/WorkSpace/Arai/Scripts/PlayerHitWall.cs
// 最終更新日：4/07
=======
// 最終更新日：04/08
>>>>>>> 0722a15c18f14d9d7c606093145b759313198c29:ProjectCS/Assets/WorkSpace/Arai/Script/PlayerHitWall.cs
// 
// [Log]
// 03/31　荒井　プレイヤーが壁に衝突した際の挙動を作成
// 03/31　荒井　移動の仮スクリプトを自作し動作を確認
// 04/01　荒井　Playerオブジェクトの本スクリプトに対応
<<<<<<< HEAD:ProjectCS/Assets/WorkSpace/Arai/Scripts/PlayerHitWall.cs
// 04/07　荒井　Tooltip記述をコメントに変更
=======
// 04/08　髙下　壁反射の仕様を変更
>>>>>>> 0722a15c18f14d9d7c606093145b759313198c29:ProjectCS/Assets/WorkSpace/Arai/Script/PlayerHitWall.cs
//======================================================

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerHitWall : MonoBehaviour
{
    // 壁に衝突した際の加速量
    [SerializeField] private float Acceleration = 1.0f;

    [SerializeField]
    private float MaxVelocityY = 50.0f;  // Y軸最大の速さ
    [SerializeField]
    private float MinVelocityY = -50.0f; // Y軸最小の速さ
    [SerializeField]
    private float InputAcceptDuration = 1.0f;// 壁に衝突後の入力受付時間
    private float InputAcceptTimer = 0.0f;   // 入力受付の経過時間
    [Tooltip("入力による加速を実行するかどうか")]
    [SerializeField]
    private bool IsInputEnabled = true;

    private bool IsJumpReflect = false;  // ジャンプ時の壁反射で力を加えるかどうか
    private bool IsHitWall = false;      // 壁に衝突したかどうか

    private Rigidbody Rb;    // プレイヤーのRigidbody

    // プレイヤーの移動方向と速度にアクセスするための変数
    // 同じオブジェクトにアタッチされているスクリプトであるという想定での実装
    MovePlayer MovePlayerScript;    //実際のスクリプト

    void Start()
    {
        MovePlayerScript = GetComponent<MovePlayer>();
        Rb = GetComponent<Rigidbody>(); // Rigidbodyを取得

        if (MovePlayerScript == null)
        {
            Debug.LogError("プレイヤー >> MovePlayerスクリプトが見つかりません");
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


    private void OnCollisionEnter(Collision collision)
    {
        if (MovePlayerScript == null) return;

        // 衝突したオブジェクトのタグをチェック
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "BrokenWall")
        {
<<<<<<< HEAD:ProjectCS/Assets/WorkSpace/Arai/Scripts/PlayerHitWall.cs
=======
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

>>>>>>> origin/Takashita:ProjectCS/Assets/WorkSpace/Arai/Script/PlayerHitWall.cs
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
}
