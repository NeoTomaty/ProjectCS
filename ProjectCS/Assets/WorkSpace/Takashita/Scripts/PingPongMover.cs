using UnityEngine;

public class PingPongMover : MonoBehaviour
{
    [SerializeField] private Vector3 moveDirection = Vector3.right; // �ړ������i��F�E�����j
    [SerializeField] private float speed = 1f;                       // �ړ����x
    [SerializeField] private float travelTime = 2f;                 // �Г��ɂ����鎞��

    private Vector3 direction;  // ���ۂ̈ړ�����
    private float timer = 0f;   // �^�C�}�[
    private Vector3 startPosition; // �ړ��̋N�_�i��ɒ��S�j

    private void Start()
    {
        direction = moveDirection.normalized;
        startPosition = transform.position;
    }

    private void Update()
    {
        // �ړ�
        transform.position += direction * speed * Time.deltaTime;

        // �^�C�}�[�X�V
        timer += Time.deltaTime;

        if (timer >= travelTime)
        {
            // �������]
            direction = -direction;
            timer = 0f;
        }
    }
}
