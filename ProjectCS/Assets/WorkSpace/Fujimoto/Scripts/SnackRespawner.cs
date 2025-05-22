//====================================================
// �X�N���v�g���FSnackRespawner
// �쐬�ҁF���{
// 
// [Log]
// 05/06 ���{�@snack�̃��X�|�[�������쐬
//====================================================

using UnityEngine;

public class SnackRespawner : MonoBehaviour
{
    [Header("�X�i�b�N�̃��X�|�[���ʒu")]
    [SerializeField] private Transform snackRespawnPoint;

    private float groundY;

    void Start()
    {
        // Ground�惊�Ⴍ�Ȃ����烊�X�|�[��
        GameObject ground = GameObject.FindGameObjectWithTag("Ground");
        if (ground != null)
        {
            groundY = ground.transform.position.y;
            Debug.Log("SnackRespawner >>groundY = " + groundY);
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
        if (snackRespawnPoint != null)
        {
            transform.position = snackRespawnPoint.position;

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            Debug.Log("Snack�����X�|�[�����܂���");
        }
        else
        {
            Debug.LogWarning("snackRespawnPoint �����ݒ�ł��I");
        }
    }
}