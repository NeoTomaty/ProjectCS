//====================================================
// スクリプト名：LRMovePlayer
// 作成者：高下
// 内容：プレイヤーの左右移動処理
// 最終更新日：03/31
// 
// [Log]
// 03/27 高下 スクリプト作成 
// 03/31 高下 左右移動処理追加
//====================================================
using UnityEngine;

public class LRMovePlayer : MonoBehaviour
{
    public PlayerSpeedManager PlayerSpeedManager; // 速度管理クラス

    [SerializeField]
    private float TurnSpeed = 100.0f; // カーブの回転速度

    private Vector3 MoveDirection;    // 現在の進行方向

    // 他のスクリプトから進行方向を取得するためのプロパティ                                  
    public Vector3 GetMoveDirection => MoveDirection;

    void Start()
    {
        MoveDirection = transform.forward; // 初期の進行方向

        if (PlayerSpeedManager == null)
        {
            Debug.LogWarning("AutoMovePlayerスクリプトがアタッチされていません。");
        }
    }

    void Update()
    {
        if (PlayerSpeedManager == null) return;

        // 速度を取得
        float speed = PlayerSpeedManager.GetPlayerSpeed;

        // 速度が速いほどカーブしにくくする
        float rotationAmount = (TurnSpeed / Mathf.Max(speed, 1)) * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            // 左カーブ
            MoveDirection = Quaternion.Euler(0, -rotationAmount, 0) * MoveDirection;
        }
        if (Input.GetKey(KeyCode.D))
        {
            // 右カーブ
            MoveDirection = Quaternion.Euler(0, rotationAmount, 0) * MoveDirection;
        }
    }
}
