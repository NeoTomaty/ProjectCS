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

    private BlownAway_Ver3 BAV3;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        BAV3 = GetComponent<BlownAway_Ver3>();
    }

    // Snack��ł��グ��֐�
    public void Launch()
    {
        if (rb != null)
        {
            rb.AddForce(Vector3.up * launchForce, ForceMode.Impulse);
            BAV3.MoveToRandomXZInRespawnArea();
            Debug.Log("Snack��ł��グ�܂����I");
        }
    }
}
