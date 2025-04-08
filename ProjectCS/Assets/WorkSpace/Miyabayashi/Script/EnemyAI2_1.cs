using UnityEngine;
using UnityEngine.AI;

public class EnemyAI2_1 : MonoBehaviour
{
    public Transform[] waypoints; // 巡回ポイント
    private int currentWaypoint = 0; // 現在の巡回ポイント
    private NavMeshAgent agent;
    private Transform player; // プレイヤーの位置
    public float chaseRadius = 0.5f; // プレイヤーを追尾する距離 (メートル単位)
    private bool isChasing = false; // 追尾状態
    private bool isStopped = false; // 停止フラグ（攻撃を受けたかどうか）

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // NavMeshAgentを取得
        player = GameObject.FindGameObjectWithTag("Player").transform; // プレイヤーをタグで取得
        MoveToNextWaypoint(); // 最初の巡回ポイントに移動開始
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
            MoveToNextWaypoint();
        }

        // 巡回ポイント到達時に次のポイントへ移動
        if (!isChasing && agent.remainingDistance < 0.5f && !agent.pathPending)
        {
            MoveToNextWaypoint();
        }
    }

    void MoveToNextWaypoint()
    {
        if (waypoints.Length == 0) return; // 巡回ポイントがない場合は処理しない
        agent.SetDestination(waypoints[currentWaypoint].position); // 次の巡回ポイントを設定
        currentWaypoint = (currentWaypoint + 1) % waypoints.Length; // 次の巡回ポイントを設定（ループ）
    }

    public void OnAttacked()
    {
        // 攻撃されたときに停止
        isStopped = true;
        agent.isStopped = true; // NavMeshAgentの動作を停止
    }
}