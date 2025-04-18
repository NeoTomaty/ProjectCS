//====================================================
// �X�N���v�g���FMiniMapCameraRotationRelative
// �쐬�ҁF�|��
// ���e�F�v���C���[�̉�]�ɉ����ă~�j�}�b�v�J������
// �@�@�@���炩�ɑ��Ή�]
// �ŏI�X�V���F04/16
// [Log]
// 04/16 �|�� �X�N���v�g�쐬
//====================================================
using UnityEngine;

public class MiniMapCameraRotationRelative : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private float rotationSmoothSpeed = 5f;

    private Vector3 initialPlayerForward;
    private float currentYRotation;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("�v���C���[�����ݒ�ł�");
            enabled = false;
            return;
        }

        // �����̑O�x�N�g�����L�^�i�n�ʕ��ʂɓ��e�j
        Vector3 forward = player.forward;
        forward.y = 0;
        initialPlayerForward = forward.normalized;

        // ����Y��]�p�x��ۑ��i���݂̃J������Y�p�x�j
        currentYRotation = transform.eulerAngles.y;
    }

    void LateUpdate()
    {
        // ���݂̃v���C���[�̑O�x�N�g�����擾���A�����ɐ���
        Vector3 currentForward = player.forward;
        currentForward.y = 0;
        currentForward.Normalize();

        // �����x�N�g�����猻�݂̑O�x�N�g���܂ł̊p�x�������擾
        float angle = Vector3.SignedAngle(initialPlayerForward, currentForward, Vector3.up);

        // ���炩�ɃJ������Y������
        currentYRotation = Mathf.LerpAngle(currentYRotation, angle, Time.deltaTime * rotationSmoothSpeed);

        // �J������]�K�p�iX���͘��Ղ�90�x�AY���������I�AZ��0�Œ�j
        transform.rotation = Quaternion.Euler(90f, currentYRotation, 0f);
    }
}