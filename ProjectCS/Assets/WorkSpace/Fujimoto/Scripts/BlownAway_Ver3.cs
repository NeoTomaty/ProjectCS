//====================================================
// �X�N���v�g���FBlownAway_Ver3
// �쐬�ҁF���{
// ���e�F���t�e�B���O�񐔂ɉ����Ĕ�ԗ͂��i�K�I�ɏオ��
// [Log]
// 05/13 ���{ ���t�e�B���O�񐔂ɉ����Ĕ�ԗ͂��i�K�I�ɏオ��
// 05/30 �r�� �X�R�A�̃R���{�{�[�i�X�̃��Z�b�g������
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

    [SerializeField]
    [Header("�N���A�������Ǘ����Ă���I�u�W�F�N�g")]
    private ClearConditions ClearConditionsScript; // ���t�e�B���O�񐔊Ǘ��̃X�N���v�g

    private bool IsRespawn = true;

    void Start()
    {
        Rb = GetComponent<Rigidbody>();

        FallPoint = GetComponent<FallPointCalculator>();

        if (!CameraFunction) Debug.LogError("CameraFunction���ݒ肳��Ă��܂���");
    }

    void OnTriggerEnter(Collider other)
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
    void Update()
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
    }

    // �����X�s�[�h�𐧌�����
    void FixedUpdate()
    {
        // �����������x������𒴂��Ă����琧��
        if (Rb.linearVelocity.y < -MaxFallSpeed)
        {
            Vector3 clampedVelocity = Rb.linearVelocity;
            clampedVelocity.y = -MaxFallSpeed;
            Rb.linearVelocity = clampedVelocity;

            Debug.Log($"�������x�𐧌����܂���: {Rb.linearVelocity.y}");
        }
    }
   

    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Ground"))
        {
            flyingPoint.ResetComboBonus();
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            // ���i�q�b�g�h�~
            if (!HitSnack) return;

            // ���i�q�b�g�h�~�t���Ofalse
            HitSnack = false;

            ClearConditionsScript.CheckLiftingCount();

            //ClearConditionsScript.
            // Snack�ɐG�ꂽ��HitNextFallArea��true�ɖ߂�
            HitNextFallArea = true;

            // ���t�e�B���O�񐔂����Z
            liftingCount++;

            // �͂��v�Z�F��{ + �� �~ ������
            float force = baseForce + (liftingCount * forcePerLift);

            // ������̃x�N�g���ɗ͂�������
            Vector3 forceDir = Vector3.up * force;
            Rb.AddForce(forceDir, ForceMode.Impulse);

            Debug.Log(liftingCount);


            // snack�̍��W�̃��O
            Debug.Log($"snack�̍��W: {transform.position}");

            // �Q�[�W�ɂ��␳
            if (LiftingJump != null)
            {
                if (LiftingJump.IsLiftingPart)
                {
                    //ForceDirection *= LiftingJump.GetForce * LiftingJump.GetJumpPower;

                    // �v���C���[�̃��t�e�B���O�p�[�g���I������
                    LiftingJump.FinishLiftingJump();    // AddForce�̑O�ɌĂяo���Ȃ��ƃX�i�b�N����΂Ȃ�

                    if (flyingPoint != null)
                    {
                        flyingPoint.CalculateScore();
                        Debug.LogWarning("�X�R�A�v�Z�J�n");
                    }
                }
            }

            flyingPoint.CalculateScore();

            // �q�b�g�X�g�b�v���J�n����
            StartCoroutine(HitStop());

            // �J�����̋������b�N�I���J�n
            CameraFunction.StartLockOn(true);
        }
    }

    // �q�b�g�X�g�b�v�֐�
    System.Collections.IEnumerator HitStop()
    {
        Time.timeScale = 0f; // ���Ԃ��~�߂�i�X���[���[�V�����j
        float timer = 0f;

        // ���A���^�C���ň�莞�ԑ҂�
        while (timer < hitStopTime)
        {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        Time.timeScale = 1f; // ���Ԃ��ĊJ����
    }

    // ���X�|�[���ʒu
    private void MoveToRandomXZInRespawnArea()
    {
        if (RespawnArea == null || GroundArea == null)
        {
            Debug.LogWarning("RespawnArea���ݒ肳��Ă��܂���");
            return;
        }

        // �v���C���[�����ꂽ�瑽�i�q�b�g�h�~�t���O��true
        HitSnack = true;

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