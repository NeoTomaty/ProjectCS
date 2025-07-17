//====================================================
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
// 06/23 高下 打ち上げと同じタイミングで次のワープ先を計算するように変更
// 06/23 高下 落下が始まったタイミングでワープするように設定
// 06/27 中町 プレイヤーがスナックに当たったときのSE音量調整実装
//====================================================
using UnityEngine;
using System.Collections;

public class BlownAway_Ver3 : MonoBehaviour
{

    [SerializeField] private float MaxFallSpeed = 30.0f;

    [SerializeField] private Transform RespawnArea;

    [SerializeField] private LiftingJump LiftingJump;

    [SerializeField] private Transform GroundArea;

    [SerializeField] private CameraFunction CameraFunction;

    [SerializeField] private FlyingPoint flyingPoint;

    [SerializeField] private ClearConditions ClearConditionsScript;

    private FallPointCalculator FallPoint;

    private bool HitNextFallArea = true;

    private bool HitSnack = true;

    private Rigidbody Rb;

    private int liftingCount = 1;

    public bool isHitStopActive = false;

    private bool shouldEndHitStop = false;

    private bool IsRespawn = true;

    private bool IsFlyingAway = true;

    [Header("スナック放置時のペナルティ設定")]
    [SerializeField]
    private float ScorePenaltyStartTime = 40f;

    [SerializeField]
    private float ScoreDecreaseInterval = 10f;

    [SerializeField]
    private float ScoreDecreasePoint = 100f;

    private float PenaltyCount = 0f;
    private bool IsPenaltyTime = false;

    [SerializeField]
    private SnackEffectController snackEffectController;

    [Header("SE")]
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip HitSE;

    [SerializeField] private PlayerAnimationController playerAnimController;

    [SerializeField, Range(0.0f, 1.0f)]
    private float SEVolume = 0.5f;

    // 次のワープ先を保持する
    private Vector3 nextWarpPosition = Vector3.zero;

    public Vector3 NextWarpPosition => nextWarpPosition;

    private bool IsWaiting = true;

    [Header("スナック打ち上げ関連の数値")]
    private bool IsLaunch = false;
    [SerializeField] private float FirstTargetHeight = 500f;
    [SerializeField] private float MaxTargetHeight = 1000f;
    [SerializeField] private float LaunchMultiplier = 1.1f;
    private float CurrentTargetHeight = 500f;
    private float InitialVelocity;            // 初速
    private float CurrentVelocity;            // 現在の速度
    private float ElapsedLaunchTime = 0f;
    private float StartY = 0f;
    private float GravityScaleY = 9.8f;

    private void Start()
    {
        Rb = GetComponent<Rigidbody>();

        FallPoint = GetComponent<FallPointCalculator>();

        if (gameObject.name.EndsWith("(Clone)"))
        {
            snackEffectController.PlayFlyingEffect();
            MoveToRandomXZInRespawnArea();
        }

        CurrentTargetHeight = FirstTargetHeight;
        GravityScaleY = Mathf.Abs(GetComponent<ObjectGravity>().GetGravityScaleY());

    }

    // 複製時に引数で渡されたコンポーネントを設定する
    public void SetTarget(
        CameraFunction CF, 
        FlyingPoint FP, 
        ClearConditions CC, 
        LiftingJump LJ,
        Transform respawnArea, 
        Transform groundArea, 
        PlayerAnimationController PAC,
        float firstTargetHeight,
        float maxTargetHeight,
        float launchMultiplier,
        float maxFallSpeed
        )
    {
        CameraFunction = CF;
        flyingPoint = FP;
        ClearConditionsScript = CC;
        LiftingJump = LJ;
        RespawnArea = respawnArea;
        GroundArea = groundArea;
        playerAnimController = PAC;
        nextWarpPosition = transform.position;
        FirstTargetHeight = firstTargetHeight;
        MaxTargetHeight = maxTargetHeight;
        LaunchMultiplier = launchMultiplier;
        MaxFallSpeed = maxFallSpeed;
    }

