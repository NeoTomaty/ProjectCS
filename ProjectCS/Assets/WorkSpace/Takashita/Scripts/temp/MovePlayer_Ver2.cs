//====================================================
// スクリプト名：MovePlayer_Ver2
// 作成者：高下
// 内容：プレイヤーの自動前進移動(Rigidbodyで移動)
// 最終更新日：05/16
// 
// [Log]
// 05/16 高下 スクリプト作成
//====================================================
using UnityEngine;

public class MovePlayer_Ver2 : MonoBehaviour
{
    public PlayerSpeedManager PlayerSpeedManager; // 速度管理クラス
    private LiftingJump LiftingJump; // リフティングジャンプクラス

    private Vector3 MoveDirection;    // 現在の進行方向
    // 他のスクリプトから進行方向を取得するためのプロパティ                                  
    public Vector3 GetMoveDirection => MoveDirection;

    [SerializeField]
    private float RayDistance = 10.0f;
    [SerializeField]
    private float HitStopDuration = 0.1f;  // ヒットストップの時間
    private float HitStopTimer = 0.0f;  // ヒットストップのタイマー

    private bool IsHitStopActive = false; // ヒットストップ中かどうか
    public bool GetIsHitStopActive => IsHitStopActive;

    private Rigidbody Rb;

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

        LiftingJump = GetComponent<LiftingJump>();
        if (LiftingJump == null)
        {
            Debug.LogWarning("LiftingJumpスクリプトがPlayerにアタッチされていません。");
        }

        Rb = GetComponent<Rigidbody>();
    }

    void Update()
    {

        if (IsHitStopActive)
        {
            // ヒットストップが有効な場合、移動を止める
            if (HitStopTimer > 0.0f)
            {
                HitStopTimer -= Time.deltaTime;  // タイマーを減少
                return;  // ヒットストップ中は移動を停止
            }
            else
            {
                IsHitStopActive = false;
            }
        }

        if (PlayerSpeedManager == null) return;

        // レイキャストで壁を検出
        // すり抜けが有効な場合はレイキャストによる衝突判定を行わない
        RaycastHit hit;
        if (!LiftingJump.IsIgnore && Physics.Raycast(transform.position, MoveDirection, out hit, RayDistance))
        {
            // 衝突したオブジェクトのタグを確認
            if (hit.collider.CompareTag(WallTag))  // 壁タグに一致するか確認
            {
                // レイが壁に衝突した場合、その地点に移動
                // めり込んだ分を押し戻すため、衝突点から移動方向の反対方向に少しずらす
                Vector3 PushBackDirection = -MoveDirection.normalized;  // 反対方向
                float radius = GetComponent<SphereCollider>().radius;
                transform.position = hit.point + PushBackDirection * radius;  // プレイヤー半径分の押し戻し
            }
            else
            {
                // 進行方向を取得し、その方向へ移動
                Rb.linearVelocity = new Vector3(MoveDirection.x * PlayerSpeedManager.GetPlayerSpeed, MoveDirection.y * PlayerSpeedManager.GetPlayerSpeed, MoveDirection.z * PlayerSpeedManager.GetPlayerSpeed);
            }
        }
        else
        {
            // 進行方向を取得し、その方向へ移動
            if (LiftingJump.IsLiftingPart)
            {
                Rb.linearVelocity = new Vector3(MoveDirection.x * PlayerSpeedManager.GetPlayerSpeed, MoveDirection.y * PlayerSpeedManager.GetPlayerSpeed, MoveDirection.z * PlayerSpeedManager.GetPlayerSpeed);
            }
            else
            {
                Rb.linearVelocity = new Vector3(MoveDirection.x * PlayerSpeedManager.GetPlayerSpeed, Rb.linearVelocity.y, MoveDirection.z * PlayerSpeedManager.GetPlayerSpeed);
            }

        }

        // 向きを進行方向に合わせる
        transform.forward = MoveDirection;
    }

    // ヒットストップを実行する
    public void StartHitStop()
    {
        HitStopTimer = HitStopDuration;
        IsHitStopActive = true;
    }
}
