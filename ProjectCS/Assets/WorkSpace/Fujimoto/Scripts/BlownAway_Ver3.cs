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
// 06/27 ���� �v���C���[���X�i�b�N�ɓ��������Ƃ���SE���ʒ�������
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

    [SerializeField, Range(0.0f, 1.0f)]
    private float SEVolume = 0.5f;

    // ���̃��[�v���ێ�����
    private Vector3 nextWarpPosition = Vector3.zero;

    public Vector3 NextWarpPosition => nextWarpPosition;

    private bool IsWaiting = true;

    [Header("�X�i�b�N�ł��グ�֘A�̐��l")]
    private bool IsLaunch = false;
    [SerializeField] private float FirstTargetHeight = 500f;
    [SerializeField] private float MaxTargetHeight = 1000f;
    [SerializeField] private float LaunchMultiplier = 1.1f;
    private float CurrentTargetHeight = 500f;
    private float InitialVelocity;            // ����
    private float CurrentVelocity;            // ���݂̑��x
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

    // �������Ɉ����œn���ꂽ�R���|�[�l���g��ݒ肷��
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

        // 1�t���[����~
        if (IsWaiting)
        {
            IsWaiting = false;
            return;
        }

        // �ł��グ��
        if (IsLaunch)
        {
            ElapsedLaunchTime += Time.deltaTime;

            // ���݂̍����Ƒ��x���v�Z
            float newY = StartY + (InitialVelocity * ElapsedLaunchTime) - (0.5f * GravityScaleY * ElapsedLaunchTime * ElapsedLaunchTime);
            CurrentVelocity = InitialVelocity - GravityScaleY * ElapsedLaunchTime;

            // ����艺�ɗ�����̂�h��
            if (newY <= StartY + CurrentTargetHeight && CurrentVelocity >= 0f)
            {
                transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            }
            else
            {
                IsLaunch = false; // ���_�ɒB������~�߂�
                HitSnack = true;
                Rb.isKinematic = false;
                CurrentTargetHeight *= LaunchMultiplier;
                CurrentTargetHeight = Mathf.Min(CurrentTargetHeight, MaxTargetHeight);
            }
        }
        // �ł��グ�ȊO�i�������Ȃǁj
        else
        {
            if(HitNextFallArea)
            {
                HitNextFallArea = false;
                Rb.linearVelocity = Vector3.zero;
                if (IsRespawn)
                {
                    Debug.Log($"�������n�܂������߁A���[�v���܂���");
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
            PenaltyCount = 0f;�@// �J�E���g���Z�b�g

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

            // ���[�v����v�Z
            MoveToRandomXZInRespawnArea();

            // ���b�N�I������Ώۂ�ݒ�
            CameraFunction.SetSnack(gameObject.transform);

            CameraFunction.StartLockOn(true);

            Launch(); // �ł��グ���J�n������
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

    // ���X�|�[���ʒu
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

    // ���[�v���������s
    private void DoWarp()
    {
        // ���[�v���̍������Œ�⏞
        if (transform.position.y > nextWarpPosition.y)
        {
            nextWarpPosition.y = transform.position.y;
        }
        transform.position = nextWarpPosition;
    }

    public void Launch()
    {
        // �K�v�ȏ������v�Z�Fv = sqrt(2gh)
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