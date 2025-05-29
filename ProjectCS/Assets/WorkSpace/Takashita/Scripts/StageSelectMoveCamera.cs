//====================================================
// �X�N���v�g���FStageSelectMoveCamera
// �쐬�ҁF����
// ���e�F�X�e�[�W�Z���N�g��ʂ̃J�����̈ړ�
// �ŏI�X�V���F05/29
// 
// [Log]
// 05/29 ���� �X�N���v�g�쐬
//====================================================
using UnityEngine;

public class StageSelectMoveCamera : MonoBehaviour
{
    [SerializeField] private Transform PlayerObject;     // �Ǐ]�Ώہi�v���C���[�j
    [SerializeField] private Vector3 Offset = new Vector3(0f, 5f, -10f); // �v���C���[����̑��Έʒu

    void LateUpdate()
    {
        if (PlayerObject == null) return;

        // �v���C���[�̈ʒu�ɑ΂��ăI�t�Z�b�g�������A�J�����̈ʒu�𖈃t���[�����ڐݒ�i��]�͕ς��Ȃ��j
        transform.position = PlayerObject.position + Offset;
    }
}
