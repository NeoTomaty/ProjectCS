//====================================================
// スクリプト名：BlownAway_Ver2
// 作成者：藤本
// 
// [Log]
// 04/13 高下 OnCollisionEnter内で飛ばし方を修正
// 04/21 高下 重力関連を別スクリプト(ObjectGravity)に移動させました
// 04/24 藤本 ポイント計算を開始するためのコードを追記しました
// 04/28 竹内 SweetSizeUpが関与する箇所を抹消
// 04/29 荒井 吹っ飛ばしにジャンプのパワーによる補正を追加
// 05/01 藤本 ステージ内にリスポーン
// 05/02 藤本 Respawnオブジェクトをすり抜けないよう高さでも判定を取るように修正
// 05/02 荒井 リフティングジャンプの終了処理の場所を修正
// 05/02 高下 対象を飛ばしたときに強制的にロックオンする処理を追加
// 05/08 藤本 リスポーンした後上昇し落下スピードを調整できるように修正
// 05/08 荒井 ゲームクリア時にスナックのリスポーンを無効化する処理を追加
// 05/15 荒井 リフティングジャンプ時のスナック吹っ飛ばし威力にゲージやチャージジャンプの影響を適用し、仮の威力補正を実装
// 05/16 荒井 クリア演出時のスナックの速度を固定にできるように変更
//====================================================
using UnityEngine;

public class BlownAway_Ver2 : MonoBehaviour
{
    [SerializeField]
    private float MinHitStopTime = 0.5f;    // ヒットストップ時間（最小）
    [SerializeField]
    private float MaxHitStopTime = 1.0f;    // ヒットストップ時間（最大）
    [SerializeField]
    private float MinUpwardForce = 50.0f;  // 真上への力（最小）
    [SerializeField]
    private float MaxUpwardForce = 200.0f; // 真上への力（最大）
    [SerializeField]
    private float MinForwardForce = 0.0f;// 前方方向の最小力
    [SerializeField]
    private float MaxForwardForce = 0.0f;// 前方方向の最大力
    [SerializeField]
    private float MinRandomXYRange = 0.0f; // ランダムに加えるXY軸の範囲（最小）
    [SerializeField]
    private float MaxRandomXYRange = 0.0f; // ランダムに加えるXY軸の範囲（最大）

    [SerializeField]
    private float MinFallSpeed = 0.0f; // 落下時のスピート（最小）
    [SerializeField]
    private float MaxFallSpeed = 30.0f; // 落下時のスピート（最大）

    [SerializeField]
    private Transform RespawnArea;         // 移動させる範囲オブジェト

    [SerializeField]
    private LiftingJump LiftingJump; // リフティングジャンプのスクリプト

    [SerializeField]
    private Transform GroundArea;   // ステージの範囲を示すオブジェクト

    [SerializeField] 
    private CameraFunction CameraFunction;

    [SerializeField]
    private FlyingPoint flyingPoint;　// スコア計算用スクリプト

    private FallPointCalculator FallPoint; // 落下地点を計算するスクリプト

    private float previousVerticalVelocity = 0f;  // リスポーン前のY方向速度を保存

    private bool HitNextFallArea = true;    // リスポーンエリアに連続で当たらないようにする

    private Rigidbody Rb;

    [SerializeField]
    [Header("クリア条件を管理しているオブジェクト")]
    private ClearConditions ClearConditionsScript; // リフティング回数管理のスクリプト

    private bool IsRespawn = true;
    private int OnClearSnackSpeed = 700;

    void Start()
    {
        Rb = GetComponent<Rigidbody>();

        FallPoint = GetComponent<FallPointCalculator>();

        if (!CameraFunction) Debug.LogError("CameraFunctionが設定されていません");
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"接触: {other.name}");

