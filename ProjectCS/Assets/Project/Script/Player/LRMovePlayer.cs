//====================================================
// スクリプト名：LRMovePlayer
// 作成者：高下
// 内容：プレイヤーの左右移動処理
// 最終更新日：04/01
// 
// [Log]
// 03/27 高下 スクリプト作成 
// 03/31 高下 左右移動処理追加
// 04/01 コントローラー操作追加
// 04/08 左右移動を速度に依存させる
//====================================================
using UnityEngine;
using UnityEngine.InputSystem;

public class LRMovePlayer : MonoBehaviour
{
    public PlayerSpeedManager PlayerSpeedManager; // 速度管理クラス
    public MovePlayer MovePlayer; // プレイヤー移動クラス



    [SerializeField] private float TurnSpeed = 100.0f;  // カーブの回転速度
    [SerializeField] private float TurnResponse = 1.0f; // カーブのしやすさ

    void Start()
    {
        if (PlayerSpeedManager == null)
        {
            Debug.LogWarning("MovePlayerスクリプトがアタッチされていません。");
        }
    }

    void Update()
    {
        if (PlayerSpeedManager == null) return;

        // 速度を取得
        float speed = PlayerSpeedManager.GetPlayerSpeed;

        // 速度が速いほどカーブしにくくする
        //float rotationAmount = (TurnSpeed / Mathf.Max(speed, 1)) * Time.deltaTime;

        // 現在の速度に応じて曲がりやすくする
        // カーブ量 = 回転速度 × 速度 × deltaTime
        float rotationAmount = TurnSpeed * (speed * TurnResponse) * Time.deltaTime;

        // プレイヤーの左右移動方向を示す変数
        float moveX = 0.0f;

        // ゲームパッドが接続されているか確認
        if (Gamepad.current != null)
        {
            // ゲームパッドの左スティック入力を優先
            moveX = Gamepad.current.leftStick.ReadValue().x;
        }
        else
        {
            // ゲームパッドが接続されていない場合、キーボードの入力を使用
            if (Input.GetKey(KeyCode.A)) moveX = -1.0f;
            if (Input.GetKey(KeyCode.D)) moveX = 1.0f;
        }



        // 回転処理（左右）
        if (Mathf.Abs(moveX) > 0.1f)    // 絶対値より大きな値の場合
        {
            float angle = rotationAmount * Mathf.Sign(moveX);   // 符号を取得
            MovePlayer.SetMoveDirection(Quaternion.Euler(0, angle, 0) * MovePlayer.GetMoveDirection);
        }

        //// 左カーブ
        //if (moveX < -0.1f)
        //{
        //    MovePlayer.SetMoveDirection(Quaternion.Euler(0, -rotationAmount, 0) * MovePlayer.GetMoveDirection);
        //}
        //// 右カーブ
        //if (moveX > 0.1f)
        //{
        //    MovePlayer.SetMoveDirection(Quaternion.Euler(0, rotationAmount, 0) * MovePlayer.GetMoveDirection);
        //}
    }
}
