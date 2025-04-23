//====================================================
// �X�N���v�g���FJumpPlayer
// �쐬�ҁF����
// ���e�F�v���C���[�̃W�����v����
// �ŏI�X�V���F04/01
// 
// [Log]
// 04/01 ���� �X�N���v�g�쐬 
<<<<<<< HEAD:ProjectCS/Assets/Project/Script/Player/Jump/JumpPlayer.cs
// 04/16 �|�� �W�����v�񐔂𐧌䂷�鏈����ǉ�
=======
// 04/21 ���� �d�͊֘A��ʃX�N���v�g(ObjectGravity)�Ɉړ������܂���
>>>>>>> dc80a4f9b3ae7b2c27f909c00a0c8f66ba447284:ProjectCS/Assets/Project/Script/Player/JumpPlayer.cs
// 
//====================================================
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpPlayer : MonoBehaviour
{
    [SerializeField]
    private float JumpForce = 10.0f;        // �W�����v��
    [SerializeField]
    private float GroundCheckRadius = 0.2f; // �n�ʔ��蔼�a

    [SerializeField]
    private int MaxJumpNum = 1;     // �W�����v�̍ő��

    [SerializeField]
    private int CurrentJumpNum = 0; // ���݂̃W�����v�\��

    private Rigidbody Rb;    // �v���C���[��Rigidbody
    private bool IsGrounded; // �n�ʂɐڂ��Ă��邩�ǂ���

    // �n�ʂ��m�F���邽�߂̃^�O
    private string GroundTag = "Ground";

    void Start()
    {
        Rb = GetComponent<Rigidbody>(); // Rigidbody���擾
        CurrentJumpNum = MaxJumpNum;    // �W�����v�񐔂��w��
    }
   
    void Update()
    {
<<<<<<< HEAD:ProjectCS/Assets/Project/Script/Player/Jump/JumpPlayer.cs
        // �d�͕����ɉ���������
        Rb.AddForce(GravityScale, ForceMode.Acceleration);

=======
        // �n�ʂɐڂ��Ă��邩�m�F
        IsGrounded = CheckIfGrounded();
>>>>>>> dc80a4f9b3ae7b2c27f909c00a0c8f66ba447284:ProjectCS/Assets/Project/Script/Player/JumpPlayer.cs

        // �W�����v�̓��̓`�F�b�N
        bool jumpInputDetected = false;

        // �Q�[���p�b�h���ڑ�����Ă���ꍇ�AA�{�^����D��
        if (Gamepad.current != null)
        {
            jumpInputDetected = Input.GetKeyDown(KeyCode.JoystickButton0);
        }
        // �Q�[���p�b�h���ڑ�����Ă��Ȃ��ꍇ�A�X�y�[�X�L�[���g�p
        else
        {
            jumpInputDetected = Input.GetKeyDown(KeyCode.Space);
        }

        // �W�����v�L�[�y�ђn�ʂɐڂ��Ă��āA�W�����v�񐔂��c���Ă����
        if (jumpInputDetected && IsGrounded && (CurrentJumpNum > 0))
        {
            // �f�o�b�O���O
            Debug.Log("�W�����v����");
            // �W�����v����
            Jump();
            // �W�����v�񐔂����炷
            CurrentJumpNum--;
        }
    }

    // �ڐG�����Ƃ�
    void OnCollisionEnter(Collision collision)
    {
        // �n�ʂƐڐG�����Ƃ��̂݃W�����v�񐔂���
        if (collision.gameObject.CompareTag(GroundTag))
        {
            Debug.Log("�W�����v�񐔉�");
            CurrentJumpNum = MaxJumpNum;
            IsGrounded = true;
        }
    }

    // �ڐG���Ȃ��Ȃ����u��
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(GroundTag))
        {
            IsGrounded = false;
        }
    }


    // �W�����v����
    void Jump()
    {
        // �W�����v�͂�������
        Rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        // �f�o�b�O���O
        Debug.Log("�W�����v��������");
    }
}
