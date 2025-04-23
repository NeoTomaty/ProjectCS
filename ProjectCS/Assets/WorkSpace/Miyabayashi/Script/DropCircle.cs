//======================================================
// DropCircle�X�N���v�g
// �쐬�ҁF�{��
// �ŏI�X�V���F4/17
// 
// [Log]4/17 �{�с@�����ʒu�\��
//
//  target�ɂ�Snack������
//======================================================

using UnityEngine;

public class DropCircle : MonoBehaviour
{
    public Transform target; // �e�𗎂Ƃ��Ώۂ̃I�u�W�F�N�g
    public float minY = 0f;  // �Œ�̍����i��������X�P�[�����O�J�n�j
    public float maxY = 10f; // �ő�̍����i���̂Ƃ��ő�X�P�[���j

    public float minScale = 0.01f; // Y�����������̍ŏ��X�P�[���i�قڌ����Ȃ��j
    public float maxScale = 2.0f;  // Y���傫�����̍ő�X�P�[��

    void Update()
    {
        if (target == null) return;

        // �Ώۂ�Y���W���擾
        float height = target.position.y;

        
        float t = Mathf.InverseLerp(minY, maxY, height);

        // �X�P�[�����Ԃ��Čv�Z
        float scale = Mathf.Lerp(minScale, maxScale, t);

        
        transform.localScale = new Vector3(scale, scale, 1f);

        
        transform.position = new Vector3(target.position.x, 0.2f, target.position.z);
    }

}
