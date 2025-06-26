//====================================================
// スクリプト名：BlownAway_Ver3
// 作成者：藤本
// 内容：リフティング回数に応じて飛ぶ力が段階的に上がる
// [Log]
// 05/13 藤本 リフティング回数に応じて飛ぶ力が段階的に上がる
// 05/30 荒井 スコアのコンボボーナスのリセットを実装
// 06/05 藤本 多段ヒット防止処理を追加
// 06/06 森脇 アニメーションとタイミング動悸させるためにヒットストップ変更
// 06/13 森脇 カメラの制御フラグ追加
// 06/13 高下 スナック複製時に必要なコンポーネントを参照するSetTarget関数を追加
// 06/13 荒井 クリアカウントのタイミングをリフティング時→落下時に変更
// 06/19 中町 プレイヤーがスナックに当たったときのSE実装
// 06/20 森脇 アニメーションの設定
// 06/20 荒井 スナック放置時のペナルティ処理を追加
//====================================================
using UnityEngine;
using System.Collections;

public class BlownAway_Ver3 : MonoBehaviour
{
    [Header("ヒットストップ時間")]
    [SerializeField] private float hitStopTime = 0.5f;            // ヒットストップの時間（秒）

    [Header("吹っ飛びの基本設定")]
    [SerializeField] private float baseForce = 100f;      // 初期飛ぶ力

    [SerializeField] private float forcePerLift = 100f;    // リフティング1回ごとに追加する力

    [SerializeField]
    private float MinUpwardForce = 50.0f;  // 真上への力（最小）

    [SerializeField]
    private float MaxUpwardForce = 200.0f; // 真上への力（最大）

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

    private bool HitSnack = true; // snackに多段ヒットしないようにする

    private Rigidbody Rb;

    private int liftingCount = 1; // すっ飛ぶ力で使用するリフティング回数

    private bool isHitStopActive = false;

    private bool shouldEndHitStop = false;

    [SerializeField]
    [Header("クリア条件を管理しているオブジェクト")]
    private ClearConditions ClearConditionsScript; // リフティング回数管理のスクリプト

    private bool IsRespawn = true;

    private bool IsFlyingAway = true;

    [Header("スナック放置時のペナルティ設定")]
    [SerializeField]
    FlyingPoint FlyingPointScript;
    [SerializeField]
    private float ScorePenaltyStartTime = 40f;
    [SerializeField]
    private float ScoreDecreaseInterval = 10f;
    [SerializeField]
    private float ScoreDecreasePoint = 100f;

    private float PenaltyCount = 0f;
    private bool IsPenaltyTime = false;

    [SerializeField]
    [Header("吹っ飛びエフェクト")]
    private SnackEffectController snackEffectController;

    [Header("SE")]
    //効果音を鳴らすためのAudioSource
    [SerializeField] private AudioSource audioSource;

    //プレイヤーが当たったときの効果音
    [SerializeField] private AudioClip HitSE;

    [Header("アニメーション")]
    [SerializeField] private PlayerAnimationController playerAnimController;

    private void Start()
    {
        Rb = GetComponent<Rigidbody>();

        FallPoint = GetComponent<FallPointCalculator>();

        if (!CameraFunction) Debug.LogError("CameraFunctionが設定されていません");
    }

    // 複製時に引数で渡されたコンポーネントを設定する
    public void SetTarget(CameraFunction CF, FlyingPoint FP, ClearConditions CC, LiftingJump LJ, Transform respawnArea, Transform groundArea, PlayerAnimationController PAController)
    {
        CameraFunction = CF;
        flyingPoint = FP;
        ClearConditionsScript = CC;
        LiftingJump = LJ;
        RespawnArea = respawnArea;
        GroundArea = groundArea;
        playerAnimController = PAController;
    }

    private void OnTriggerEnter(Collider other)
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
    private void Update()
    {
        // Respawnオブジェクトのより高い位置にいたらリスポーン
        if (RespawnArea && transform.position.y > RespawnArea.position.y && HitNextFallArea == true)
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

        // スナック放置ペナルティの処理
        if (!IsFlyingAway)
        {
            PenaltyCount += Time.deltaTime;

            if (IsPenaltyTime)
            {
                // 一定時間ごとにスコア減少
                if (PenaltyCount > ScoreDecreaseInterval)
                {
                    // 関数呼び出し
                    if (FlyingPointScript != null)
                    {
                        FlyingPointScript.DecreaseScore(ScoreDecreasePoint);
                    }

                    // カウントリセット
                    PenaltyCount = 0f;
                }
            }
            else
            {
                // ペナルティ開始時間を超えたらスコア減少開始
                if (PenaltyCount > ScorePenaltyStartTime)
                {
                    IsPenaltyTime = true;

                    // カウントリセット
                    PenaltyCount = 0f;
                }
            }
        }
    }

