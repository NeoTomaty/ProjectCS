//======================================================
// [LiftingJump]
// �쐬�ҁF�r��C
// �ŏI�X�V���F04/26
// 
// [Log]
// 04/26�@�r��@�L�[����͂�����^�[�Q�b�g�Ɍ������ĂԂ����ł����悤�Ɏ���
//======================================================
using UnityEngine;
using UnityEngine.InputSystem;

// �W�����v����ŖڕW�n�_�ɂԂ����ł��������̃e�X�g�p�̃X�N���v�g
public class LiftingJump : MonoBehaviour
{
    [SerializeField] private float JumpImpact = 10f; // �W�����v�̃C���p�N�g

    [SerializeField] private KeyCode JumpKey = KeyCode.Space; // �W�����v�L�[

    [SerializeField] GameObject TargetObject; // �ڕW�n�_

    [SerializeField] TestMove TestMove; // TestMove�X�N���v�g�̎Q��

    [SerializeField ]private SlowMotionController SlowMotionController; // �X���[���[�V�����R���g���[���[�̎Q��

    private float DefaultSpeed = 0.0f; // �f�t�H���g�̈ړ����x

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TargetObject == null) return;

        // �W�����v�L�[�������ꂽ�Ƃ�
        if (Input.GetKeyDown(JumpKey))
        {
            // Rigidbody�̏d�͂𖳌���
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.useGravity = false;

            // �ڕW�n�_�̈ʒu���擾
            Vector3 TargetPosition = TargetObject.transform.position;

            // �v���C���[�̈ʒu���擾
            Vector3 PlayerPosition = transform.position;

            // �v���C���[����ڕW�n�_�ւ̃x�N�g�����v�Z
            Vector3 JumpDirection = (TargetPosition - PlayerPosition);

            // �ړ��x�N�g����ݒ�
            TestMove.SetMoveVector(JumpDirection);

            // �ړ����x��ݒ�
            TestMove.SetMoveSpeed(JumpImpact);

            // �X���[���[�V�������J�n
            SlowMotionController.StartSlowMotion();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Snack")
        {
            // Rigidbody�̏d�͂�L����
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.useGravity = true;

            // �ړ��x�N�g���ƈړ����x�����Z�b�g
            TestMove.SetMoveVector(Vector3.zero);
            TestMove.SetMoveSpeed(DefaultSpeed);

            // �X���[���[�V�������~
            SlowMotionController.StopSlowMotion();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Snack")
        {
            // Rigidbody�̏d�͂�L����
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.useGravity = true;

            // �ړ��x�N�g���ƈړ����x�����Z�b�g
            TestMove.SetMoveVector(Vector3.zero);
            TestMove.SetMoveSpeed(DefaultSpeed);

            // �X���[���[�V�������~
            SlowMotionController.StopSlowMotion();
        }
    }
}
