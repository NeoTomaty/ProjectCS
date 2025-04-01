//======================================================
// [�X�N���v�g��]Enemy1
// �쐬�ҁF�X�e���S
// �ŏI�X�V���F4/01
//
// [Log]
// 3/31  �X�e�@�X�N���v�g�쐬
//======================================================

using UnityEngine;

public class EnemyPlayerTest : MonoBehaviour
{
    public float moveSpeed = 5f;
    public int maxHealth = 100;
    private int currentHealth;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(moveX, 0, moveZ) * moveSpeed;
        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player Died!");
        Respawn();
    }

    private void Respawn()
    {
        transform.position = Vector3.zero; // 仮のリスポーン地点
        currentHealth = maxHealth;
    }
}