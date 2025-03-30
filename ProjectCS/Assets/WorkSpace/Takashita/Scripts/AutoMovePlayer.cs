//====================================================
// スクリプト名：AutoMovePlayer
// 作成者：高下
// 内容：プレイヤーの自動前進移動
// 最終更新日：03/31
// 
// [Log]
// 03/27 高下 スクリプト作成 
// 03/31 高下 Update内に移動処理追加
//====================================================
using UnityEngine;

public class AutoMovePlayer : MonoBehaviour
{
    public PlayerSpeedManager PlayerSpeedManager; // 速度管理クラス
    public LRMovePlayer LRMovePlayer;             // カーブ処理を管理するクラス

    void Start()
    {
        if (PlayerSpeedManager == null)
        {
            Debug.LogWarning("AutoMovePlayerスクリプトがアタッチされていません。");
        }

        if (LRMovePlayer == null)
        {
            Debug.LogWarning("LRMovePlayerスクリプトがアタッチされていません。");
        }
    }

    void Update()
    {
        if (PlayerSpeedManager == null || LRMovePlayer == null) return;

        // 速度を取得
        float speed = PlayerSpeedManager.GetPlayerSpeed;

        // 進行方向を取得し、その方向へ移動
        transform.position += LRMovePlayer.GetMoveDirection * speed * Time.deltaTime;

        // 向きを進行方向に合わせる
        transform.forward = LRMovePlayer.GetMoveDirection;
    }
}