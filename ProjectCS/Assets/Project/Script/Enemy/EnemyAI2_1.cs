//======================================================
// EnemyAI2_1�X�N���v�g
// �쐬�ҁF�{��
// �ŏI�X�V���F4/7
// 
// [Log]4/10 �{�с@�G2.1�̗v�f�ǉ�
//======================================================
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI2_1 : MonoBehaviour
{
    [Header("�ړ��ݒ�")]
    public float moveSpeed = 3.5f;
    private int direction = 1; // ��������i�i�ނ��߂邩�j

    public Transform[] waypoints; // ����|�C���g
    private int currentWaypoint = 0; // ���݂̏���|�C���g
    private NavMeshAgent agent;
    private Transform player; // �v���C���[�̈ʒu
    public float chaseRadius = 0.5f; // �v���C���[��ǔ����鋗�� (���[�g���P��)
    private bool isChasing = false; // �ǔ����
    private bool isStopped = false; // ��~�t���O�i�U�����󂯂����ǂ����j

    public int health = 100;
    public int scoreValue = 100;
    public GameObject hitEffect;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // NavMeshAgent���擾
        player = GameObject.FindGameObjectWithTag("Player").transform; // �v���C���[���^�O�Ŏ擾
        MoveToNextWaypoint(); // �ŏ��̏���|�C���g�Ɉړ��J�n
        agent.speed = moveSpeed;
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
            ReturnToClosestWaypoint(); // ��ԋ߂�����|�C���g�ɖ߂�
        }

        // ����|�C���g���B���Ɏ��̃|�C���g�ֈړ�
        if (!isChasing && agent.remainingDistance < 0.5f && !agent.pathPending)
        {
            MoveToNextWaypoint();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            Vector3 forceDirection = (transform.position - collision.transform.position).normalized;
            float forceMagnitude = collision.relativeVelocity.magnitude + 5f;
            rb.AddForce(forceDirection * forceMagnitude, ForceMode.Impulse);

            if (hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }

            // �X�R�A�����Z
            GameManager.Instance.AddScore(scoreValue);
            // 5�b��Ɏ���������
            Invoke(nameof(Die), 5f);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }


    void MoveToNextWaypoint()
    {
        if (waypoints.Length == 0) return;

        agent.SetDestination(waypoints[currentWaypoint].position);

        // ���̏���|�C���g�Ɍ����ăC���f�b�N�X���X�V
        currentWaypoint += direction;

        // �[�ɗ���������𔽓]
        if (currentWaypoint >= waypoints.Length)
        {
            currentWaypoint = waypoints.Length - 2;
            direction = -1;
        }
        else if (currentWaypoint < 0)
        {
            currentWaypoint = 1;
            direction = 1;
        }
    }

    void ReturnToClosestWaypoint()
    {
        if (waypoints.Length == 0) return; // ����|�C���g���Ȃ��ꍇ�͏������Ȃ�

        // ��ԋ߂�����|�C���g��T��
        float closestDistance = Mathf.Infinity;
        int closestWaypoint = 0;

        for (int i = 0; i < waypoints.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, waypoints[i].position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestWaypoint = i;
            }
        }

        // ��ԋ߂�����|�C���g��ݒ�
        currentWaypoint = closestWaypoint;
        agent.SetDestination(waypoints[currentWaypoint].position);
    }




    public void OnAttacked()
    {
        // �U�����ꂽ�Ƃ��ɒ�~
        isStopped = true;
        agent.isStopped = true; // NavMeshAgent�̓�����~
    }
}