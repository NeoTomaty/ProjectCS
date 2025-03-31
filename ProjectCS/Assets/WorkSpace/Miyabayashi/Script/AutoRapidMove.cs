//======================================================
// [�X�N���v�g��]AutoRapidMove
// �쐬�ҁF�{�ѕ��P
// �ŏI�X�V���F3/31
// 
// [Log]
//======================================================
using UnityEngine;


public class AutoRapidMove : MonoBehaviour
{
    public Transform targetPosition; // �ړ���̈ʒu
    public float speed = 10.0f;     // �ړ����x

    private void OnTriggerEnter(Collider other)
    {
        // "RapidGate" �ɐG�ꂽ��ړ��J�n
        if (other.CompareTag("RapidGate"))
        {
            StartCoroutine(MoveToPosition());
        }
    }

    private System.Collections.IEnumerator MoveToPosition()
    {
        // ���݈ʒu����ړI�n�ֈړ�
        while (Vector3.Distance(transform.position, targetPosition.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition.position,
                speed * Time.deltaTime // �t���[�����[�g�Ɉˑ����Ȃ��ړ�
            );
            yield return null;
        }
    }
}

