//====================================================
// �X�N���v�g���FJumpPlayer
// �쐬�ҁF����
// ���e�F�v���C���[�̃W�����v����
// �ŏI�X�V���F04/01
// 
// [Log]
// 04/01 ���� �X�N���v�g�쐬 
// 04/21 ���� �d�͊֘A��ʃX�N���v�g(ObjectGravity)�Ɉړ������܂���
// 04/23 ���� ���͂Ɋւ���d�l�ύX(PlayerInput(InputActionAsset))
//====================================================
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpPlayer : MonoBehaviour
{
    [SerializeField]
    private float JumpForce = 10.0f;        // �W�����v��
    [SerializeField]
    private float GroundCheckRadius = 0.2f; // �n�ʔ��蔼�a

    private Rigidbody Rb;    // �v���C���[��Rigidbody
    private bool IsGrounded; // �n�ʂɐڂ��Ă��邩�ǂ���

    // �n�ʂ��m�F���邽�߂̃^�O
    private string GroundTag = "Ground";

    private PlayerInput PlayerInput; // �v���C���[�̓��͂��Ǘ�����component
    private InputAction JumpAction;  // �W�����v�p��InputAction

    private void Awake()
    {
        // �����ɃA�^�b�`����Ă��� PlayerInput ���擾
        PlayerInput = GetComponent<PlayerInput>();
    }
    void Start()
    {
        Rb = GetComponent<Rigidbody>(); // Rigidbody���擾

        // �Ή�����InputAction���擾
        JumpAction = PlayerInput.actions["Jump"];
    }
   
    void Update()
    {
        // �n�ʂɐڂ��Ă��邩�m�F
        IsGrounded = CheckIfGrounded();
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

    // �L�������ꂽ�Ƃ��ɁuJump�v�A�N�V�����ɃC�x���g�n���h����o�^
    private void OnEnable()
    {
        // �v���C���[���͂� "Jump" �A�N�V���������s���ꂽ�Ƃ��� OnJump ���\�b�h���Ăяo��
        PlayerInput.actions["Jump"].performed += OnJump;
    }

    // ���������ꂽ�Ƃ��ɃC�x���g�n���h��������
    private void OnDisable()
    {
        if (PlayerInput != null && PlayerInput.actions != null)
        {
            // "Jump" �A�N�V�����ɓo�^����Ă��� OnJump ������
            PlayerInput.actions["Jump"].performed -= OnJump;
        }
    }

    // �uJump�v�A�N�V�����̓��͂����������Ƃ��ɌĂ΂�鏈��
    public void OnJump(InputAction.CallbackContext context)
    {
        // ���͂������i�{�^���������ꂽ�u�ԁj���n�ʂɐڒn���Ă���Ƃ��ɃW�����v�����s
        if (context.performed && IsGrounded)
        {
            Jump();
        }
    }
}
