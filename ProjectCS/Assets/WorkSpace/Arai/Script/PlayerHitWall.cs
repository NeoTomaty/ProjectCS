//======================================================
// [PlayerHitWall]
// 作成者：荒井修
// 最終更新日：04/08
// 
// [Log]
// 03/31　荒井　プレイヤーが壁に衝突した際の挙動を作成
// 03/31　荒井　移動の仮スクリプトを自作し動作を確認
// 04/01　荒井　Playerオブジェクトの本スクリプトに対応
// 04/08　髙下　壁反射の仕様を変更
//======================================================

using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerHitWall : MonoBehaviour
{
    [Tooltip("壁に衝突した際の加速量")]
    [SerializeField] private float Acceleration = 1.0f;

    [SerializeField]
    private float MaxVelocityY = 50.0f;  // Y軸最大の速さ
    [SerializeField]
    private float MinVelocityY = -50.0f; // Y軸最小の速さ

    private bool IsJumpReflect = false;  // ジャンプ時の壁反射で力を加えるかどうか

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

   
    private void OnCollisionEnter(Collision collision)
    {
        if (MovePlayerScript == null) return;

        // 衝突したオブジェクトのタグをチェック
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "BrokenWall")
        {
            Debug.Log("プレイヤー >> 壁に当たりました");

            // プレイヤーの移動ベクトルを取得
            Vector3 PlayerMoveDirection = MovePlayerScript.GetMoveDirection;

            // 壁の接触面の法線ベクトルを取得
            Vector3 Normal = collision.contacts[0].normal;

            // 反射ベクトルを計算
            Vector3 Reflect = Vector3.Reflect(PlayerMoveDirection, Normal).normalized;

            // 反射ベクトルをプレイヤーに適用
            MovePlayerScript.SetMoveDirection(Reflect);

            // プレイヤーを加速
            MovePlayerScript.PlayerSpeedManager.SetAccelerationValue(Acceleration);

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
