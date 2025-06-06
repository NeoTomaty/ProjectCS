using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AutoRapidMove : MonoBehaviour
{
    public Transform targetPosition; // �ړ���̈ʒu
    private bool IsMoving = false;   // �ړ������ǂ����̃t���O
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // "RapidGate" �ɐG��A���� "Player" �^�O���t���Ă���ꍇ�Ɉړ��J�n
        if (other.CompareTag("RapidGate") && gameObject.CompareTag("Player") && !IsMoving)
        {
            IsMoving = true;
            StartCoroutine(MoveToTarget());
        }
    }

    private System.Collections.IEnumerator MoveToTarget()
    {
        // �v���C���[�̑���𖳌���
        MovePlayer playerController = GetComponent<MovePlayer>();
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        // NavMeshAgent�ŖړI�n�ֈړ�
        agent.SetDestination(targetPosition.position);

        // �����܂őҋ@
        while (!agent.pathPending && agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }

        // �v���C���[�̑�����ēx�L����
        if (playerController != null)
        {
            playerController.enabled = true;
        }

        IsMoving = false;
    }
}
