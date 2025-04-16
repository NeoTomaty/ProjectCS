//======================================================
// [BoostGimmick]
// �쐬�ҁF�r��C
// �ŏI�X�V���F04/08
// 
// [Log]
// 04/08�@�r��@�v���C���[�ƏՓ˂���ƌp�����Ԃ��ǉ������悤�Ɏ���
// 04/08�@�r��@��x��������ƃC���X�^���X�������������悤�Ɏ���
//======================================================

using UnityEngine;

public class BoostGimmick : MonoBehaviour
{
    // �����M�~�b�N�̊Ǘ��N���X
    [SerializeField] private BoostGimmickManager BoostGimmickManager;

    // �M�~�b�N�̌p������
    [SerializeField] private float GimmickDurationSeconds = 5.0f;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            // �M�~�b�N�̌��ʂ�ǉ�
            BoostGimmickManager.AddGimmickDuration(GimmickDurationSeconds);

            // �M�~�b�N�𖳌���
            gameObject.SetActive(false);
        }
    }
}
