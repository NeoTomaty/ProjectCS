using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // 移動速度

    void Update()
    {
        // 入力を取得
        float horizontal = Input.GetAxis("Horizontal"); // AとDキー
        float vertical = Input.GetAxis("Vertical"); // WとSキー

        // 移動ベクトルを計算
        Vector3 movement = new Vector3(horizontal, 0, vertical) * moveSpeed * Time.deltaTime;

        // プレイヤーを移動
        transform.Translate(movement, Space.World);
    }
}