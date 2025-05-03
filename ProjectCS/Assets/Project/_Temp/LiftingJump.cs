//======================================================
// [LiftingJump]
// �쐬�ҁF�r��C
// �ŏI�X�V���F05/03
// 
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
//======================================================
using UnityEngine;
using UnityEngine.InputSystem;

// �W�����v����ŖڕW�n�_�ɂԂ����ł��������̃e�X�g�p�̃X�N���v�g
// �v���C���[�ɃA�^�b�`
public class LiftingJump : MonoBehaviour
{
    [SerializeField] GameObject TargetObject;                   // �ڕW�n�_
    [SerializeField] private GaugeController GaugeController;   // �Q�[�W�R���g���[���[�̎Q��
    private MovePlayer MovePlayer;                              // �v���C���[�̈ړ��X�N���v�g�̎Q��
    private PlayerSpeedManager PlayerSpeedManager;              // �v���C���[�̈ړ����x���Ǘ�����X�N���v�g�̎Q��
    private ObjectGravity ObjectGravityScript;                  // �d�̓X�N���v�g�̎Q��
    private PlayerInput PlayerInput;                            // �v���C���[�̓��͂��Ǘ�����component

    [SerializeField] private float BaseJumpPower = 10f; // ��ƂȂ�W�����v�̑��x

    private float JumpPower = 0f;
    public float GetJumpPower => JumpPower; // �W�����v�͂̎擾

    private float BaseSpeed = 0f; // ���̈ړ����x

    [SerializeField] private float BaseForce = 10f; // �Փˌ�̗�
    public float GetForce => BaseForce * GaugeController.GetGaugeValue;

    [SerializeField] private float SlowMotionFactor = 0.1f; //�X���[���[�V�����̓x����
    [SerializeField] private float SlowMotionDistance = 1f; // �X���[���[�V�����ֈڍs���鋗��

    private bool IgnoreNonTargetCollisions = false;    // �^�[�Q�b�g�ȊO�Ƃ̏Փ˂𖳎����邩�ǂ����i��肭���삵�Ȃ��Ȃ�������SerializeField���珜�O�j

    private Collider[] AllColliders;    // �S�I�u�W�F�N�g�̓����蔻��

    private bool IsJumping = false;
    public bool IsLiftingPart => IsJumping; // ���t�e�B���O�W�����v�����ǂ���

    private bool IsNearTargetLast = false; // �^�[�Q�b�g�ɋ߂Â������ǂ���

    // �X���[���[�V�����̃I���I�t��؂�ւ���֐�
    private void SetSlowMotion(bool Enabled)
    {
        if (Enabled)
        {
            // �X���[���[�V�������J�n
            PlayerSpeedManager.SetOverSpeed((BaseJumpPower * JumpPower) * SlowMotionFactor); // �X���[���[�V�������̈ړ����x��ݒ�
        }
        else
        {
            // �X���[���[�V�������I��
            PlayerSpeedManager.SetOverSpeed(BaseJumpPower * JumpPower); // �ړ����x��߂�
        }
    }

    public void ResetGaugeValue()
    {
        GaugeController.SetGaugeValue(0f);
    }

    public void SetJumpPower(float Power)
    {
        JumpPower = Power;
    }

    // ���t�e�B���O�W�����v���J�n����֐�
    public void StartLiftingJump()
    {
        PlayerInput.actions.Disable(); // ���͂𖳌��ɂ���

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
        TargetObject.GetComponent<Rigidbody>().isKinematic = true;

        ObjectGravityScript.IsActive = false;
        GetComponent<Rigidbody>().Sleep();  // ������Rigidbody���X���[�v��Ԃɂ���

        IsJumping = true;

        // �v���C���[����ڕW�n�_�ւ̃x�N�g�����v�Z
        Vector3 JumpDirection = (TargetObject.transform.position - transform.position);
        MovePlayer.SetMoveDirection(JumpDirection.normalized);

        // �v���C���[������������
        BaseSpeed = PlayerSpeedManager.GetPlayerSpeed; // ���̈ړ����x��ۑ�
        PlayerSpeedManager.SetOverSpeed(BaseJumpPower * JumpPower);
    }

    // ���t�e�B���O�W�����v���~����֐�
    public void FinishLiftingJump()
    {
        PlayerInput.actions.Enable(); // ���͂�L���ɂ���

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
        TargetObject.GetComponent<Rigidbody>().isKinematic = false;

        ObjectGravityScript.IsActive = true;

        IsJumping = false;

        // �Q�[�W���~
        GaugeController.Stop();

        // �㏸���~�߂�
        Vector3 MoveDirection = MovePlayer.GetMoveDirection;    // ���݂̈ړ��������擾
        MoveDirection.y = 0;                                    // Y���̈ړ��𖳌��ɂ���
        MovePlayer.SetMoveDirection(MoveDirection.normalized);  // �ړ�������ݒ�

        // �ړ����x�����ɖ߂�
        PlayerSpeedManager.SetSpeed(BaseSpeed);
    }

    // �^�[�Q�b�g�ɋ߂Â����u�Ԃ𔻒肷��֐�
    private bool IsNearTargetEnter()
    {
        // �^�[�Q�b�g�Ƃ̋������v�Z
        float distance = Vector3.Distance(transform.position, TargetObject.transform.position);

        if (distance < SlowMotionDistance)
        {
            if (!IsNearTargetLast)
            {
                IsNearTargetLast = true;
                return true;
            }
        }
        else
        {
            IsNearTargetLast = false;
        }

        return false;
    }

    private void Awake()
    {
        // �����ɃA�^�b�`����Ă���PlayerInput���擾
        PlayerInput = GetComponent<PlayerInput>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MovePlayer = GetComponent<MovePlayer>();
        PlayerSpeedManager = GetComponent<PlayerSpeedManager>();
        ObjectGravityScript = GetComponent<ObjectGravity>();

        // �S�I�u�W�F�N�g�̓����蔻����擾
        AllColliders = FindObjectsByType<Collider>(FindObjectsSortMode.None);
    }

    // Update is called once per frame
    void Update()
    {
        if (TargetObject == null) return;

        // �㏸��
        if (IsJumping)
        {
            // ��苗���܂Ń^�[�Q�b�g�ɋ߂Â�����X���[���[�V�������J�n
            if (IsNearTargetEnter())
            {
                // �X���[���[�V�������J�n
                SetSlowMotion(true);

                // �Q�[�W��\��
                GaugeController.Play();
            }else if(GaugeController.IsFinishEnter())
            {
                // �X���[���[�V�������I��
                SetSlowMotion(false);
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
