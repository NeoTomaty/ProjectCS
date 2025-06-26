//====================================================
// �X�N���v�g���FBlownAway_Ver3
// �쐬�ҁF���{
// ���e�F���t�e�B���O�񐔂ɉ����Ĕ�ԗ͂��i�K�I�ɏオ��
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
//====================================================
using UnityEngine;
using System.Collections;

public class BlownAway_Ver3 : MonoBehaviour
{
    [Header("�q�b�g�X�g�b�v����")]
    [SerializeField] private float hitStopTime = 0.5f;            // �q�b�g�X�g�b�v�̎��ԁi�b�j

    [Header("������т̊�{�ݒ�")]
    [SerializeField] private float baseForce = 100f;      // ������ԗ�

    [SerializeField] private float forcePerLift = 100f;    // ���t�e�B���O1�񂲂Ƃɒǉ������

    [SerializeField]
    private float MinUpwardForce = 50.0f;  // �^��ւ̗́i�ŏ��j

    [SerializeField]
    private float MaxUpwardForce = 200.0f; // �^��ւ̗́i�ő�j

    [SerializeField]
    private float MinRandomXYRange = 0.0f; // �����_���ɉ�����XY���͈̔́i�ŏ��j

    [SerializeField]
    private float MaxRandomXYRange = 0.0f; // �����_���ɉ�����XY���͈̔́i�ő�j

    [SerializeField]
    private float MinFallSpeed = 0.0f; // �������̃X�s�[�g�i�ŏ��j

    [SerializeField]
    private float MaxFallSpeed = 30.0f; // �������̃X�s�[�g�i�ő�j

    [SerializeField]
    private Transform RespawnArea;         // �ړ�������͈̓I�u�W�F�g

    [SerializeField]
    private LiftingJump LiftingJump; // ���t�e�B���O�W�����v�̃X�N���v�g

    [SerializeField]
    private Transform GroundArea;   // �X�e�[�W�͈̔͂������I�u�W�F�N�g

    [SerializeField]
    private CameraFunction CameraFunction;

    [SerializeField]
    private FlyingPoint flyingPoint;�@// �X�R�A�v�Z�p�X�N���v�g

    private FallPointCalculator FallPoint; // �����n�_���v�Z����X�N���v�g

    private float previousVerticalVelocity = 0f;  // ���X�|�[���O��Y�������x��ۑ�

    private bool HitNextFallArea = true;    // ���X�|�[���G���A�ɘA���œ�����Ȃ��悤�ɂ���

    private bool HitSnack = true; // snack�ɑ��i�q�b�g���Ȃ��悤�ɂ���

    private Rigidbody Rb;

    private int liftingCount = 1; // ������ԗ͂Ŏg�p���郊�t�e�B���O��

    private bool isHitStopActive = false;

    private bool shouldEndHitStop = false;

    [SerializeField]
    [Header("�N���A�������Ǘ����Ă���I�u�W�F�N�g")]
    private ClearConditions ClearConditionsScript; // ���t�e�B���O�񐔊Ǘ��̃X�N���v�g

    private bool IsRespawn = true;

    private bool IsFlyingAway = true;

    [Header("�X�i�b�N���u���̃y�i���e�B�ݒ�")]
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
    [Header("������уG�t�F�N�g")]
    private SnackEffectController snackEffectController;

    [Header("SE")]
    //���ʉ���炷���߂�AudioSource
    [SerializeField] private AudioSource audioSource;

    //�v���C���[�����������Ƃ��̌��ʉ�
    [SerializeField] private AudioClip HitSE;

    [Header("�A�j���[�V����")]
    [SerializeField] private PlayerAnimationController playerAnimController;

    private void Start()
    {
        Rb = GetComponent<Rigidbody>();

        FallPoint = GetComponent<FallPointCalculator>();

        if (!CameraFunction) Debug.LogError("CameraFunction���ݒ肳��Ă��܂���");
    }

    // �������Ɉ����œn���ꂽ�R���|�[�l���g��ݒ肷��
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
        Debug.Log($"�ڐG: {other.name}");

