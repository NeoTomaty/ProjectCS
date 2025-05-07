//====================================================
// �X�N���v�g���FSnackLauncher
// �쐬�ҁF���{
// 
// [Log]
// 05/07 ���{�@�J�E���g�_�E���I�����Snack��ł��グ�鏈������
//====================================================

using UnityEngine;

public class SnackLauncher : MonoBehaviour
{
    [Header("�ł��グ���")]
    [SerializeField] private float launchForce = 300f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Snack��ł��グ��֐�
    public void Launch()
    {
        if (rb != null)
        {
            rb.AddForce(Vector3.up * launchForce, ForceMode.Impulse);
            Debug.Log("Snack��ł��グ�܂����I");
        }
    }
}