        if (other.CompareTag("Respawn") && HitNextFallArea == true)
        {
            HitNextFallArea = false;

            // 現在のY方向速度を保存
            previousVerticalVelocity = Rb.linearVelocity.y;

            MoveToRandomXZInRespawnArea();
        }
    }

    // snackがRespawnオブジェクトをスレ抜けてもリスポーンする
    void Update()
    {
        // Respawnオブジェクトのより高い位置にいたらリスポーン
        if (RespawnArea && transform.position.y > RespawnArea.position.y　&& HitNextFallArea == true)
        {
            HitNextFallArea = false;
            // 現在のY方向速度を保存
            previousVerticalVelocity = Rb.linearVelocity.y;

            // リスポーン無効でなければ
            if (IsRespawn)
            {
                Debug.Log($"Respawnオブジェクトの高さを超えたためリスポーン");
                MoveToRandomXZInRespawnArea();
            }
        }
    }

    // 落下スピードを制限する
    void FixedUpdate()
    {
        // 落下中かつ速度が上限を超えていたら制限
        if (Rb.linearVelocity.y < -MaxFallSpeed)
        {
            Vector3 clampedVelocity = Rb.linearVelocity;
            clampedVelocity.y = -MaxFallSpeed;
            Rb.linearVelocity = clampedVelocity;

            Debug.Log($"落下速度を制限しました: {Rb.linearVelocity.y}");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        ClearConditionsScript.CheckLiftingCount();

        // Snackに触れたらHitNextFallAreaをtrueに戻す
        HitNextFallArea = true;

        // PlayerSpeedManagerスクリプトを取得
        PlayerSpeedManager PlayerSpeedManager = collision.gameObject.GetComponent<PlayerSpeedManager>();
        if (!PlayerSpeedManager)
        {
            Debug.LogWarning("PlayerSpeedManager取得失敗");
            return;
        }
        // プレイヤーの現在の速度割合を取得
        float SpeedRatio = PlayerSpeedManager.GetSpeedRatio();
        // 現在のお菓子の大きさの割合を取得する
        float ScaleRatio = transform.localScale.x;

        // 真上方向に力を加える
        Vector3 UpwardDirection = Vector3.up * Mathf.Lerp(MinUpwardForce, MaxUpwardForce, SpeedRatio);

        // プレイヤーの前方方向を取得（向いている方向）
        Vector3 PlayerForward = collision.transform.forward;
        Vector3 ForwardForce = PlayerForward * Mathf.Lerp(MinForwardForce, MaxForwardForce, ScaleRatio);

        // ランダムなXY方向のベクトルを生成
        float RandomAngle = Random.Range(0.0f, 2.0f * Mathf.PI); // ランダムな角度
        Vector3 RandomDirection = new Vector3(Mathf.Cos(RandomAngle), 0.0f, Mathf.Sin(RandomAngle));

        // ランダム方向ベクトルを指定した距離になるようにスケーリング
        RandomDirection = RandomDirection.normalized * Mathf.Lerp(MinRandomXYRange, MaxRandomXYRange, ScaleRatio);

        // 最終的な力の方向
        //Vector3 ForceDirection = UpwardDirection + RandomDirection + ForwardForce;

        // snackの座標のログ
        Debug.Log($"snackの座標: {transform.position}");

        Vector3 ForceDirection = UpwardDirection;

        // ゲージによる補正
        if (IsRespawn && LiftingJump != null)
        {
            if (LiftingJump.IsLiftingPart)
            {
                // プレイヤーの速度が早くなるほど、吹っ飛ばし威力に減衰補正をかける
                float Ratio = PlayerSpeedManager.GetSpeedRatio();
                float Multiplier = 1.3f - (Ratio * 0.5f);
                ForceDirection *= LiftingJump.GetForce * (LiftingJump.GetJumpPower * Multiplier);

                // プレイヤーのリフティングパートを終了する
                LiftingJump.FinishLiftingJump();    // AddForceの前に呼び出さないとスナックが飛ばない

                if (flyingPoint != null)
                {
                    flyingPoint.CalculateScore();
                    Debug.LogWarning("スコア計算開始");
                }
            }
        }

        // クリア演出時にスナックが吹っ飛ぶ速度を固定化
        if (!IsRespawn)
        {
            ForceDirection = Vector3.up * OnClearSnackSpeed;
        }

        // Rigidbodyに力を加える
        Rb.AddForce(ForceDirection, ForceMode.Impulse);

        // ヒットストップを開始する
        StartCoroutine(HitStop(Mathf.Lerp(MinHitStopTime, MaxHitStopTime, SpeedRatio)));

        // カメラの強制ロックオン開始
        CameraFunction.StartLockOn(true);
    }

    // ヒットストップ関数
    System.Collections.IEnumerator HitStop(float speed)
    {
        Time.timeScale = 0f;

        float normalizedSpeed = Mathf.InverseLerp(1f, 20f, speed);
        float duration = Mathf.Lerp(0.5f, MaxHitStopTime, normalizedSpeed);

        Vector3 originalPosition = transform.localPosition;
        float timer = 0f;

        while (timer < duration)
        {
            float shakeStrength = 0.05f;
            float offsetX = Random.Range(-shakeStrength, shakeStrength);
            float offsetY = Random.Range(-shakeStrength, shakeStrength);

            transform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0);

            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
        Time.timeScale = 1f;

    }

    // リスポーン位置
    private void MoveToRandomXZInRespawnArea()
    {
        if (RespawnArea == null || GroundArea == null)
        {
            Debug.LogWarning("RespawnAreaが設定されていません");
            return;
        }

        // 範囲取得
        Vector3 respawnCenter = RespawnArea.position;
        Vector3 respawnSize = RespawnArea.localScale;

        Vector3 groundCenter = GroundArea.position;
        Vector3 groundSize = GroundArea.localScale;

        // XとZの最小・最大を両方の範囲で共通する部分に制限
        float minX = Mathf.Max(respawnCenter.x - respawnSize.x / 2, groundCenter.x - groundSize.x / 2);
        float maxX = Mathf.Min(respawnCenter.x + respawnSize.x / 2, groundCenter.x + groundSize.x / 2);

        float minZ = Mathf.Max(respawnCenter.z - respawnSize.z / 2, groundCenter.z - groundSize.z / 2);
        float maxZ = Mathf.Min(respawnCenter.z + respawnSize.z / 2, groundCenter.z + groundSize.z / 2);

        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);

        // Y座標はRespawnAreaの高さに設定
        float y = respawnCenter.y;

        // 保存した上方向の力を代入
        Vector3 newPos = new Vector3(randomX, y, randomZ);
        transform.position = newPos;
        Rb.linearVelocity = new Vector3(0f, previousVerticalVelocity, 0f);

        Debug.Log($"上昇速度: {Rb.linearVelocity.y}");
        Debug.Log($"リスポーン座標（グラウンド内）: {newPos}");

        FallPoint?.CalculateGroundPoint();

    }

    // クリア時処理
    public void OnClear(int SnackSpeed)
    {
        // リスポーン無効化
        IsRespawn = false;

        // スナックの速度を設定
        OnClearSnackSpeed = SnackSpeed;
    }
}