    private void Update()
    {
        // スナック放置ペナルティの処理
        if (!IsFlyingAway)
        {
            PenaltyCount += Time.deltaTime;

            if (IsPenaltyTime)
            {
                // 一定時間ごとにスコア減少
                if (PenaltyCount > ScoreDecreaseInterval)
                {
                    if (flyingPoint != null)
                    {
                        flyingPoint.DecreaseScore(ScoreDecreasePoint);
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
                    if (flyingPoint != null)
                    {
                        flyingPoint.DecreaseScore(ScoreDecreasePoint);
                    }

                    IsPenaltyTime = true;

                    // カウントリセット
                    PenaltyCount = 0f;
                }
            }
        }

        // 1フレーム停止
        if (IsWaiting)
        {
            IsWaiting = false;
            return;
        }

        // 打ち上げ中
        if (IsLaunch)
        {
            ElapsedLaunchTime += Time.deltaTime;

            // 現在の高さと速度を計算
            float newY = StartY + (InitialVelocity * ElapsedLaunchTime) - (0.5f * GravityScaleY * ElapsedLaunchTime * ElapsedLaunchTime);
            CurrentVelocity = InitialVelocity - GravityScaleY * ElapsedLaunchTime;

            // 床より下に落ちるのを防ぐ
            if (newY <= StartY + CurrentTargetHeight && CurrentVelocity >= 0f)
            {
                transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            }
            else
            {
                IsLaunch = false; // 頂点に達したら止める
                HitSnack = true;
                Rb.isKinematic = false;
                CurrentTargetHeight *= LaunchMultiplier;
                CurrentTargetHeight = Mathf.Min(CurrentTargetHeight, MaxTargetHeight);
            }
        }
        // 打ち上げ以外（落下中など）
        else
        {
            if(HitNextFallArea)
            {
                HitNextFallArea = false;
                Rb.linearVelocity = Vector3.zero;
                if (IsRespawn)
                {
                    Debug.Log($"落下が始まったため、ワープしました");
                    DoWarp();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (Rb.linearVelocity.y < -MaxFallSpeed)
        {
            Vector3 clampedVelocity = Rb.linearVelocity;
            clampedVelocity.y = -MaxFallSpeed;
            Rb.linearVelocity = clampedVelocity;

            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        snackEffectController.StopFlyingEffect();

        if (collision.gameObject.CompareTag("Ground"))
        {
            if (IsFlyingAway)
            {
                flyingPoint.ResetComboBonus();

                ClearConditionsScript.CheckLiftingCount(gameObject);

                IsFlyingAway = false;
            }
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            if (audioSource != null && HitSE != null)
            {
                audioSource.PlayOneShot(HitSE,SEVolume);
            }

            if (!HitSnack) return;

            HitSnack = false;

            IsFlyingAway = true;
            IsPenaltyTime = false;
            PenaltyCount = 0f;　// カウントリセット

            //ClearConditionsScript.
            HitNextFallArea = true;

            if (LiftingJump != null)
            {
                if (LiftingJump.IsLiftingPart)
                {
                    LiftingJump.FinishLiftingJump();

                    if (flyingPoint != null)
                    {
                        flyingPoint.CalculateScore();
                    }

                    StartCoroutine(HitStopManual());
                }
                else
                {
                    StartCoroutine(HitStopTimed(0.5f));
                }
            }
            else
            {
                StartCoroutine(HitStopTimed(0.5f));
            }

            flyingPoint.CalculateScore();

            // ワープ先を計算
            MoveToRandomXZInRespawnArea();

            // ロックオンする対象を設定
            CameraFunction.SetSnack(gameObject.transform);

            CameraFunction.StartLockOn(true);

            Launch(); // 打ち上げを開始させる
        }
    }

    private IEnumerator HitStopTimed(float duration)
    {
        if (isHitStopActive) yield break;

        Time.timeScale = 0f;
        isHitStopActive = true;

        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        Time.timeScale = 1f;
        isHitStopActive = false;

        snackEffectController.PlayFlyingEffect();
    }

    private IEnumerator HitStopManual()
    {
        if (isHitStopActive) yield break;

        Time.timeScale = 0f;
        isHitStopActive = true;
        shouldEndHitStop = false;

        playerAnimController.PlayRandomAnimation();

        while (!shouldEndHitStop)
        {
            yield return null;
        }

        Time.timeScale = 1f;
        isHitStopActive = false;

        snackEffectController.PlayFlyingEffect();
    }

    public void EndHitStop()
    {
        if (isHitStopActive)
        {
            shouldEndHitStop = true;
        }
    }

    // リスポーン位置
    public void MoveToRandomXZInRespawnArea()
    {
        if (RespawnArea == null || GroundArea == null)
        {
            return;
        }

        do
        {
            Vector3 respawnCenter = RespawnArea.position;
            Vector3 respawnSize = RespawnArea.localScale;

            Vector3 groundCenter = GroundArea.position;
            Vector3 groundSize = GroundArea.localScale;

            float minX = Mathf.Max(respawnCenter.x - respawnSize.x / 2, groundCenter.x - groundSize.x / 2);
            float maxX = Mathf.Min(respawnCenter.x + respawnSize.x / 2, groundCenter.x + groundSize.x / 2);

            float minZ = Mathf.Max(respawnCenter.z - respawnSize.z / 2, groundCenter.z - groundSize.z / 2);
            float maxZ = Mathf.Min(respawnCenter.z + respawnSize.z / 2, groundCenter.z + groundSize.z / 2);

            float randomX = Random.Range(minX, maxX);
            float randomZ = Random.Range(minZ, maxZ);

            float y = respawnCenter.y;

            Vector3 newPos = new Vector3(randomX, y, randomZ);
            nextWarpPosition = newPos;
            IsWaiting = true;
        }
        while (!FallPoint.CalculateGroundPoint(nextWarpPosition));

    }

    public void OnClear()
    {
        IsRespawn = false;
    }

    // ワープ処理を実行
    private void DoWarp()
    {
        // ワープ時の高さを最低補償
        if (transform.position.y > nextWarpPosition.y)
        {
            nextWarpPosition.y = transform.position.y;
        }
        transform.position = nextWarpPosition;
    }

    public void Launch()
    {
        // 必要な初速を計算：v = sqrt(2gh)
        InitialVelocity = Mathf.Sqrt(2f * GravityScaleY * CurrentTargetHeight);
        CurrentVelocity = InitialVelocity;
        ElapsedLaunchTime = 0f;
        IsLaunch = true;
        Rb.isKinematic = true;
        StartY = transform.position.y;
    }

    public void PlayLaunchEffect()
    {
        snackEffectController.PlayFlyingEffect();
    }
}