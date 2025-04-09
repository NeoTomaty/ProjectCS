//======================================================
// [�X�N���v�g��]AutoRapidMove
// �쐬�ҁF�{�ѕ��P
// �ŏI�X�V���F4/8
// 
// [Log]
// 3/31  �{�с@�X�N���v�g�쐬
// 4/8�@�����@�v���C���[�������G���A�ɓ�������v���C���[�̑������؎󂯕t���Ȃ������ǉ�
// 4/8�@�����@IsMoving�t���O�ǉ�
// 4/8�@�����@MoveToPosition�֐��Ƀv���C���[�̑���𖳌������鏈����ǉ�
// 4/8�@�����@MoveToPosition�֐��Ɉړ�������A�v���C���[�̑�����ēx�L�������鏈���ǉ�
//======================================================
using UnityEngine;


public class AutoRapidMove : MonoBehaviour
{
    public Transform targetPosition; // �ړ���̈ʒu
    public float speed = 10.0f;     // �ړ����x
    private bool IsMoving = false; // �ړ������ǂ����̃t���O

    private void OnTriggerEnter(Collider other)
    {
        // "RapidGate" �ɐG�ꂽ��ړ��J�n
        if (other.CompareTag("RapidGate")&&!IsMoving)
        {
            IsMoving = true;
            StartCoroutine(MoveToPosition());
        }
    }

    private System.Collections.IEnumerator MoveToPosition()
    {
        // �v���C���[�̑���𖳌���
        PlayerMoveUp PlayerController = GetComponent<PlayerMoveUp>();
        if (PlayerController != null)
        {
            PlayerController.enabled = false;
        }

        // ���݈ʒu����ړI�n�ֈړ�
        while (Vector3.Distance(transform.position, targetPosition.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition.position,
                speed * Time.deltaTime 
            );
            yield return null;
        }

        // �ړ�������A�v���C���[�̑�����ēx�L����(�K�v�ɉ�����)
        if (PlayerController != null)
        {
            PlayerController.enabled = true;
        }

        IsMoving = false;
    }
}