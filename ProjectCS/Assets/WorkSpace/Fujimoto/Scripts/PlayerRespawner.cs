//====================================================
// �X�N���v�g���FPlayerRespawner
// �쐬�ҁF���{
// 
// [Log]
// 05/06 ���{�@Player�̃��X�|�[�������쐬
//====================================================

using UnityEngine;

public class PlayerRespawner : MonoBehaviour
{
    [Header("�v���C���[�̃��X�|�[���ʒu")]
    [SerializeField] private Transform playerRespawnPoint;

    private float groundY;

    void Start()
    {
        // Ground�惊�Ⴍ�Ȃ����烊�X�|�[��
        GameObject ground = GameObject.FindGameObjectWithTag("Ground");
        if (ground != null)
        {
            groundY = ground.transform.position.y;
        }
        else
        {
            Debug.LogWarning("Ground�^�O���t�����n�ʂ�������܂���BY=0�Ŕ��肵�܂��B");
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

    // ���X�|�[������
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

            Debug.Log("Player�����X�|�[�����܂���");
        }
        else
        {
            Debug.LogWarning("playerRespawnPoint �����ݒ�ł��I");
        }
    }
}

