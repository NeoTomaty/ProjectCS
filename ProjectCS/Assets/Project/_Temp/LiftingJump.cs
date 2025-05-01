//======================================================
// [LiftingJump]
// �쐬�ҁF�r��C
// �ŏI�X�V���F04/29
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
//======================================================
using UnityEngine;
using UnityEngine.InputSystem;

// �W�����v����ŖڕW�n�_�ɂԂ����ł��������̃e�X�g�p�̃X�N���v�g
// �v���C���[�ɃA�^�b�`
public class LiftingJump : MonoBehaviour
{
    [SerializeField] GameObject TargetObject;   // �ڕW�n�_

    private MovePlayer MovePlayer;              // �v���C���[�̈ړ��X�N���v�g�̎Q��

    private ObjectGravity ObjectGravityScript;  // �d�̓X�N���v�g�̎Q��

    [SerializeField] private GaugeController GaugeController;   // �Q�[�W�R���g���[���[�̎Q��

    [SerializeField] private float BaseJumpPower = 10f; // ��ƂȂ�W�����v�̑��x

    private float JumpPower = 0f;
    public float GetJumpPower => JumpPower; // �W�����v�͂̎擾

    //[SerializeField] private float BaseSpeed = 20f; // �Փˌ�̈ړ����x

    [SerializeField] private float BaseForce = 10f; // �Փˌ�̗�
    public float GetForce => BaseForce * GaugeController.GetGaugeValue;

    [SerializeField] private float SlowMotionFactor = 0.1f; //�X���[���[�V�����̓x����

    [SerializeField] private float SlowMotionDistance = 1f; // �X���[���[�V�����ֈڍs���鋗��

    [SerializeField] private bool IgnoreNonTargetCollisions = false;    // �^�[�Q�b�g�ȊO�Ƃ̏Փ˂𖳎����邩�ǂ���

    private Collider[] AllColliders;    // �S�I�u�W�F�N�g�̓����蔻��

    private bool IsJumping = false;

    private bool IsNearTargetLast = false; // �^�[�Q�b�g�ɋ߂Â������ǂ���

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
        if(IgnoreNonTargetCollisions)   // ���蔲���L����
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

        ObjectGravityScript.IsActive = false;

        // �v���C���[����ڕW�n�_�ւ̃x�N�g�����v�Z
        Vector3 JumpDirection = (TargetObject.transform.position - transform.position);

        GetComponent<Rigidbody>().AddForce(JumpDirection.normalized * BaseJumpPower * JumpPower, ForceMode.Impulse);

        IsJumping = true;
    }

    // ���t�e�B���O�W�����v���~����֐�
    public void FinishLiftingJump()
    {
        if (IgnoreNonTargetCollisions)  // ���蔲���L����
        {
            Collider SelfCollider = GetComponent<Collider>();   // �����̃R���C�_�[���擾

            foreach (Collider col in AllColliders)
            {
                // �����̃R���C�_�[�ƑS�ẴR���C�_�[�̓����蔻���L���ɂ���
                Physics.IgnoreCollision(SelfCollider, col, false);
            }
        }

        ObjectGravityScript.IsActive = true;

        IsJumping = false;

        // �Q�[�W���~
        GaugeController.Stop();

        // �X���[���[�V�������~
        Time.timeScale = 1.0f;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MovePlayer = GetComponent<MovePlayer>();
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
            // �v���C���[����ڕW�n�_�ւ̃x�N�g�����v�Z
            Vector3 JumpDirection = (TargetObject.transform.position - transform.position);
            // �ړ��x�N�g����ݒ�
            MovePlayer.SetMoveDirection(JumpDirection.normalized);

            // ��苗���܂Ń^�[�Q�b�g�ɋ߂Â�����X���[���[�V�������J�n
            if (IsNearTargetEnter())
            {
                // �X���[���[�V�������J�n
                Time.timeScale = SlowMotionFactor;

                // �Q�[�W��\��
                GaugeController.Play();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IgnoreNonTargetCollisions) return;

        // �^�[�Q�b�g�ȊO�̃I�u�W�F�N�g�ɏՓ˂����ꍇ
        if (collision.gameObject != TargetObject)
        {
            // ���t�e�B���O�W�����v���I��
            FinishLiftingJump();
        }
    }
}
