//====================================================
// �X�N���v�g���FObjectGravity
// �쐬�ҁF����
// ���e�F�I�u�W�F�N�g�̏d�͂��Ǘ�����N���X
// �ŏI�X�V���F05/09
// 
// [Log]
// 04/21 ���� �X�N���v�g�쐬 
// 04/27 �r�� �A�N�e�B�u�t���O��ǉ� 
// 05/09 ���� �������x�̐����ǉ�
//====================================================
using UnityEngine;

public class ObjectGravity : MonoBehaviour
{
    [SerializeField]
    private Vector3 GravityScale = new Vector3(0.0f, -9.8f, 0.0f);     // �d�͂̑傫��

    [SerializeField]
    private float MaxFallSpeed = 20.0f; // �ő嗎�����x�i���̒l�j

    private Rigidbody Rb;    // �I�u�W�F�N�g��Rigidbody

    public bool IsActive = true;

    void Start()
    {
        Rb = GetComponent<Rigidbody>(); // Rigidbody���擾
    }

    void Update()
    {
        if (!IsActive) return;
        // �d��
        Rb.AddForce(GravityScale, ForceMode.Acceleration);

        // �������x�𐧌�
        if (Rb.linearVelocity.y < -MaxFallSpeed)
        {
            Vector3 clampedVelocity = Rb.linearVelocity;
            clampedVelocity.y = -MaxFallSpeed;
            Rb.linearVelocity = clampedVelocity;
        }
    }
}
