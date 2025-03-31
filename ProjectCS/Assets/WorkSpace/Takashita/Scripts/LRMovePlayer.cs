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
using UnityEngine.EventSystems;

public class LRMovePlayer : MonoBehaviour
{
    public PlayerSpeedManager PlayerSpeedManager; // 速度管理クラス
    public MovePlayer MovePlayer; // プレイヤー移動クラス

    [SerializeField]
    private float TurnSpeed = 100.0f; // カーブの回転速度

    void Start()
    {
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
            MovePlayer.SetMoveDirection(Quaternion.Euler(0, -rotationAmount, 0) * MovePlayer.GetMoveDirection);
        }
        if (Input.GetKey(KeyCode.D))
        {
            // 右カーブ
            MovePlayer.SetMoveDirection(Quaternion.Euler(0, rotationAmount, 0) * MovePlayer.GetMoveDirection);
        }
    }
}
