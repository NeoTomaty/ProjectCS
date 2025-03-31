//====================================================
// スクリプト名：MovePlayer
// 作成者：高下
// 内容：プレイヤーの自動前進移動
// 最終更新日：03/31
// 
// [Log]
// 03/27 高下 スクリプト作成 
// 03/31 高下 Update内に移動処理追加
// 03/31 高下 スクリプト名変更 AutoMovePlayer→MovePlayer
//====================================================
using UnityEngine;
using UnityEngine.UIElements;

public class MovePlayer : MonoBehaviour
{
    public PlayerSpeedManager PlayerSpeedManager; // 速度管理クラス
   
    private Vector3 MoveDirection;    // 現在の進行方向

    // 他のスクリプトから進行方向を取得するためのプロパティ                                  
    public Vector3 GetMoveDirection => MoveDirection;

    // 他のスクリプトから進行方向を設定するためのセッター
    public void SetMoveDirection(Vector3 NewDirection)
    {
        MoveDirection = NewDirection;
    }

    void Start()
    {
        MoveDirection = transform.forward;

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

        // 進行方向を取得し、その方向へ移動
        transform.position += MoveDirection * speed * Time.deltaTime;

        // 向きを進行方向に合わせる
        transform.forward = MoveDirection;
    }
}