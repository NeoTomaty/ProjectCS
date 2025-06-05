//======================================================
// [LiftingJump]
// �쐬�ҁF�r��C
// �ŏI�X�V���F05/29
// [Log]
// 04/26�@�r��@�L�[����͂�����^�[�Q�b�g�Ɍ������ĂԂ����ł����悤�Ɏ���
// 04/27�@�r��@�^�[�Q�b�g�ɋ߂Â�����X���[���[�V�������n�܂�悤�Ɏ���
// 04/27�@�r��@���X�N���v�g�ƍ��킹�ē��삷��悤�ɏC��
// 04/28�@�r��@�W�����v���̈ړ���transform����AddForce�ɕύX
// 04/29�@�r��@�`���[�W�W�����v�ɂ��W�����v�̃p���[�␳��ǉ�
// 05/01�@�r��@�^�[�Q�b�g�ȊO�̃I�u�W�F�N�g�ɏՓ˂����烊�t�e�B���O�W�����v�𒆎~���鏈����ǉ�
// 05/01�@�r��@�^�[�Q�b�g�ȊO�̃I�u�W�F�N�g�����蔲���鏈����ǉ�
// 05/01�@�r��@���~�Ƃ��蔲����؂�ւ���p�����[�^��ǉ�
// 05/02�@�r��@���t�e�B���O�W�����v���Ƀ^�[�Q�b�g�̓������~�߂鏈����ǉ�
// 05/03�@�r��@�W�����v���̈ړ���AddForce����transform�ɕύX
// 05/03�@�r��@�X���[���[�V�����̐�����@��timeScale����PlayerSpeedManager�ɕύX
// 05/03�@�r��@�X���[���[�V�������J�n���ꂽ���ɗ����Ă����悤�ɂȂ�o�O���C��
// 05/03�@�r��@���t�e�B���O�W�����v���ɍ��E�ړ��⌸�����̑���𖳌��ɂ��鏈����ǉ�
// 05/07�@�r��@���蔲�����[�h���L���Ȃ̂ɃI�u�W�F�N�g�����蔲�����Ȃ��o�O���C��
// 05/07�@�r��@�N���A�J�E���g�ő��i�q�b�g��������鋓�����C��
// 05/15�@�r��@�ړ����x�̕ω��̑�����PlayerSpeedManager����MovePlayer�ɕύX
// 05/28�@�r��@IsNearTargetNextFrame�֐���ǉ�
// 05/28�@�r��@���t�e�B���O�W�����v�ŃX�s�[�h��������QTE���������Ȃ����Ƃ�����o�O���C��
// 05/29�@�{�с@�|�[�Y��ʕ\���{�^���̒�~
// 05/29�@�r��@�X�N���v�g���s���̗D��x��ݒ�
// 05/29�@�X�e�@���f���̃t���O�ω�
// 05/29�@�r��@QTE�p�~
// 06/05�@�r��@�����������Ƃ��ă��t�e�B���O�W�����v�������I�����鏈����ǉ�
//======================================================
using UnityEngine;
using UnityEngine.InputSystem;

// �W�����v����ŖڕW�n�_�ɂԂ����ł��������̃e�X�g�p�̃X�N���v�g
// �v���C���[�ɃA�^�b�`
public class LiftingJump : MonoBehaviour
{
    [SerializeField] private GameObject TargetObject;                   // �ڕW�n�_
    private MovePlayer MovePlayer;                              // �v���C���[�̈ړ��X�N���v�g�̎Q��
    private ObjectGravity ObjectGravityScript;                  // �d�̓X�N���v�g�̎Q��
    private PlayerInput PlayerInput;                            // �v���C���[�̓��͂��Ǘ�����component
    public PlayerInput PauseInput;                              //�|�[�Y��ʂ̑���󂯎��

    [SerializeField] private float JumpSpeed = 2f;  // �W�����v���̈ړ����x�␳

    private float JumpPower = 0f;
    public float GetJumpPower => JumpPower; // �W�����v�͂̎擾

    [SerializeField] private bool IgnoreNonTargetCollisions = false;    // �^�[�Q�b�g�ȊO�Ƃ̏Փ˂𖳎����邩�ǂ���
    public bool IsIgnore => IgnoreNonTargetCollisions && IsJumping;

    private Collider[] AllColliders;    // �S�I�u�W�F�N�g�̓����蔻��

    private bool IsJumping = false;
    public bool IsLiftingPart => IsJumping; // ���t�e�B���O�W�����v�����ǂ���

    private float OnStartedPlayerHeight = 0f; // ���t�e�B���O�W�����v�J�n���̃v���C���[�̍���
    private float TerminateHeight = 200f; // ���t�e�B���O�W�����v�������I�ɏI�������鍂�x

