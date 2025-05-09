//====================================================
// �X�N���v�g���FPlayerSpeedManager
// �쐬�ҁF����
// ���e�F�v���C���[�̑��x�Ǘ�
// �ŏI�X�V���F05/03
// 
// [Log]
// 03/31 ���� �X�N���v�g�쐬 
// 04/01 �r�� SetAccelerationValue�֐���private����public�ɕύX
// 04/01 �|�� SetDecelerationValue�֐���ǉ�
// 04/13 ���� GetSpeedRatio�֐��ǉ�
// 04/27 �r�� SetOverAccelerationValue�֐���ǉ�
// 05/03 �r�� SetSpeed�֐���SetOverSpeed�֐���ǉ�
// 05/03 �r�� SetOverAccelerationValue�֐����폜
// 05/08 ���� GetMaxSpeed��GetMinSpeed�֐��ǉ�
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

    // �v���C���[�̑��x��ݒ肷��֐�
    public void SetSpeed(float Speed)
    {
        PlayerSpeed = Speed;
        PlayerSpeed = Mathf.Clamp(PlayerSpeed, MinPlayerSpeed, MaxPlayerSpeed); // ���x�𐧌�
    }

    // �v���C���[�̑��x�Ɍ��E�𒴂����l��ݒ肷��֐�
    public void SetOverSpeed(float Speed)
    {
        PlayerSpeed = Speed;
    }

    // �v���C���[�̑��x�������擾
    public float GetSpeedRatio()
    {
        return Mathf.Clamp01((PlayerSpeed - MinPlayerSpeed) / (MaxPlayerSpeed - MinPlayerSpeed));
    }

    // �v���C���[�̍ő呬�x�擾
    public float GetMaxSpeed()
    {
        return MaxPlayerSpeed;
    }

    // �v���C���[�̍ŏ����x�擾
    public float GetMinSpeed()
    {
        return MinPlayerSpeed;
    }
}
