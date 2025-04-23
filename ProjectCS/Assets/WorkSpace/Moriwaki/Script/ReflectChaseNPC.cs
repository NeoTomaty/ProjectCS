//======================================================
// [ReflectingNPC]
// �쐬�ҁF�X�e
// �ŏI�X�V���F04/22
//
// [Log]
// 04/12�@�X�e NPC�̔��ˈړ�������
//======================================================

using UnityEngine;

public class ReflectingNPC : MonoBehaviour
{
    public Transform targetObject;
    public float minSpeed = 2f;
    public float maxSpeed = 10f;
    public float acceleration = 5f;
    public string wallTag = "Wall";

    private float currentSpeed;
    private Vector3 direction;

    public float GetCurrentSpeed => currentSpeed;

    private void Start()
    {
        if (!ShouldSpawn())
        {
            Destroy(gameObject);
            return;
        }

        currentSpeed = minSpeed; // �������x
        SetDirectionToTarget();
    }

    private void Update()
    {
        Accelerate();
        Move();
    }

    private void Accelerate()
    {
        currentSpeed += acceleration * Time.deltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, minSpeed, maxSpeed);
    }

    private void SetDirectionToTarget()
    {
        direction = (targetObject.position - transform.position).normalized;
    }

    private void Move()
    {
        transform.position += direction * currentSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(wallTag))
        {
            Vector3 normal = collision.contacts[0].normal;
            direction = Vector3.Reflect(direction, normal).normalized;

            // �ǂɓ��������������x�^�[�Q�b�g�����֏C��
            SetDirectionToTarget();
        }
    }

    private bool ShouldSpawn()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        return players.Length == 1;
    }
}