using UnityEngine;

public class PingPongMover : MonoBehaviour
{
    [SerializeField] private Transform pointA; // �J�n�n�_
    [SerializeField] private Transform pointB; // �I�_
    [SerializeField] private float speed = 1f; // �ړ����x

    private void Update()
    {
        float t = Mathf.PingPong(Time.time * speed, 1f); // 0�`1������
        transform.position = Vector3.Lerp(pointA.position, pointB.position, t);
    }
}