        if (other.CompareTag("Respawn") && HitNextFallArea == true)
        {
            HitNextFallArea = false;

            // ���݂�Y�������x��ۑ�
            previousVerticalVelocity = Rb.linearVelocity.y;

            MoveToRandomXZInRespawnArea();
        }
    }

    // snack��Respawn�I�u�W�F�N�g���X�������Ă����X�|�[������
    private void Update()
    {
        // Respawn�I�u�W�F�N�g�̂�荂���ʒu�ɂ����烊�X�|�[��
        if (RespawnArea && transform.position.y > RespawnArea.position.y && HitNextFallArea == true)
        {
            HitNextFallArea = false;
            // ���݂�Y�������x��ۑ�
            previousVerticalVelocity = Rb.linearVelocity.y;

            // ���X�|�[�������łȂ����
            if (IsRespawn)
            {
                Debug.Log($"Respawn�I�u�W�F�N�g�̍����𒴂������߃��X�|�[��");
                MoveToRandomXZInRespawnArea();
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
                    // �֐��Ăяo��
                    if (FlyingPointScript != null)
                    {
                        FlyingPointScript.DecreaseScore(ScoreDecreasePoint);
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
                    IsPenaltyTime = true;

                    // �J�E���g���Z�b�g
                    PenaltyCount = 0f;
                }
            }
        }
    }

    // �����X�s�[�h�Ƒł�������͂𐧌�����
    private void FixedUpdate()
    {
        // �����������x������𒴂��Ă����琧��
        if (Rb.linearVelocity.y < -MaxFallSpeed)
        {
            Vector3 clampedVelocity = Rb.linearVelocity;
            clampedVelocity.y = -MaxFallSpeed;
            Rb.linearVelocity = clampedVelocity;

            // �v���C���[�����ꂽ�瑽�i�q�b�g�h�~�t���O��true
            HitSnack = true;

            // ������уG�t�F�N�g��~
            //snackEffectController.StopFlyingEffect();

            Debug.Log($"�������x�𐧌����܂���: {Rb.linearVelocity.y}");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // ������΂���Ԃ̎��������s
            if (IsFlyingAway)
            {
                // �R���{�{�[�i�X���Z�b�g
                flyingPoint.ResetComboBonus();

                // �N���A�J�E���g�i�s
                ClearConditionsScript.CheckLiftingCount(gameObject);

                IsFlyingAway = false;
            }
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            //SE���Đ�
            if (audioSource != null && HitSE != null)
            {
                audioSource.PlayOneShot(HitSE);
            }

            // ���i�q�b�g�h�~
            if (!HitSnack) return;

            // ���i�q�b�g�h�~�t���Ofalse
            HitSnack = false;


            // ������΂���Ԃֈȍ~
            IsFlyingAway = true;
            IsPenaltyTime = false;
            PenaltyCount = 0f;�@// �J�E���g���Z�b�g

            //ClearConditionsScript.
            // Snack�ɐG�ꂽ��HitNextFallArea��true�ɖ߂�
            HitNextFallArea = true;

            // ���t�e�B���O�񐔂����Z
            liftingCount++;

            // �͂��v�Z�F��{ + �� �~ ������
            float force = baseForce + (liftingCount * forcePerLift);

            Debug.Log($"�́F{force}");

            // �͂̐���
            if (force > MaxUpwardForce)
            {
                force = MaxUpwardForce;
                Debug.Log($"������̗́F{force}");
            }

            // ������̃x�N�g���ɗ͂�������
            Vector3 forceDir = Vector3.up * force;
            Rb.AddForce(forceDir, ForceMode.Impulse);

            Debug.Log(liftingCount);

            // snack�̍��W�̃��O
            Debug.Log($"snack�̍��W: {transform.position}");

            // �Q�[�W�ɂ��␳
            if (LiftingJump != null)
            {
                Debug.Log("���t�e�B���O�J�n");
                if (LiftingJump.IsLiftingPart)
                {
                    // �v���C���[�̃��t�e�B���O�p�[�g���I������
                    LiftingJump.FinishLiftingJump();

                    if (flyingPoint != null)
                    {
                        flyingPoint.CalculateScore();
                        Debug.LogWarning("�X�R�A�v�Z�J�n");
                        Debug.LogWarning($"�ł��������{Rb.linearVelocity.y}");
                    }

                    // �Q�[�W���g��ꂽ�Ƃ��͎蓮�����^�C�v
                    StartCoroutine(HitStopManual());
                }
                else
                {
                    // �Q�[�W���g���Ă��Ȃ��Ƃ���0.5�b�Ŏ�������
                    StartCoroutine(HitStopTimed(0.5f));
                }
            }
            else
            {
                // LiftingJump �� null�i�ʏ�q�b�g�Ȃǁj�ł���������
                StartCoroutine(HitStopTimed(0.5f));
            }

            flyingPoint.CalculateScore();

            // �q�b�g�X�g�b�v���J�n����
            //  StartCoroutine(HitStop());

            // ���b�N�I������Ώۂ�ݒ�
            CameraFunction.SetSnack(gameObject.transform);

            // �J�����̋������b�N�I���J�n
            CameraFunction.StartLockOn(true);
        }
    }

    // �q�b�g�X�g�b�v�֐�
    //private System.Collections.IEnumerator HitStop()
    //{
    //    Time.timeScale = 0f; // ���Ԃ��~�߂�i�X���[���[�V�����j
    //    float timer = 0f;

    //    // ���A���^�C���ň�莞�ԑ҂�
    //    while (timer < hitStopTime)
    //    {
    //        timer += Time.unscaledDeltaTime;
    //        yield return null;
    //    }

    //    Time.timeScale = 1f; // ���Ԃ��ĊJ����

    //    Debug.Log("�q�b�g�X�g�b�v�J�n");
    //}

    //private IEnumerator HitStop()
    //{
    //    if (isHitStopActive) yield break;

    //    Time.timeScale = 0f;
    //    isHitStopActive = true;
    //    shouldEndHitStop = false;

    //    Debug.Log("�q�b�g�X�g�b�v�J�n");

    //    // �O������ EndHitStop() ���Ă΂��܂őҋ@
    //    while (!shouldEndHitStop)
    //    {
    //        yield return null;
    //    }

    //    Time.timeScale = 1f;
    //    isHitStopActive = false;

    //    Debug.Log("�q�b�g�X�g�b�v�I��");
    //}

    private IEnumerator HitStopTimed(float duration)
    {
        if (isHitStopActive) yield break;

        Time.timeScale = 0f;
        isHitStopActive = true;

        Debug.Log($"�q�b�g�X�g�b�v�i���������j�J�n: {duration}�b");

        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        Time.timeScale = 1f;
        isHitStopActive = false;

        Debug.Log("�q�b�g�X�g�b�v�I���i�����j");

        // ������уG�t�F�N�g�J�n
        snackEffectController.PlayFlyingEffect();
    }

    private IEnumerator HitStopManual()
    {
        if (isHitStopActive) yield break;

        Time.timeScale = 0f;
        isHitStopActive = true;
        shouldEndHitStop = false;

        Debug.Log("�q�b�g�X�g�b�v�i�蓮�����j�J�n");

        playerAnimController.PlayRandomAnimation();

        // �O������ EndHitStop() ���Ă΂��܂ő҂�
        while (!shouldEndHitStop)
        {
            yield return null;
        }

        Time.timeScale = 1f;
        isHitStopActive = false;

        Debug.Log("�q�b�g�X�g�b�v�I���i�蓮�j");

        // ������уG�t�F�N�g�J�n
        snackEffectController.PlayFlyingEffect();
    }

    // �O���X�N���v�g����Ăяo���ăq�b�g�X�g�b�v���I��������
    public void EndHitStop()
    {
        if (isHitStopActive)
        {
            shouldEndHitStop = true;
        }
    }

    // ���X�|�[���ʒu
    private void MoveToRandomXZInRespawnArea()
    {
        if (RespawnArea == null || GroundArea == null)
        {
            Debug.LogWarning("RespawnArea���ݒ肳��Ă��܂���");
            return;
        }

        // �͈͎擾
        Vector3 respawnCenter = RespawnArea.position;
        Vector3 respawnSize = RespawnArea.localScale;

        Vector3 groundCenter = GroundArea.position;
        Vector3 groundSize = GroundArea.localScale;

        // X��Z�̍ŏ��E�ő�𗼕��͈̔͂ŋ��ʂ��镔���ɐ���
        float minX = Mathf.Max(respawnCenter.x - respawnSize.x / 2, groundCenter.x - groundSize.x / 2);
        float maxX = Mathf.Min(respawnCenter.x + respawnSize.x / 2, groundCenter.x + groundSize.x / 2);

        float minZ = Mathf.Max(respawnCenter.z - respawnSize.z / 2, groundCenter.z - groundSize.z / 2);
        float maxZ = Mathf.Min(respawnCenter.z + respawnSize.z / 2, groundCenter.z + groundSize.z / 2);

        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);

        // Y���W��RespawnArea�̍����ɐݒ�
        float y = respawnCenter.y;

        // �ۑ�����������̗͂���
        Vector3 newPos = new Vector3(randomX, y, randomZ);
        transform.position = newPos;
        Rb.linearVelocity = new Vector3(0f, previousVerticalVelocity, 0f);

        Debug.Log($"�㏸���x: {Rb.linearVelocity.y}");
        Debug.Log($"���X�|�[�����W�i�O���E���h���j: {newPos}");

        FallPoint?.CalculateGroundPoint();
    }

    // �N���A������
    public void OnClear()
    {
        IsRespawn = false;
    }
}