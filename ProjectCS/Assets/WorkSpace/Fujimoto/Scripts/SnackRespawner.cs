//====================================================
// スクリプト名：SnackRespawner
// 作成者：藤本
// 
// [Log]
// 05/06 藤本　snackのリスポーン処理作成
//====================================================

using UnityEngine;

public class SnackRespawner : MonoBehaviour
{
    [Header("スナックのリスポーン位置")]
    [SerializeField] private Transform snackRespawnPoint;

    private float groundY;

    void Start()
    {
        // Groundよリ低くなったらリスポーン
        GameObject ground = GameObject.FindGameObjectWithTag("Ground");
        if (ground != null)
        {
            groundY = ground.transform.position.y;
            Debug.Log("SnackRespawner >>groundY = " + groundY);
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
        if (snackRespawnPoint != null)
        {
            transform.position = snackRespawnPoint.position;

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            Debug.Log("Snackがリスポーンしました");
        }
        else
        {
            Debug.LogWarning("snackRespawnPoint が未設定です！");
        }
    }
}