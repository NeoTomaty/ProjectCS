//======================================================
// [BoostGimmick]
// �쐬�ҁF�r��C
// �ŏI�X�V���F05/30
// 
// [Log]
// 04/08�@�r��@�v���C���[�ƏՓ˂���ƌp�����Ԃ��ǉ������悤�Ɏ���
// 04/08�@�r��@��x��������ƃC���X�^���X�������������悤�Ɏ���
// 05/30�@�����@�p���[�A�b�vSE����
//======================================================

using UnityEngine;

public class BoostGimmick : MonoBehaviour
{
    // �����M�~�b�N�̊Ǘ��N���X
    [SerializeField] private BoostGimmickManager BoostGimmickManager;

    // �M�~�b�N�̌p������
    [SerializeField] private float GimmickDurationSeconds = 5.0f;

    //���ʉ����Đ����邽�߂�AudioSource
    [SerializeField] private AudioSource SeAudioSource;

    //���ʉ���AudioClip
    [SerializeField] private AudioClip BoostSE;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            // �M�~�b�N�̌��ʂ�ǉ�
            BoostGimmickManager.AddGimmickDuration(GimmickDurationSeconds);

            //���ʉ����Đ�
            if(SeAudioSource != null && BoostSE != null)
            {
                SeAudioSource.PlayOneShot(BoostSE);
            }

            // �M�~�b�N�𖳌���
            gameObject.SetActive(false);
        }
    }
}
