//====================================================
// �X�N���v�g���FPlayerSpeedManager
// �쐬�ҁF����
// ���e�F�v���C���[�̑��x�Ǘ�
// �ŏI�X�V���F03/31
// 
// [Log]
// 03/31 ���� �X�N���v�g�쐬 
// 04/01 �r�� SetAccelerationValue�֐���private����public�ɕύX
//====================================================
using UnityEngine;

public class PlayerSpeedManager : MonoBehaviour
{
    [SerializeField]
    private float PlayerSpeed = 100.0f; // �v���C���[�̑��x�l

    // PlayerSpeed�擾
    public float GetPlayerSpeed => PlayerSpeed;

    // �v���C���[�̑��x�̉��Z�֐�
    public void SetAccelerationValue(float AccelerationValue)
    {
        PlayerSpeed += AccelerationValue;
        Debug.Log("PlayerSpeed���x���Z�l�F" + AccelerationValue);
    }
}
