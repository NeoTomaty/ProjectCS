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
//====================================================
using UnityEngine;
using System.Collections;

public class BlownAway_Ver3 : MonoBehaviour
{
    [SerializeField] private float hitStopTime = 0.5f;
    [SerializeField] private float baseForce = 100f;

    [SerializeField] private float forcePerLift = 100f;

    [SerializeField]
    private float MinUpwardForce = 50.0f; 

    [SerializeField]
    private float MaxUpwardForce = 200.0f;

    [SerializeField]
    private float MinRandomXYRange = 0.0f;

    [SerializeField]
    private float MaxRandomXYRange = 0.0f;

    [SerializeField]
    private float MinFallSpeed = 0.0f;

    [SerializeField]
    private float MaxFallSpeed = 30.0f;

    [SerializeField]
    private Transform RespawnArea;

    [SerializeField]
    private LiftingJump LiftingJump;

    [SerializeField]
    private Transform GroundArea;

    [SerializeField]
    private CameraFunction CameraFunction;

    [SerializeField]
    private FlyingPoint flyingPoint;

    private FallPointCalculator FallPoint;

    private float previousVerticalVelocity = 0f;

    private bool HitNextFallArea = true;

    private bool HitSnack = true;

    private Rigidbody Rb;

    private int liftingCount = 1;

    private bool isHitStopActive = false;

    private bool shouldEndHitStop = false;

    [SerializeField]
    private ClearConditions ClearConditionsScript;

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

    // 次のワープ先を保持する
    private Vector3 nextWarpPosition = Vector3.zero;
    public Vector3 NextWarpPosition => nextWarpPosition;

    private void Start()
    {
        Rb = GetComponent<Rigidbody>();

        FallPoint = GetComponent<FallPointCalculator>();

    }

    // 複製時に引数で渡されたコンポーネントを設定する
    public void SetTarget(CameraFunction CF, FlyingPoint FP, ClearConditions CC, LiftingJump LJ, Transform respawnArea, Transform groundArea, PlayerAnimationController PAC)
    {
        CameraFunction = CF;
        flyingPoint = FP;
        ClearConditionsScript = CC;
        LiftingJump = LJ;
        RespawnArea = respawnArea;
        GroundArea = groundArea;
        playerAnimController = PAC;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Respawn") && HitNextFallArea == true)
        {
            HitNextFallArea = false;

            previousVerticalVelocity = Rb.linearVelocity.y;

            MoveToRandomXZInRespawnArea();
        }
    }

    private void Update()
    {

        if (RespawnArea && Rb.linearVelocity.y < 0f && HitNextFallArea == true)
        {
            HitNextFallArea = false;
            previousVerticalVelocity = Rb.linearVelocity.y;

            if (IsRespawn)
            {
                Debug.Log($"落下が始まったため、ワープしました");
                DoWarp();
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
    }

    private void FixedUpdate()
    {
        if (Rb.linearVelocity.y < -MaxFallSpeed)
        {
            Vector3 clampedVelocity = Rb.linearVelocity;
            clampedVelocity.y = -MaxFallSpeed;
            Rb.linearVelocity = clampedVelocity;

            HitSnack = true;

            snackEffectController.StopFlyingEffect();

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
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
                audioSource.PlayOneShot(HitSE);
            }

            Collider snackCollider = GetComponent<Collider>();
            Collider playerCollider = collision.collider;

            Physics.IgnoreCollision(snackCollider, playerCollider, true);
            StartCoroutine(EnableCollisionLater(snackCollider, playerCollider, 1.0f));

            if (!HitSnack) return;

            HitSnack = false;


            IsFlyingAway = true;
            IsPenaltyTime = false;
            PenaltyCount = 0f;　// カウントリセット

            //ClearConditionsScript.
            HitNextFallArea = true;

            liftingCount++;

            float force = baseForce + (liftingCount * forcePerLift);


            if (force > MaxUpwardForce)
            {
                force = MaxUpwardForce;
            }
            
            Debug.Log(liftingCount);


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

            Rb.linearVelocity = Vector3.zero;
            Rb.angularVelocity = Vector3.zero;

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

        StartCoroutine(AddForceUpwardDelayed());
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

        StartCoroutine(AddForceUpwardDelayed());
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
        FallPoint?.CalculateGroundPoint();
    }

    public void OnClear()
    {
        IsRespawn = false;
    }

    // ワープ処理を実行
    private void DoWarp()
    {
        // ワープ時の高さを最低補償
        if(transform.position.y > nextWarpPosition.y)
        {
            nextWarpPosition.y = transform.position.y;
        }
     
        transform.position = nextWarpPosition;
        Rb.linearVelocity = new Vector3(0f, previousVerticalVelocity, 0f);
    }
    private IEnumerator EnableCollisionLater(Collider colA, Collider colB, float delay)
    {
        yield return new WaitForSeconds(delay);
        Physics.IgnoreCollision(colA, colB, false);
    }

    private IEnumerator AddForceUpwardDelayed()
    {
        yield return new WaitForFixedUpdate();

        Rb.linearVelocity = Vector3.zero;
        Rb.angularVelocity = Vector3.zero;

        float force = baseForce + (liftingCount * forcePerLift);
        if (force > MaxUpwardForce) force = MaxUpwardForce;

        Debug.Log($"Delayed AddForce: {force}");

        Vector3 forceDir = Vector3.up * force;
        Rb.AddForce(forceDir, ForceMode.Impulse);

    }

}



