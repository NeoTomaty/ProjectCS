//====================================================
// スクリプト名：MovePlayer
// 作成者：高下
// 内容：プレイヤーの自動前進移動
// 最終更新日：05/22
// 
// [Log]
// 03/27 高下 スクリプト作成 
// 03/31 高下 Update内に移動処理追加
// 03/31 高下 スクリプト名変更 AutoMovePlayer→MovePlayer
// 04/08 高下 高速時の壁貫通防止処理を実装
// 04/09 竹内 AutoRapidMove対応するようにスクリプトを修正
// 05/07 荒井 LiftingJumpのすり抜けモードに対応するように変更
// 05/15 荒井 MoveSpeedMultiplier変数を追加し、移動処理に速度の補正を乗せられるように変更
// 05/22 中町 移動開始音SE実装
// 06/27 中町 移動開始音SE音量調整実装
//====================================================
using UnityEngine;
using UnityEngine.Audio;

public class MovePlayer : MonoBehaviour
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

    // 移動速度の倍率
    [System.NonSerialized] private float _MoveSpeedMultiplier = 1f; // PlayerSpeedManagerのスピード値を変えずに速度を変えたいため追加
    public float MoveSpeedMultiplier
    {
        get => _MoveSpeedMultiplier;
        set => _MoveSpeedMultiplier = Mathf.Min(value, 3f);
    }

    private Rigidbody Rb;

    //SEを再生するためのAudioSource
    [SerializeField] private AudioSource audioSource;
    
    //移動開始後に鳴らすSE
    [SerializeField] private AudioClip MoveStartSE;

    //SEの音量(0〜1の範囲)
    [SerializeField, Range(0.0f, 1.0f)] private float MoveStartSEVolume = 0.5f;

    //前フレームで移動していたかどうかを記録
    private bool WasMoving = false;

    //SEをすでに再生したかどうかのフラグ
    private bool HasPlayedSE = false;

    //移動開始からの経過時間を記録するタイマー
    private float MoveStartTimer = 0.0f;

    //SEを鳴らすまでの遅延時間(秒)
    private float DelayBeforeSE = 0.05f;

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

        //現在の移動状態を判定(MoveDirectionの大きさが0.01より大きければ移動中とみなす)
        bool IsCurrentlyMoving = MoveDirection.magnitude > 0.01f;

        if(IsCurrentlyMoving)
        {
            //前のフレームでは止まっていて、今のフレームで移動を開始したとき
            if(!WasMoving)
            {
                //タイマーをリセット
                MoveStartTimer = 0.0f;

                //SE再生フラグもリセット
                HasPlayedSE = false;
            }

            //SEがまだ再生されていないとき、タイマーを進める
            if(!HasPlayedSE)
            {
                //経過時間を加算
                MoveStartTimer += Time.deltaTime;

                //一定時間(DelayBeforeSE)経過したらSEを再生
                if(MoveStartTimer >= DelayBeforeSE)
                {
                    if(audioSource != null && MoveStartSE != null)
                    {
                        //指定した音量でSEを一度だけ再生
                        audioSource.PlayOneShot(MoveStartSE, MoveStartSEVolume);

                        //再生済みフラグを立てる
                        HasPlayedSE = true;
                    }
                }
            }
        }

        //現在の移動状態を次フレーム用に保存
        WasMoving = IsCurrentlyMoving;

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
                Rb.linearVelocity = new Vector3(MoveDirection.x * PlayerSpeedManager.GetPlayerSpeed, MoveDirection.y * PlayerSpeedManager.GetPlayerSpeed, MoveDirection.z * PlayerSpeedManager.GetPlayerSpeed) * MoveSpeedMultiplier;
            }
        }
        else
        {
            // 進行方向を取得し、その方向へ移動
            if (LiftingJump.IsLiftingPart)
            {
                Rb.linearVelocity = new Vector3(MoveDirection.x * PlayerSpeedManager.GetPlayerSpeed, MoveDirection.y * PlayerSpeedManager.GetPlayerSpeed, MoveDirection.z * PlayerSpeedManager.GetPlayerSpeed) * MoveSpeedMultiplier;
            }
            else
            {
                Rb.linearVelocity = new Vector3(MoveDirection.x * PlayerSpeedManager.GetPlayerSpeed, Rb.linearVelocity.y, MoveDirection.z * PlayerSpeedManager.GetPlayerSpeed) * MoveSpeedMultiplier;
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