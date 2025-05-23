//====================================================
// �X�N���v�g���FSnackRespawner
// �쐬�ҁF���{
// 
// [Log]
// 05/06 ���{�@snack�̃��X�|�[�������쐬
// 05/22 �r��@groundY�̐ݒ胍�W�b�N���C��
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

        // Ground�^�O�̃I�u�W�F�N�g��S�Ď擾
        GameObject[] grounds = GameObject.FindGameObjectsWithTag("Ground");
        if (grounds.Length != 0)
        {
            // �����l�ݒ�
            groundY = grounds[0].transform.position.y;

            // ��ԒႢ�ʒu�̒n�ʂŐݒ�
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