    //[SerializeField] private PlayerAnimationController playerAnimController;

    public void SetJumpPower(float Power)
    {
        JumpPower = Power;
    }

    // ���t�e�B���O�W�����v���J�n����֐�
    public void StartLiftingJump()
    {
        PlayerInput.actions.Disable(); // ���͂𖳌��ɂ���
        PauseInput.actions.Disable(); // ���͂𖳌��ɂ���
        if (IgnoreNonTargetCollisions)   // ���蔲���L����
        {
            Collider SelfCollider = GetComponent<Collider>();                   // �����̃R���C�_�[���擾
            Collider TargetCollider = TargetObject.GetComponent<Collider>();    // �^�[�Q�b�g�̃R���C�_�[���擾

            foreach (Collider col in AllColliders)
            {
                // �����̃R���C�_�[�ƑS�ẴR���C�_�[�̓����蔻��𖳎�
                Physics.IgnoreCollision(SelfCollider, col, true);
            }

            // �����̃R���C�_�[�ƃ^�[�Q�b�g�̃R���C�_�[�̓����蔻�肾���L��
            Physics.IgnoreCollision(SelfCollider, TargetCollider, false);
        }

        // �^�[�Q�b�g�̓������~�߂�
        TargetObject.GetComponent<ObjectGravity>().IsActive = false;
        TargetObject.GetComponent<Rigidbody>().Sleep(); // �^�[�Q�b�g��Rigidbody���X���[�v��Ԃɂ���iisKinematic���Ƒ��i�q�b�g�����Ńq�b�g�J�E���g���i�݂�����j

        ObjectGravityScript.IsActive = false;
        GetComponent<Rigidbody>().Sleep();  // ������Rigidbody���X���[�v��Ԃɂ���i�X���[���[�V�������ɗ����Ă����΍�j

        IsJumping = true;

        // �v���C���[�̍�����ۑ�
        OnStartedPlayerHeight=transform.position.y;

        // �v���C���[����ڕW�n�_�ւ̃x�N�g�����v�Z
        Vector3 JumpDirection = (TargetObject.transform.position - transform.position);
        MovePlayer.SetMoveDirection(JumpDirection.normalized);

        // �v���C���[������������
        MovePlayer.MoveSpeedMultiplier = JumpSpeed * JumpPower;
    }

    // ���t�e�B���O�W�����v���~����֐�
    public void FinishLiftingJump()
    {
        PlayerInput.actions.Enable(); // ���͂�L���ɂ���
        PauseInput.actions.Enable(); // ���͂�L���ɂ���
        if (IgnoreNonTargetCollisions)  // ���蔲���L����
        {
            Collider SelfCollider = GetComponent<Collider>();   // �����̃R���C�_�[���擾

            foreach (Collider col in AllColliders)
            {
                // �����̃R���C�_�[�ƑS�ẴR���C�_�[�̓����蔻���L���ɂ���
                Physics.IgnoreCollision(SelfCollider, col, false);
            }
        }

        // �^�[�Q�b�g�̓������~�߂��̂�����
        TargetObject.GetComponent<ObjectGravity>().IsActive = true;

        ObjectGravityScript.IsActive = true;

        IsJumping = false;

        // �㏸���~�߂�
        Vector3 MoveDirection = MovePlayer.GetMoveDirection;    // ���݂̈ړ��������擾
        MoveDirection.y = 0;                                    // Y���̈ړ��𖳌��ɂ���
        MovePlayer.SetMoveDirection(MoveDirection.normalized);  // �ړ�������ݒ�

        // �ړ����x�����ɖ߂�
        MovePlayer.MoveSpeedMultiplier = 1f;

        //playerAnimController.PlayRandomAnimation();
    }

    private void Awake()
    {
        // �����ɃA�^�b�`����Ă���PlayerInput���擾
        PlayerInput = GetComponent<PlayerInput>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        MovePlayer = GetComponent<MovePlayer>();
        ObjectGravityScript = GetComponent<ObjectGravity>();

        // �S�I�u�W�F�N�g�̓����蔻����擾
        AllColliders = FindObjectsByType<Collider>(FindObjectsSortMode.None);
    }

    // Update is called once per frame
    private void Update()
    {
        if (IsJumping)
        {
            // �v���C���[�̍����������I�����x�𒴂����烊�t�e�B���O�W�����v�I��
            if (transform.position.y > OnStartedPlayerHeight + TerminateHeight)
            {
                FinishLiftingJump();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IgnoreNonTargetCollisions) return;

        if (!IsJumping) return;

        // �^�[�Q�b�g�ȊO�̃I�u�W�F�N�g�ɏՓ˂����ꍇ
        if (collision.gameObject != TargetObject)
        {
            // ���t�e�B���O�W�����v���I��
            FinishLiftingJump();
        }
    }
}