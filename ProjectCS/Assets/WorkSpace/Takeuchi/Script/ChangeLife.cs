//====================================================
// �X�N���v�g���FChangeLife
// �쐬�ҁF�|��
// ���e�F���C�t�����炷
// �ŏI�X�V���F04/08
// 
// [Log]
// 04/08 �|�� �X�N���v�g�쐬
//====================================================
using UnityEngine;
public class ChangeLife : MonoBehaviour
{
    public LifeManager LifeManager; // ���C�t�}�l�[�W���[�X�N���v�g
    public string TagName = "";     // �Q�ƃ^�O

    void OnCollisionEnter(Collision collision)
    {
        // �Q�Ƃ����^�O�̃I�u�W�F�N�g�ɐڐG�����ꍇ
        if (collision.gameObject.CompareTag(TagName))
        {
            // ���C�t�����炷
            LifeManager.DecreaseLife();
        }
    }
}
