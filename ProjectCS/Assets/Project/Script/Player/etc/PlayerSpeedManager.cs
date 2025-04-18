//====================================================
// �X�N���v�g���FPlayerSpeedManager
// �쐬�ҁF����
// ���e�F�v���C���[�̑��x�Ǘ�
// �ŏI�X�V���F04/13
// 
// [Log]
// 03/31 ���� �X�N���v�g�쐬 
// 04/01 �r�� SetAccelerationValue�֐���private����public�ɕύX
// 04/01 �|�� SetDecelerationValue�֐���ǉ�
// 04/13 ���� GetSpeedRatio�֐��ǉ�
//====================================================
using UnityEngine;

public class PlayerSpeedManager : MonoBehaviour
{
    [SerializeField]
    private float PlayerSpeed = 100.0f;    // �v���C���[�̑��x�l
    [SerializeField]
    private float MaxPlayerSpeed = 500.0f; // �v���C���[�̑��x�ő�l
    [SerializeField]
    private float MinPlayerSpeed = 120.0f; // �v���C���[�̑��x�ŏ��l

    // PlayerSpeed�擾
    public float GetPlayerSpeed => PlayerSpeed;

    // �v���C���[�̑��x�̉��Z�֐�
    public void SetAccelerationValue(float AccelerationValue)
    {
        PlayerSpeed += AccelerationValue;
        PlayerSpeed = Mathf.Clamp(PlayerSpeed, MinPlayerSpeed, MaxPlayerSpeed); // ���x�𐧌�
    }

    // �v���C���[�̑��x�̌��Z�֐�
    public void SetDecelerationValue(float DecelerationValue)
    {
        PlayerSpeed -= DecelerationValue;
        PlayerSpeed = Mathf.Clamp(PlayerSpeed, MinPlayerSpeed, MaxPlayerSpeed); // ���x�𐧌�
    }

    // �v���C���[�̑��x�������擾
    public float GetSpeedRatio()
    {
        return Mathf.Clamp01((PlayerSpeed - MinPlayerSpeed) / (MaxPlayerSpeed - MinPlayerSpeed));
    }
}
