//======================================================
// [SpeedLineController]
// �쐬�ҁF�r��C
// �ŏI�X�V���F04/16
// 
// [Log]
// 04/13�@�r��@�v���C���[�̑��x�ɉ����ďW�����̃X�P�[�����ω�����悤�Ɏ���
// 04/16�@�r��@�e�N�X�`����UV�A�j���[�V����������
//======================================================

using UnityEngine;
using UnityEngine.UI;

// �W�����I�u�W�F�N�g�ɃA�^�b�`
public class SpeedLineController : MonoBehaviour
{
    [SerializeField] private PlayerSpeedManager PlayerSpeedManager;

    // UV�A�j���[�V�����ݒ�
    [SerializeField] private RawImage SpeedLineImage;           // �W������RawImage�R���|�[�l���g
    [SerializeField] private int TotalFrame = 6;                // ���t���[����
    [SerializeField] private float AnimationDuration = 0.1f;    // �A�j���[�V�����i�s�y�[�X
    private int CurrentFrame = 0; // ���݂̃t���[����
    private float Timer = 0.0f;

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

        if (SpeedLineImage == null) return;
        Rect rect = SpeedLineImage.uvRect;
        rect.height = 1.0f / TotalFrame;
        SpeedLineImage.uvRect = rect;
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


        // UV�A�j���[�V����
        if (SpeedLineImage == null) return;
        Timer += Time.deltaTime;
        // ��莞�Ԃ��ƂɃA�j���[�V�����i�s
        if (Timer >= AnimationDuration)
        {
            Timer = 0.0f;   // �^�C�}�[���Z�b�g
            CurrentFrame++; //�A�j���[�V�����t���[�����Z
            CurrentFrame %= TotalFrame;

            // �\���ӏ��؂芷��
            Rect rect = SpeedLineImage.uvRect;
            rect.y = rect.height * CurrentFrame;
            SpeedLineImage.uvRect = rect;
        }
    }
}
