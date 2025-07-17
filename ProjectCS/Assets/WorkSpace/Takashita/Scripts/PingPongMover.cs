using UnityEngine;

public class PingPongMover : MonoBehaviour
{
    [SerializeField] private Transform pointA; // 開始地点
    [SerializeField] private Transform pointB; // 終点
    [SerializeField] private float speed = 1f; // 移動速度

    private void Update()
    {
        float t = Mathf.PingPong(Time.time * speed, 1f); // 0～1を往復
        transform.position = Vector3.Lerp(pointA.position, pointB.position, t);
    }
}
