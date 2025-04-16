//====================================================
// �X�N���v�g���FJumpPlayer
// �쐬�ҁF����
// ���e�F�v���C���[�̃W�����v����
// �ŏI�X�V���F04/01
// 
// [Log]
// 04/01 ���� �X�N���v�g�쐬 
// 
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
    private Vector3 GravityScale = new Vector3(0.0f, -9.8f, 0.0f);     // �d�͂̑傫��

    private Rigidbody Rb;    // �v���C���[��Rigidbody
    private bool IsGrounded; // �n�ʂɐڂ��Ă��邩�ǂ���

    // �n�ʂ��m�F���邽�߂̃^�O
    private string GroundTag = "Ground";

    void Start()
    {
        Rb = GetComponent<Rigidbody>(); // Rigidbody���擾
    }
   
    void Update()
    {
        // �d�͕����ɉ���������
        Rb.AddForce(GravityScale, ForceMode.Acceleration);

        // �n�ʂɐڂ��Ă��邩�m�F
        IsGrounded = CheckIfGrounded();

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

        // �W�����v����
        if (jumpInputDetected && IsGrounded)
        {
            Jump();
        }
    }

    // �n�ʔ���
    bool CheckIfGrounded()
    {
        // �n�ʂ̃^�O�����I�u�W�F�N�g�ɐڂ��Ă��邩�m�F
        // �v���C���[�̑������班�����Ƀ`�F�b�N���s���A���a���Œn�ʂ̃^�O�����邩�m�F
        Collider[] colliders = Physics.OverlapSphere(transform.position - Vector3.up * 0.1f, GroundCheckRadius);

        foreach (var collider in colliders)
        {
            if (collider.CompareTag(GroundTag))
            {
                return true; // �n�ʂɐڂ��Ă���ꍇ
            }
        }
        return false; // �n�ʂɐڂ��Ă��Ȃ��ꍇ
    }

    // �W�����v����
    void Jump()
    {
        // �W�����v�͂�������
        Rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        Debug.Log("�W�����v��������");
    }
}
