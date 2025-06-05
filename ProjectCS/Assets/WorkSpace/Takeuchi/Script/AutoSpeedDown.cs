//====================================================
// �X�N���v�g���FAutoSpeedDown
// �쐬�ҁF�|��
// ���e�F���Ԍo�߂ɂ��v���C���[�̌�������
// �ŏI�X�V���F06/03
// 
// [Log]
// 06/03 �|�� �X�N���v�g�쐬 
//====================================================
using UnityEngine;

public class AutoSpeedDown : MonoBehaviour
{
    [SerializeField]
    [Header("���b���ƂɌ�������̂�")]
    private float time = 1f;                // ��������܂ł̊Ԋu

    [SerializeField]
    [Header("�ǂꂭ�炢��������̂�")]
    private float decelerationAmount = 1f; // ������

    // ����
    private float timer = 0f;

    // �v���C���[�X�s�[�h�Ǘ��X�N���v�g
    private PlayerSpeedManager speedManager;

    // ������
    private void Start()
    {
        // �X�N���v�g��T��
        speedManager = GetComponent<PlayerSpeedManager>();
        if (speedManager == null)
        {
            Debug.LogError("PlayerSpeedManager��������܂���B");
        }
    }

    // �X�V
    private void Update()
    {
        if (speedManager == null) return;

        // ���t���[�����Ԍv��
        timer += Time.deltaTime;

        // �ݒ肵�����Ԃ��o������
        if (timer >= time)
        {
            // ����
            timer = 0f;
            speedManager.SetDecelerationValue(decelerationAmount);
        }
    }
}