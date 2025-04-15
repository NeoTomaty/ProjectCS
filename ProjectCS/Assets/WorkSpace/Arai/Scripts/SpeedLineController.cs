//======================================================
// [SpeedLineController]
// �쐬�ҁF�r��C
// �ŏI�X�V���F04/13
// 
// [Log]
// 04/13�@�r��@�v���C���[�̑��x�ɉ����ďW�����̃X�P�[�����ω�����悤�Ɏ���
//======================================================

using UnityEngine;

// �W�����I�u�W�F�N�g�ɃA�^�b�`
public class SpeedLineController : MonoBehaviour
{
    [SerializeField] private PlayerSpeedManager PlayerSpeedManager;

    // �X�P�[���̐ݒ�
    [SerializeField] private float LowSpeedScale = 1.5f;      // �ᑬ��
    [SerializeField] private float HighSpeedScale = 0.8f;     // ������
    [SerializeField] private float ScaleLerpSpeed = 1.0f;     // �X�P�[���̕ω����x

    // ���x�̐ݒ�
    [SerializeField] private float PlayerLowSpeed = 120.0f;         // �ŏ����x
    [SerializeField] private float PlayerHighSpeed = 500.0f;        // �ő呬�x

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.localScale = new Vector3(LowSpeedScale, LowSpeedScale, 1.0f); // �����l��ݒ�
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerSpeedManager == null) return;

        // �v���C���[�̑��x���擾
        float PlayerSpeed = PlayerSpeedManager.GetPlayerSpeed;

        // �X�P�[���̌v�Z
        float Scale = Mathf.Lerp(LowSpeedScale, HighSpeedScale, (PlayerSpeed - PlayerLowSpeed) / (PlayerHighSpeed - PlayerLowSpeed));

        // �Ȃ߂炩�ɕω�������
        Scale = Mathf.Lerp(transform.localScale.x, Scale, Time.deltaTime * ScaleLerpSpeed);

        // �X�P�[����ݒ�
        transform.localScale = new Vector3(Scale, Scale, 1.0f);
    }
}
