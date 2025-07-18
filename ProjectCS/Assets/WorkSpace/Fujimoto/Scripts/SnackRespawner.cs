//====================================================
// スクリプト名：SnackRespawner
// 作成者：藤本
// 
// [Log]
// 05/06 藤本　snackのリスポーン処理作成
// 05/22 荒井　groundYの設定ロジックを修正
// 06/13 高下 スナック複製時に必要なコンポーネントを参照するSetTarget関数を追加
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

        // Groundタグのオブジェクトを全て取得
        GameObject[] grounds = GameObject.FindGameObjectsWithTag("Ground");
        if (grounds.Length != 0)
        {
            // 初期値設定
            groundY = grounds[0].transform.position.y;

            // 一番低い位置の地面で設定
            foreach (GameObject ground in grounds)
            {
                if (ground.transform.position.y < groundY)
                {
                    groundY = ground.transform.position.y;
                }
            }
        }
        else
        {
            Debug.LogWarning("Groundタグが付いた地面が見つかりません。Y=0で判定します。");
            groundY = 0f;
        }
    }

    public void SetTarget(Transform SnackRespawnArea)
    {
        snackRespawnPoint = SnackRespawnArea;
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