using UnityEngine;
using UnityEngine.InputSystem;

public class TestIsHitAny : MonoBehaviour
{
    // �ǂɏՓ˂����ۂ̉�����
    [Header("�ǂɏՓ˂����Ƃ��̉����x")]
    [SerializeField] private float Acceleration = 1.0f;

    [Header("Y�����̑��x")]
    [SerializeField]
    private float MaxVelocityY = 50.0f;  // Y���ő�̑���
    [SerializeField]
    private float MinVelocityY = -50.0f; // Y���ŏ��̑���


    [Header("�{�^�����͂ɂ����������s���邩�ǂ���")]
    [SerializeField]
    private bool IsInputEnabled = true;
    [Header("�{�^�����͂܂��̐ݒ�")]
    [SerializeField]
    private float InputAcceptDuration = 1.0f;// �ǂɏՓˌ�̓��͎�t����
    private float InputAcceptTimer = 0.0f;   // ���͎�t�̌o�ߎ���


    // ���[�v��̍��W
    [Header("���X�|�[����̍��W")]
    [SerializeField]
    public Vector3 warpPosition = new Vector3(0, 0, 0);


    private bool IsJumpReflect = false;  // �W�����v���̕ǔ��˂ŗ͂������邩�ǂ���
    private bool IsHitWall = false;      // �ǂɏՓ˂������ǂ���

    private Rigidbody Rb;    // �v���C���[��Rigidbody

    private LiftingJump LiftingJumpScript; // ���t�e�B���O�W�����v�̃X�N���v�g


    // �v���C���[�̈ړ������Ƒ��x�ɃA�N�Z�X���邽�߂̕ϐ�
    // �����I�u�W�F�N�g�ɃA�^�b�`����Ă���X�N���v�g�ł���Ƃ����z��ł̎���
    MovePlayer MovePlayerScript;    //���ۂ̃X�N���v�g

    [SerializeField]
    private TestCameraFunction CameraFunction;

    void Start()
    {
        MovePlayerScript = GetComponent<MovePlayer>();
        Rb = GetComponent<Rigidbody>(); // Rigidbody���擾
        LiftingJumpScript = GetComponent<LiftingJump>(); // LiftingJump���擾

        if (MovePlayerScript == null)
        {
            Debug.LogError("�v���C���[ >> MovePlayer�X�N���v�g��������܂���");
        }
    }

    private void Update()
    {
        if (!IsHitWall) return;

        if (InputAcceptTimer > 0.0f)
        {
            InputAcceptTimer -= Time.deltaTime;

            // �W�����v�̓��̓`�F�b�N
            bool AccelerationInputDetected = false;

            // �Q�[���p�b�h���ڑ�����Ă���ꍇ�AB�{�^���i���j��D��
            if (Gamepad.current != null)
            {
                AccelerationInputDetected = Input.GetKeyDown(KeyCode.JoystickButton1);
            }
            // �Q�[���p�b�h���ڑ�����Ă��Ȃ��ꍇ�A�G���^�[�L�[�i���j���g�p
            else
            {
                AccelerationInputDetected = Input.GetKeyDown(KeyCode.Return);
            }

            // ��������
            if (AccelerationInputDetected)
            {
                // �v���C���[������
                MovePlayerScript.PlayerSpeedManager.SetAccelerationValue(Acceleration);
                Debug.Log("��������");
            }
        }
        else
        {
            IsHitWall = false;
        }
    }

    // �Փ˂����ۂ̏���
    private void OnCollisionEnter(Collision collision)
    {
        if (MovePlayerScript == null) return;

        // �Փ˂����I�u�W�F�N�g�̃^�O���`�F�b�N
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "BrokenWall")
        {
            Debug.Log("�v���C���[ >> �ǂɓ�����܂���");

            // ���͂ɂ����������Ȃ��̂Ȃ�A���̂܂܉������������s
            if (!IsInputEnabled)
            {
                // �v���C���[������
                MovePlayerScript.PlayerSpeedManager.SetAccelerationValue(Acceleration);
            }
            else
            {
                IsHitWall = true;
                InputAcceptTimer = InputAcceptDuration;
            }

            // �v���C���[�̈ړ��x�N�g�����擾
            Vector3 PlayerMoveDirection = MovePlayerScript.GetMoveDirection;

            // �ǂ̐ڐG�ʂ̖@���x�N�g�����擾
            Vector3 Normal = collision.contacts[0].normal;

            // ���˃x�N�g�����v�Z
            Vector3 Reflect = Vector3.Reflect(PlayerMoveDirection, Normal).normalized;

            // ���˃x�N�g�����v���C���[�ɓK�p
            MovePlayerScript.SetMoveDirection(Reflect);

            // �q�b�g�X�g�b�v���s
            MovePlayerScript.StartHitStop();

            if (!IsJumpReflect) return;

            // �ǔ��ˌ�̗͂̕�����Velocity�ɉ����Č��肷��
            Reflect = new Vector3(0.0f, Mathf.Clamp(Rb.linearVelocity.y, MinVelocityY, MaxVelocityY), 0.0f);

            // �ǔ��ˌ�Ɉ��̗͂�������
            Rb.AddForce(Reflect, ForceMode.Impulse);

            // �ǔ��ˎ���AddForce�𖳌��ɂ���
            IsJumpReflect = false;

            CameraFunction.SetIsPlayerWallHit(true, Reflect);
        }
        // �n�ʂɒ������ꍇ�͕ω�����MoveDirectionY��0�ɂ���
        else if (collision.gameObject.tag == "Ground")
        {
            // �v���C���[�̈ړ��x�N�g�����擾
            Vector3 PlayerMoveDirection = MovePlayerScript.GetMoveDirection;

            // PlayerMoveDirection.y��0�Ƀ��Z�b�g
            PlayerMoveDirection.y = 0.0f;

            // PlayerMoveDirection�̍X�V
            MovePlayerScript.SetMoveDirection(PlayerMoveDirection);

            // �ǔ��ˎ���AddForce��L���ɂ���
            IsJumpReflect = true;
        }

    }

    // �Փ˂����ۂ̏����iis Trigger�j
    private void OnTriggerEnter(Collider other)
    {
        // �Փ˂����I�u�W�F�N�g�̃^�O���`�F�b�N
        if (other.gameObject.tag == "Respawn")
        {
            // �I�u�W�F�N�g�̈ʒu��warpPosition�ɕύX����
            transform.position = warpPosition;
        }
    }
}
