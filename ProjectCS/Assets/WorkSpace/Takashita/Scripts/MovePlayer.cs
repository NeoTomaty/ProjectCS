//====================================================
// スクリプト名：MovePlayer
// 作成者：高下
// 内容：プレイヤーの自動前進移動
// 最終更新日：04/08
// 
// [Log]
// 03/27 高下 スクリプト作成 
// 03/31 高下 Update内に移動処理追加
// 03/31 高下 スクリプト名変更 AutoMovePlayer→MovePlayer
// 04/08 高下 高速時の壁貫通防止処理を実装
//====================================================
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public PlayerSpeedManager PlayerSpeedManager; // 速度管理クラス
   
    private Vector3 MoveDirection;    // 現在の進行方向

    [SerializeField]
    float RayDistance = 10.0f;

    // 他のスクリプトから進行方向を取得するためのプロパティ                                  
    public Vector3 GetMoveDirection => MoveDirection;

    // 他のスクリプトから進行方向を設定するためのセッター
    public void SetMoveDirection(Vector3 NewDirection)
    {
        MoveDirection = NewDirection;
    }

    private string WallTag = "Wall";   // 壁のタグ

    void Start()
    {
        MoveDirection = transform.forward;

        if (PlayerSpeedManager == null)
        {
            Debug.LogWarning("MovePlayerスクリプトがアタッチされていません。");
        }
    }

    void Update()
    {
        if (PlayerSpeedManager == null) return;

        // レイキャストで壁を検出
        RaycastHit hit;
        if (Physics.Raycast(transform.position, MoveDirection, out hit, RayDistance))
        {
            // 衝突したオブジェクトのタグを確認
            if (hit.collider.CompareTag(WallTag))  // 壁タグに一致するか確認
            {
                // レイが壁に衝突した場合、その地点に移動
                // めり込んだ分を押し戻すため、衝突点から移動方向の反対方向に少しずらす
                Vector3 PushBackDirection = -MoveDirection.normalized;  // 反対方向
                transform.position = hit.point + PushBackDirection * 0.1f;  // 0.1fは押し戻しの距離
            }
            else
            {
                // 進行方向を取得し、その方向へ移動
                transform.position += MoveDirection * PlayerSpeedManager.GetPlayerSpeed * Time.deltaTime;
            }
        }
        else
        {
            // 進行方向を取得し、その方向へ移動
            transform.position += MoveDirection * PlayerSpeedManager.GetPlayerSpeed * Time.deltaTime;
        }

        // 向きを進行方向に合わせる
        transform.forward = MoveDirection;
    }
}