//====================================================
// [Log]
// 05/13 ���{ ���t�e�B���O�񐔂ɉ����Ĕ�ԗ͂��i�K�I�ɏオ��
// 05/30 �r�� �X�R�A�̃R���{�{�[�i�X�̃��Z�b�g������
// 06/05 ���{ ���i�q�b�g�h�~������ǉ�
// 06/06 �X�e �A�j���[�V�����ƃ^�C�~���O���������邽�߂Ƀq�b�g�X�g�b�v�ύX
// 06/13 �X�e �J�����̐���t���O�ǉ�
// 06/13 ���� �X�i�b�N�������ɕK�v�ȃR���|�[�l���g���Q�Ƃ���SetTarget�֐���ǉ�
// 06/13 �r�� �N���A�J�E���g�̃^�C�~���O�����t�e�B���O�����������ɕύX
// 06/19 ���� �v���C���[���X�i�b�N�ɓ��������Ƃ���SE����
// 06/20 �X�e �A�j���[�V�����̐ݒ�
// 06/20 �r�� �X�i�b�N���u���̃y�i���e�B������ǉ�
// 06/23 ���� �ł��グ�Ɠ����^�C�~���O�Ŏ��̃��[�v����v�Z����悤�ɕύX
// 06/23 ���� �������n�܂����^�C�~���O�Ń��[�v����悤�ɐݒ�
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

    [Header("�X�i�b�N���u���̃y�i���e�B�ݒ�")]
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

    // ���̃��[�v���ێ�����
    private Vector3 nextWarpPosition = Vector3.zero;
    public Vector3 NextWarpPosition => nextWarpPosition;

    private void Start()
    {
        Rb = GetComponent<Rigidbody>();

        FallPoint = GetComponent<FallPointCalculator>();

    }

    // �������Ɉ����œn���ꂽ�R���|�[�l���g��ݒ肷��
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
                Debug.Log($"�������n�܂������߁A���[�v���܂���");
                DoWarp();
            }
        }

        // �X�i�b�N���u�y�i���e�B�̏���
        if (!IsFlyingAway)
        {
            PenaltyCount += Time.deltaTime;

            if (IsPenaltyTime)
            {
                // ��莞�Ԃ��ƂɃX�R�A����
                if (PenaltyCount > ScoreDecreaseInterval)
                {
                    if (flyingPoint != null)
                    {
                        flyingPoint.DecreaseScore(ScoreDecreasePoint);
                    }

                    // �J�E���g���Z�b�g
                    PenaltyCount = 0f;
                }
            }
            else
            {
                // �y�i���e�B�J�n���Ԃ𒴂�����X�R�A�����J�n
                if (PenaltyCount > ScorePenaltyStartTime)
                {
                    if (flyingPoint != null)
                    {
                        flyingPoint.DecreaseScore(ScoreDecreasePoint);
                    }

                    IsPenaltyTime = true;

                    // �J�E���g���Z�b�g
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
            PenaltyCount = 0f;�@// �J�E���g���Z�b�g

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

            // ���[�v����v�Z
            MoveToRandomXZInRespawnArea();

            // ���b�N�I������Ώۂ�ݒ�
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

    // ���X�|�[���ʒu
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

    // ���[�v���������s
    private void DoWarp()
    {
        // ���[�v���̍������Œ�⏞
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



