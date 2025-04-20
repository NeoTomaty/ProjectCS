//======================================================
// EnemyAI2_1スクリプト
// 作成者：宮林
// 最終更新日：4/7
// 
// [Log]4/10 宮林　敵2.1の要素追加
//======================================================
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI2_1 : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 3.5f;
    private int direction = 1; // 巡回方向（進むか戻るか）

    public Transform[] waypoints; // 巡回ポイント
    private int currentWaypoint = 0; // 現在の巡回ポイント
    private NavMeshAgent agent;
    private Transform player; // プレイヤーの位置
    public float chaseRadius = 0.5f; // プレイヤーを追尾する距離 (メートル単位)
    private bool isChasing = false; // 追尾状態
    private bool isStopped = false; // 停止フラグ（攻撃を受けたかどうか）

    public int health = 100;
    public int scoreValue = 100;
    public GameObject hitEffect;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // NavMeshAgentを取得
        player = GameObject.FindGameObjectWithTag("Player").transform; // プレイヤーをタグで取得
        MoveToNextWaypoint(); // 最初の巡回ポイントに移動開始
        agent.speed = moveSpeed;
    }

    void Update()
    {
        if (isStopped) return; // 攻撃を受けている間は動かない

        // プレイヤーとの距離を計算
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < chaseRadius)
        {
            // プレイヤーが半径内にいる場合追尾
            isChasing = true;
            agent.SetDestination(player.position);
        }
        else if (isChasing)
        {
            // プレイヤーが半径外に出た場合巡回再開
            isChasing = false;
            ReturnToClosestWaypoint(); // 一番近い巡回ポイントに戻る
        }

        // 巡回ポイント到達時に次のポイントへ移動
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


    void MoveToNextWaypoint()
    {
        if (waypoints.Length == 0) return;

        agent.SetDestination(waypoints[currentWaypoint].position);

        // 次の巡回ポイントに向けてインデックスを更新
        currentWaypoint += direction;

        // 端に来たら方向を反転
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
        if (waypoints.Length == 0) return; // 巡回ポイントがない場合は処理しない

        // 一番近い巡回ポイントを探す
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

        // 一番近い巡回ポイントを設定
        currentWaypoint = closestWaypoint;
        agent.SetDestination(waypoints[currentWaypoint].position);
    }




    public void OnAttacked()
    {
        // 攻撃されたときに停止
        isStopped = true;
        agent.isStopped = true; // NavMeshAgentの動作を停止
    }
}