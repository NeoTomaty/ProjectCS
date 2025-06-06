using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AutoRapidMove : MonoBehaviour
{
    public Transform targetPosition; // 移動先の位置
    private bool IsMoving = false;   // 移動中かどうかのフラグ
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // "RapidGate" に触れ、かつ "Player" タグが付いている場合に移動開始
        if (other.CompareTag("RapidGate") && gameObject.CompareTag("Player") && !IsMoving)
        {
            IsMoving = true;
            StartCoroutine(MoveToTarget());
        }
    }

    private System.Collections.IEnumerator MoveToTarget()
    {
        // プレイヤーの操作を無効化
        MovePlayer playerController = GetComponent<MovePlayer>();
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        // NavMeshAgentで目的地へ移動
        agent.SetDestination(targetPosition.position);

        // 到着まで待機
        while (!agent.pathPending && agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }

        // プレイヤーの操作を再度有効化
        if (playerController != null)
        {
            playerController.enabled = true;
        }

        IsMoving = false;
    }
}
