//======================================================
// CollisionCounter �X�N���v�g
// �쐬�ҁF�{��
// �ŏI�X�V���F4/23
// 
// [Log]4/23 �{�с@�Փˉ񐔂̌v�Z������ǉ�
//                 
//======================================================

using UnityEngine;

public class CollisionCounter : MonoBehaviour
{
    // �Փˉ񐔂�ێ�����ϐ�
    private int collisionCount = 0;

    void OnCollisionEnter(Collision collision)
    {
        // �Փ˂�������̃^�O���`�F�b�N
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Wall")|| collision.gameObject.CompareTag("Snack"))
        {
            collisionCount++;
            Debug.Log("�Փˉ�: " + collisionCount);
        }
    }

    // �J�E���g�𑼂���Q�Ƃ��������p
    public int GetCollisionCount()
    {
        return collisionCount;
    }
}
