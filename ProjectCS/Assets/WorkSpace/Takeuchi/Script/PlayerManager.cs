//====================================================
// スクリプト名：PlayerManager
// 作成者：竹内
// 内容：プレイヤーの自動前進移動（改訂版）
// 最終更新日：04/08
// 
// [Log]
// 04/22 竹内 スクリプト作成
//====================================================
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public float moveSpeed = 5f; // プレイヤーの移動速度
    public float speedIncrease = 1f; // 接触時の移動速度増加量
    public Vector3 respawnPosition = new Vector3(0f, 1f, 0f); // リスポーン位置

    private Vector3 moveDirection = Vector3.forward; // 初期の前進方向

    void Update()
    {
        // 左右キー入力の処理
        float horizontal = Input.GetAxis("Horizontal");
        if (horizontal != 0)
        {
            moveDirection = new Vector3(horizontal, 0, 1).normalized;
        }

        // プレイヤーを移動
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // 進行方向を常に向く
        if (moveDirection != Vector3.zero)
        {
            transform.forward = moveDirection;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Wall"))
        {
            moveSpeed += speedIncrease;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RespawnArea"))
        {
            transform.position = respawnPosition;
        }
    }
}