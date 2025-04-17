//======================================================
// [�X�N���v�g��]Enemy1
// �쐬�ҁF�X�e���S
// �ŏI�X�V���F4/01
//
// [Log]
// 3/31  スクリプト作成　森脇
// 4/16  エフェクト発生位置修正　森脇
//======================================================

using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    public int health = 100;
    public int scoreValue = 100;
    public GameObject hitEffect;
    public GameObject bulletPrefab;
    public float rotationSpeed = 30f; // 回転速度（度/秒）
    public float fireRate = 2f; // 弾を撃つ間隔（秒）

    private void Start()
    {
        // 一定間隔で弾を撃つ
        InvokeRepeating(nameof(ShootBullet), fireRate, fireRate);
    }

    private void Update()
    {
        // 常に回転する（Y軸回転）
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            Vector3 forceDirection = (transform.position - collision.transform.position).normalized;
            float forceMagnitude = collision.relativeVelocity.magnitude + 5f;
            rb.AddForce(forceDirection * forceMagnitude, ForceMode.Impulse);

            if (hitEffect != null && collision.contacts.Length > 0)
            {
                // ★ぶつかった位置に出す（最初の接触点を使う）
                Vector3 hitPosition = collision.contacts[0].point;
                Instantiate(hitEffect, hitPosition, Quaternion.identity);
            }

            // スコアを加算
            GameManager.Instance.AddScore(scoreValue);
            // 5秒後に自分を消す
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

    private void ShootBullet()
    {
        if (bulletPrefab != null)
        {
            // 弾を敵の前方に発射
            GameObject bullet = Instantiate(bulletPrefab, transform.position + transform.forward, Quaternion.identity);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            bulletRb.linearVelocity = transform.forward * 10f; // 弾の速度
        }
    }
}