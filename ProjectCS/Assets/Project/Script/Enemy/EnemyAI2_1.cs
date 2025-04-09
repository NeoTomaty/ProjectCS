using UnityEngine;
using UnityEngine.AI;

public class EnemyAI2_1 : MonoBehaviour
{
    public Transform[] waypoints; // ����|�C���g
    private int currentWaypoint = 0; // ���݂̏���|�C���g
    private NavMeshAgent agent;
    private Transform player; // �v���C���[�̈ʒu
    public float chaseRadius = 0.5f; // �v���C���[��ǔ����鋗�� (���[�g���P��)
    private bool isChasing = false; // �ǔ����
    private bool isStopped = false; // ��~�t���O�i�U�����󂯂����ǂ����j

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // NavMeshAgent���擾
        player = GameObject.FindGameObjectWithTag("Player").transform; // �v���C���[���^�O�Ŏ擾
        MoveToNextWaypoint(); // �ŏ��̏���|�C���g�Ɉړ��J�n
    }

    void Update()
    {
        if (isStopped) return; // �U�����󂯂Ă���Ԃ͓����Ȃ�

        // �v���C���[�Ƃ̋������v�Z
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < chaseRadius)
        {
            // �v���C���[�����a���ɂ���ꍇ�ǔ�
            isChasing = true;
            agent.SetDestination(player.position);
        }
        else if (isChasing)
        {
            // �v���C���[�����a�O�ɏo���ꍇ����ĊJ
            isChasing = false;
            MoveToNextWaypoint();
        }

        // ����|�C���g���B���Ɏ��̃|�C���g�ֈړ�
        if (!isChasing && agent.remainingDistance < 0.5f && !agent.pathPending)
        {
            MoveToNextWaypoint();
        }
    }

    void MoveToNextWaypoint()
    {
        if (waypoints.Length == 0) return; // ����|�C���g���Ȃ��ꍇ�͏������Ȃ�
        agent.SetDestination(waypoints[currentWaypoint].position); // ���̏���|�C���g��ݒ�
        currentWaypoint = (currentWaypoint + 1) % waypoints.Length; // ���̏���|�C���g��ݒ�i���[�v�j
    }

    public void OnAttacked()
    {
        // �U�����ꂽ�Ƃ��ɒ�~
        isStopped = true;
        agent.isStopped = true; // NavMeshAgent�̓�����~
    }
}