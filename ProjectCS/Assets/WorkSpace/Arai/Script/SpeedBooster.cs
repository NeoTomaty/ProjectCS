//======================================================
// [SpeedBooster]
// �쐬�ҁF�r��C
// �ŏI�X�V���F04/07
// 
// [Log]
// 04/07�@�r��@�v���C���[�Ƃ̏Փ˂ō쓮���A��莞�ԂŌ��ʂ��I������悤�ɉ��g��
// 04/07�@�r��@���ۂɃv���C���[�̑��x��ω������Ċm�F
//======================================================

using UnityEngine;

public class SpeedBooster : MonoBehaviour
{
    // �v���C���[�̑��x�Ǘ��N���X
    [SerializeField] private PlayerSpeedManager PlayerSpeedManager;

    // �M�~�b�N�쓮���̉�����
    [SerializeField] private float AccelerationForGimmick = 500.0f;

    // �M�~�b�N�̌p������
    [SerializeField] private float GimmickDurationSeconds = 5.0f;
    private float GimmickTimer = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GimmickTimer <= 0.0f) return;

        GimmickTimer -= Time.deltaTime;

        // �M�~�b�N�̌��ʂ��؂ꂽ�猳�̑��x�ɖ߂�
        if (GimmickTimer <= 0.0f)
        {
            // �v���C���[������������
            PlayerSpeedManager.SetAccelerationValue(-AccelerationForGimmick);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �Փ˂����I�u�W�F�N�g�̃^�O���`�F�b�N
        if (collision.gameObject.tag == "Player")
        {
            // �v���C���[������������
            PlayerSpeedManager.SetAccelerationValue(AccelerationForGimmick);

            // �M�~�b�N�̌��ʎ��Ԃ�ݒ�
            GimmickTimer = GimmickDurationSeconds;
        }
    }
}
