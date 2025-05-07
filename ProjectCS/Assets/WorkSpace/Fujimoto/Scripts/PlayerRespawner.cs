//====================================================
// スクリプト名：PlayerRespawner
// 作成者：藤本
// 
// [Log]
// 05/06 藤本　Playerのリスポーン処理作成
//====================================================

using UnityEngine;

public class PlayerRespawner : MonoBehaviour
{
    [Header("プレイヤーのリスポーン位置")]
    [SerializeField] private Transform playerRespawnPoint;

    private float groundY;

    void Start()
    {
        // Groundよリ低くなったらリスポーン
        GameObject ground = GameObject.FindGameObjectWithTag("Ground");
        if (ground != null)
        {
            groundY = ground.transform.position.y;
        }
        else
        {
            Debug.LogWarning("Groundタグが付いた地面が見つかりません。Y=0で判定します。");
            groundY = 0f;
        }
    }

    void Update()
    {
        if (transform.position.y < groundY)
        {
            Respawn();
        }
    }

    // リスポーン処理
    private void Respawn()
    {
        if (playerRespawnPoint != null)
        {
            transform.position = playerRespawnPoint.position;

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            Debug.Log("Playerがリスポーンしました");
        }
        else
        {
            Debug.LogWarning("playerRespawnPoint が未設定です！");
        }
    }
}

