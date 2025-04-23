//====================================================
// �X�N���v�g���FObjectGravity
// �쐬�ҁF����
// ���e�F�I�u�W�F�N�g�̏d�͂��Ǘ�����N���X
// �ŏI�X�V���F04/21
// 
// [Log]
// 04/21 ���� �X�N���v�g�쐬 
// 
// 
//====================================================
using UnityEngine;

public class ObjectGravity : MonoBehaviour
{
    [SerializeField]
    private Vector3 GravityScale = new Vector3(0.0f, -9.8f, 0.0f);     // �d�͂̑傫��

    private Rigidbody Rb;    // �I�u�W�F�N�g��Rigidbody

    void Start()
    {
        Rb = GetComponent<Rigidbody>(); // Rigidbody���擾
    }

    void Update()
    {
        // �d�͕����ɉ���������
        Rb.AddForce(GravityScale, ForceMode.Acceleration);
    }
}
