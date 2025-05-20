//======================================================
// [Trampolin]
// �쐬�ҁF�r��C
// �ŏI�X�V���F05/20
// 
// [Log]
// 05/20�@�r��@�G�ꂽ�v���C���[���W�����v�����鏈��������
//======================================================
using UnityEngine;

public class Trampolin : MonoBehaviour
{
    [SerializeField] private float JumpPower = 100f; // �W�����v��

    private void OnCollisionEnter(Collision collision)
    {
        // �v���C���[�ɏՓ˂�����
        if (collision.gameObject.CompareTag("Player"))
        {
            // �v���C���[��Rigidbody���擾
            Rigidbody playerRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                // �W�����v�͂�������
                playerRigidbody.AddForce(Vector3.up * JumpPower, ForceMode.Impulse);
            }
        }
    }
}
