//======================================================
// [ReflectingNPC]
// 作成者：森脇
// 最終更新日：04/22
//
// [Log]
// 04/12　森脇 NPCの反射移動を実装
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

        currentSpeed = minSpeed; // 初期速度
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

            // 壁に当たったらもう一度ターゲット方向へ修正
            SetDirectionToTarget();
        }
    }

    private bool ShouldSpawn()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        return players.Length == 1;
    }
}