    // 落下スピードと打ちあがる力を制限する
    private void FixedUpdate()
    {
        // 落下中かつ速度が上限を超えていたら制限
        if (Rb.linearVelocity.y < -MaxFallSpeed)
        {
            Vector3 clampedVelocity = Rb.linearVelocity;
            clampedVelocity.y = -MaxFallSpeed;
            Rb.linearVelocity = clampedVelocity;

            // プレイヤーが離れたら多段ヒット防止フラグをtrue
            HitSnack = true;

            // 吹っ飛びエフェクト停止
            //snackEffectController.StopFlyingEffect();

            Debug.Log($"落下速度を制限しました: {Rb.linearVelocity.y}");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // 吹っ飛ばし状態の時だけ実行
            if (IsFlyingAway)
            {
                // コンボボーナスリセット
                flyingPoint.ResetComboBonus();

                // クリアカウント進行
                ClearConditionsScript.CheckLiftingCount(gameObject);

                IsFlyingAway = false;
            }
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            //SEを再生
            if (audioSource != null && HitSE != null)
            {
                audioSource.PlayOneShot(HitSE);
            }

            // 多段ヒット防止
            if (!HitSnack) return;

            // 多段ヒット防止フラグfalse
            HitSnack = false;


            // 吹っ飛ばし状態へ以降
            IsFlyingAway = true;
            IsPenaltyTime = false;
            PenaltyCount = 0f;　// カウントリセット

            //ClearConditionsScript.
            // Snackに触れたらHitNextFallAreaをtrueに戻す
            HitNextFallArea = true;

            // リフティング回数を加算
            liftingCount++;

            // 力を計算：基本 + 回数 × 増加量
            float force = baseForce + (liftingCount * forcePerLift);

            Debug.Log($"力：{force}");

            // 力の制限
            if (force > MaxUpwardForce)
            {
                force = MaxUpwardForce;
                Debug.Log($"制限後の力：{force}");
            }

            // 上方向のベクトルに力を加える
            Vector3 forceDir = Vector3.up * force;
            Rb.AddForce(forceDir, ForceMode.Impulse);

            Debug.Log(liftingCount);

            // snackの座標のログ
            Debug.Log($"snackの座標: {transform.position}");

            // ゲージによる補正
            if (LiftingJump != null)
            {
                Debug.Log("リフティング開始");
                if (LiftingJump.IsLiftingPart)
                {
                    // プレイヤーのリフティングパートを終了する
                    LiftingJump.FinishLiftingJump();

                    if (flyingPoint != null)
                    {
                        flyingPoint.CalculateScore();
                        Debug.LogWarning("スコア計算開始");
                        Debug.LogWarning($"打ちあがる力{Rb.linearVelocity.y}");
                    }

                    // ゲージが使われたときは手動解除タイプ
                    StartCoroutine(HitStopManual());
                }
                else
                {
                    // ゲージを使っていないときは0.5秒で自動解除
                    StartCoroutine(HitStopTimed(0.5f));
                }
            }
            else
            {
                // LiftingJump が null（通常ヒットなど）でも自動解除
                StartCoroutine(HitStopTimed(0.5f));
            }

            flyingPoint.CalculateScore();

            // ヒットストップを開始する
            //  StartCoroutine(HitStop());

            // ロックオンする対象を設定
            CameraFunction.SetSnack(gameObject.transform);

            // カメラの強制ロックオン開始
            CameraFunction.StartLockOn(true);
        }
    }

    // ヒットストップ関数
    //private System.Collections.IEnumerator HitStop()
    //{
    //    Time.timeScale = 0f; // 時間を止める（スローモーション）
    //    float timer = 0f;

    //    // リアルタイムで一定時間待つ
    //    while (timer < hitStopTime)
    //    {
    //        timer += Time.unscaledDeltaTime;
    //        yield return null;
    //    }

    //    Time.timeScale = 1f; // 時間を再開する

    //    Debug.Log("ヒットストップ開始");
    //}

    //private IEnumerator HitStop()
    //{
    //    if (isHitStopActive) yield break;

    //    Time.timeScale = 0f;
    //    isHitStopActive = true;
    //    shouldEndHitStop = false;

    //    Debug.Log("ヒットストップ開始");

    //    // 外部から EndHitStop() が呼ばれるまで待機
    //    while (!shouldEndHitStop)
    //    {
    //        yield return null;
    //    }

    //    Time.timeScale = 1f;
    //    isHitStopActive = false;

    //    Debug.Log("ヒットストップ終了");
    //}

    private IEnumerator HitStopTimed(float duration)
    {
        if (isHitStopActive) yield break;

        Time.timeScale = 0f;
        isHitStopActive = true;

        Debug.Log($"ヒットストップ（自動解除）開始: {duration}秒");

        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        Time.timeScale = 1f;
        isHitStopActive = false;

        Debug.Log("ヒットストップ終了（自動）");

        // 吹っ飛びエフェクト開始
        snackEffectController.PlayFlyingEffect();
    }

    private IEnumerator HitStopManual()
    {
        if (isHitStopActive) yield break;

        Time.timeScale = 0f;
        isHitStopActive = true;
        shouldEndHitStop = false;

        Debug.Log("ヒットストップ（手動解除）開始");

        playerAnimController.PlayRandomAnimation();

        // 外部から EndHitStop() が呼ばれるまで待つ
        while (!shouldEndHitStop)
        {
            yield return null;
        }

        Time.timeScale = 1f;
        isHitStopActive = false;

        Debug.Log("ヒットストップ終了（手動）");

        // 吹っ飛びエフェクト開始
        snackEffectController.PlayFlyingEffect();
    }

    // 外部スクリプトから呼び出してヒットストップを終了させる
    public void EndHitStop()
    {
        if (isHitStopActive)
        {
            shouldEndHitStop = true;
        }
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
    public void OnClear()
    {
        IsRespawn = false;
    }
}