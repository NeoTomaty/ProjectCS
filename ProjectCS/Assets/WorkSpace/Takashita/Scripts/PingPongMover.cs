using UnityEngine;

public class PingPongMover : MonoBehaviour
{
    [SerializeField] private Vector3 moveDirection = Vector3.right; // 移動方向（例：右方向）
    [SerializeField] private float speed = 1f;                       // 移動速度
    [SerializeField] private float travelTime = 2f;                 // 片道にかかる時間

    private Vector3 direction;  // 実際の移動方向
    private float timer = 0f;   // タイマー
    private Vector3 startPosition; // 移動の起点（常に中心）

    private void Start()
    {
        direction = moveDirection.normalized;
        startPosition = transform.position;
    }

    private void Update()
    {
        // 移動
        transform.position += direction * speed * Time.deltaTime;

        // タイマー更新
        timer += Time.deltaTime;

        if (timer >= travelTime)
        {
            // 方向反転
            direction = -direction;
            timer = 0f;
        }
    }
}
