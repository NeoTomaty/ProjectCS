//====================================================
// �X�N���v�g���FPlayerManager
// �쐬�ҁF�|��
// ���e�F�v���C���[�̎����O�i�ړ��i�����Łj
// �ŏI�X�V���F04/08
// 
// [Log]
// 04/22 �|�� �X�N���v�g�쐬
//====================================================
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public float moveSpeed = 5f; // �v���C���[�̈ړ����x
    public float speedIncrease = 1f; // �ڐG���̈ړ����x������
    public Vector3 respawnPosition = new Vector3(0f, 1f, 0f); // ���X�|�[���ʒu

    private Vector3 moveDirection = Vector3.forward; // �����̑O�i����

    void Update()
    {
        // ���E�L�[���͂̏���
        float horizontal = Input.GetAxis("Horizontal");
        if (horizontal != 0)
        {
            moveDirection = new Vector3(horizontal, 0, 1).normalized;
        }

        // �v���C���[���ړ�
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // �i�s��������Ɍ���
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