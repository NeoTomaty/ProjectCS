//======================================================
// [PlayerHitWall]
// �쐬�ҁF�r��C
<<<<<<< HEAD:ProjectCS/Assets/WorkSpace/Arai/Scripts/PlayerHitWall.cs
// �ŏI�X�V���F4/07
=======
// �ŏI�X�V���F04/08
>>>>>>> 0722a15c18f14d9d7c606093145b759313198c29:ProjectCS/Assets/WorkSpace/Arai/Script/PlayerHitWall.cs
// 
// [Log]
// 03/31�@�r��@�v���C���[���ǂɏՓ˂����ۂ̋������쐬
// 03/31�@�r��@�ړ��̉��X�N���v�g�����삵������m�F
// 04/01�@�r��@Player�I�u�W�F�N�g�̖{�X�N���v�g�ɑΉ�
<<<<<<< HEAD:ProjectCS/Assets/WorkSpace/Arai/Scripts/PlayerHitWall.cs
// 04/07�@�r��@Tooltip�L�q���R�����g�ɕύX
=======
// 04/08�@�����@�ǔ��˂̎d�l��ύX
>>>>>>> 0722a15c18f14d9d7c606093145b759313198c29:ProjectCS/Assets/WorkSpace/Arai/Script/PlayerHitWall.cs
//======================================================

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerHitWall : MonoBehaviour
{
    // �ǂɏՓ˂����ۂ̉�����
    [SerializeField] private float Acceleration = 1.0f;

    [SerializeField]
    private float MaxVelocityY = 50.0f;  // Y���ő�̑���
    [SerializeField]
    private float MinVelocityY = -50.0f; // Y���ŏ��̑���
    [SerializeField]
    private float InputAcceptDuration = 1.0f;// �ǂɏՓˌ�̓��͎�t����
    private float InputAcceptTimer = 0.0f;   // ���͎�t�̌o�ߎ���
    [Tooltip("���͂ɂ����������s���邩�ǂ���")]
    [SerializeField]
    private bool IsInputEnabled = true;

    private bool IsJumpReflect = false;  // �W�����v���̕ǔ��˂ŗ͂������邩�ǂ���
    private bool IsHitWall = false;      // �ǂɏՓ˂������ǂ���

    private Rigidbody Rb;    // �v���C���[��Rigidbody

    // �v���C���[�̈ړ������Ƒ��x�ɃA�N�Z�X���邽�߂̕ϐ�
    // �����I�u�W�F�N�g�ɃA�^�b�`����Ă���X�N���v�g�ł���Ƃ����z��ł̎���
    MovePlayer MovePlayerScript;    //���ۂ̃X�N���v�g

    void Start()
    {
        MovePlayerScript = GetComponent<MovePlayer>();
        Rb = GetComponent<Rigidbody>(); // Rigidbody���擾

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
            if(AccelerationInputDetected)
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


    private void OnCollisionEnter(Collision collision)
    {
        if (MovePlayerScript == null) return;

        // �Փ˂����I�u�W�F�N�g�̃^�O���`�F�b�N
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "BrokenWall")
        {
<<<<<<< HEAD:ProjectCS/Assets/WorkSpace/Arai/Scripts/PlayerHitWall.cs
=======
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

>>>>>>> origin/Takashita:ProjectCS/Assets/WorkSpace/Arai/Script/PlayerHitWall.cs
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
}
