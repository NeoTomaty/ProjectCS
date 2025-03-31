//======================================================
// MovePlayer�X�N���v�g�i�e�X�g�j
// �쐬�ҁF�|��
// �ŏI�X�V���F3/31
// 
// [Log]
//======================================================
using UnityEngine;

public class MovePlayerTest : MonoBehaviour
{
    public float MaxSpeed = 5.0f;  // �ő�ړ����x
    public float TurnSpeed = 300.0f; // ��]���x
    public Transform CameraTransform; // �Ǐ]�J������Transform
    public float Acceleration = 1.0f; // �����x
    public float Deceleration = 1.0f; // ����

    private float CurrentSpeed = 0.0f; // ���݂̑��x

    void Update()
    {
        // �J�������ݒ肳��Ă��Ȃ��ꍇ�͏������Ȃ�
        if (CameraTransform == null) return;

        // ���͎擾�iWASD�j
        float Horizontal = Input.GetAxisRaw("Horizontal");  // A(-1) / D(+1)
        float Vertical = Input.GetAxisRaw("Vertical");      // W(+1) / S(-1)

        // �J�����̌�������Ɉړ�����������
        Vector3 CameraForward = CameraTransform.forward;
        Vector3 CameraRight = CameraTransform.right;

        // ���������̉e�����Ȃ����iXZ ���ʂ̂ݍl���j
        CameraForward.y = 0;
        CameraRight.y = 0;
        CameraForward.Normalize();
        CameraRight.Normalize();

        // �J�����̕����Ɋ�Â����ړ��x�N�g�����v�Z
        Vector3 MoveDirection = CameraForward * Vertical + CameraRight * Horizontal;

        // �΂߈ړ����̑��x�����ɂ���
        MoveDirection.Normalize();

        // �ړ����͂�����ꍇ�A����
        if (MoveDirection.magnitude > 0.1f)
        {
            // �v���C���[�̉�]�i�ړ������Ɍ����j
            Quaternion TargetRotation = Quaternion.LookRotation(MoveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, TargetRotation, TurnSpeed * Time.deltaTime);

            // ��������
            CurrentSpeed = Mathf.Min(CurrentSpeed + Acceleration * Time.deltaTime, MaxSpeed);  // �ő呬�x��ݒ�
        }
        else
        {
            // ��������
            CurrentSpeed = Mathf.Max(CurrentSpeed - Deceleration * Time.deltaTime, 0); // �ŏ����x0
        }

        // �ړ������i�����x�𔽉f�j
        transform.position += MoveDirection * CurrentSpeed * Time.deltaTime;
    }
}
