//======================================================
// ChaseCamera�X�N���v�g
// �쐬�ҁF�|��
// �ŏI�X�V���F3/31
// 
// [Log]
//======================================================
using UnityEngine;

public class ChaseCamera : MonoBehaviour
{
    public Transform Target;                        // �Ǐ]����Ώہi�v���C���[�j
    public Vector3 Offset = new Vector3(0, 2, -5);  // �J�����̈ʒu�I�t�Z�b�g
    public float SmoothSpeed = 5.0f;                // �J�����̒Ǐ]���x

    void LateUpdate()
    {
        if (Target == null) return;

        // �ڕW�̈ʒu�i�v���C���[�̌��j���v�Z
        Vector3 desiredPosition = Target.position + Target.rotation * Offset;

        // �X���[�Y�ɃJ�������ړ�
        transform.position = Vector3.Lerp(transform.position, desiredPosition, SmoothSpeed * Time.deltaTime);

        // �v���C���[�̌�����Ǐ]
        transform.LookAt(Target.position + Vector3.up * 1.5f); // �ڐ���������ɒ���
    }
}
