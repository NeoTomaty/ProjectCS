//====================================================
// �X�N���v�g���FPlayerRespawn
// �쐬�ҁF�|��
// ���e�F�v���C���[��Respawn�^�O�ɐG���ƔC�ӂ̍��W�Ƀ��X�|�[�����鏈��
// �@�@�@�v���C���[�ɃA�^�b�`����
// �ŏI�X�V���F04/08
// 
// [Log]
// 04/22 �|�� �X�N���v�g�쐬
//====================================================
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
        // �C�ӂ̃��X�|�[�����W�i�C���X�y�N�^�[������ݒ�\�j
    public Vector3 respawnPosition = new Vector3(0f, 1f, 0f);

    private void OnTriggerEnter(Collider other)
    {
        // �^�O�� "Respawn" �̃I�u�W�F�N�g�ɐG�ꂽ�Ƃ�
        if (other.CompareTag("Respawn"))
        {
            // �v���C���[���w����W�Ƀ��[�v
            transform.position = respawnPosition;
        }
    }
}
