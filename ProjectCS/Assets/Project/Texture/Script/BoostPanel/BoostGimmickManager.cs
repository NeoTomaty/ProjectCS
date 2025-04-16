//======================================================
// [BoostGimmickManager]
// �쐬�ҁF�r��C
// �ŏI�X�V���F04/08
// 
// [Log]
// 04/07�@�r��@�v���C���[�Ƃ̏Փ˂ō쓮���A��莞�ԂŌ��ʂ��I������悤�ɉ��g��
// 04/07�@�r��@���ۂɃv���C���[�̑��x��ω������Ċm�F
// 04/08�@�r��@��x�g�����疳���������悤�Ɏ���
// 04/08�@�r��@�}�l�[�W���[�I�u�W�F�N�g�ɃA�^�b�`����Ǘ��N���X�֕ύX
// 04/08�@�r��@�������d�����Ĕ������Ȃ��悤�ɏC��
//======================================================

using UnityEngine;

public class BoostGimmickManager : MonoBehaviour
{
    // �v���C���[�̑��x�Ǘ��N���X
    [SerializeField] private PlayerSpeedManager PlayerSpeedManager;

    // �M�~�b�N�쓮���̉�����
    [SerializeField] private float AccelerationForGimmick = 500.0f;

    // �M�~�b�N�̌p�����ԏ��
    [SerializeField] private float GimmickDurationSecondsLimit = 5.0f;
    private float GimmickTimer = 0.0f;

    public void AddGimmickDuration(float addTime)
    {
        if (addTime <= 0.0f) return;

        // �M�~�b�N�̌��ʂ��쓮���n�߂�����������
        if (GimmickTimer <= 0.0f)
        {
            GimmickTimer = 0.0f;
            PlayerSpeedManager.SetAccelerationValue(AccelerationForGimmick);
        }

        // �M�~�b�N�̌��ʎ��Ԃ�ǉ�
        GimmickTimer += addTime;
        if (GimmickTimer > GimmickDurationSecondsLimit)
        {
            GimmickTimer = GimmickDurationSecondsLimit;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GimmickTimer <= 0.0f) return;

        GimmickTimer -= Time.deltaTime;

        // �M�~�b�N�̌��ʂ��؂ꂽ�猳�̑��x�ɖ߂�
        if (GimmickTimer <= 0.0f)
        {
            // �v���C���[������������
            PlayerSpeedManager.SetAccelerationValue(-AccelerationForGimmick);
        }
    }
}
