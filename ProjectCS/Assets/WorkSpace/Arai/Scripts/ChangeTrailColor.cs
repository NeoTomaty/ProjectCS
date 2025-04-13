//======================================================
// [ChangeTrailColor]
// �쐬�ҁF�r��C
// �ŏI�X�V���F04/12
// 
// [Log]
// 04/12�@�r��@�т̐F���X�N���v�g����ύX�ł���悤�Ɏ���
// 04/12�@�r��@�v���C���[�̑��x��臒l���ׂ������ɐF���ς��悤�Ɏ���
//======================================================

using UnityEngine;

// Trail Renderer�ō쐬�����т̐F��ύX����N���X
// �v���C���[��Trail Renderer�Ƌ��ɃA�^�b�`
public class ChangeTrailColor : MonoBehaviour
{

    [SerializeField] private TrailRenderer TrailRenderer;           // �g���C�������_���[�̎Q��
    [SerializeField] private PlayerSpeedManager PlayerSpeedManager; // �v���C���[�̑��x�Ǘ��N���X�̎Q��

    // �F�̐ݒ�
    [SerializeField] private Color LowSpeedColor = Color.blue;      // �ᑬ��
    [SerializeField] private Color MiddleSpeedColor = Color.yellow; // ������
    [SerializeField] private Color HighSpeedColor = Color.red;      // ������
    [SerializeField] private float AlphaValue = 1.0f;               // �A���t�@�i�����x�j�l

    // ���x��臒l
    [SerializeField] private float LowToMiddleSpeedThreshold = 200.0f;  // �ᑬ���璆��
    [SerializeField] private float MiddleToHighSpeedThreshold = 400.0f; // �������獂��

    // �O���f�[�V�����̐ݒ�
    private Gradient gradient;              // �O���f�[�V�����̎Q��
    private GradientColorKey[] colorKeys;   // �F�̃L�[�t���[��
    private GradientAlphaKey[] alphaKeys;   // �A���t�@�i�����x�j�̃L�[�t���[��

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (TrailRenderer == null) return;

        gradient = new Gradient();

        // �F�̃L�[�t���[���i���ԂƐF�j
        colorKeys = new GradientColorKey[2];
        colorKeys[0].color = LowSpeedColor; // �ᑬ���̐F
        colorKeys[0].time = 0.0f;           // ����
        colorKeys[1].color = LowSpeedColor;
        colorKeys[1].time = 1.0f;

        // �A���t�@�i�����x�j�̃L�[�t���[��
        alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0].alpha = AlphaValue;    // �s�����x
        alphaKeys[0].time = 0.0f;           // ����
        alphaKeys[1].alpha = AlphaValue;
        alphaKeys[1].time = 1.0f;

        gradient.SetKeys(colorKeys, alphaKeys);

        // �K�p
        TrailRenderer.colorGradient = gradient;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerSpeedManager == null) return;
        if (TrailRenderer == null) return;

        // �v���C���[�̑��x���擾
        float PlayerSpeed = PlayerSpeedManager.GetPlayerSpeed;

        // �v���C���[�̑��x�ɉ����ĐF��ύX
        if (PlayerSpeed < LowToMiddleSpeedThreshold)
        {
            colorKeys[0].color = LowSpeedColor;
            colorKeys[1].color = LowSpeedColor;
        }
        else if (PlayerSpeed < MiddleToHighSpeedThreshold)
        {
            colorKeys[0].color = MiddleSpeedColor;
            colorKeys[1].color = MiddleSpeedColor;
        }
        else
        {
            colorKeys[0].color = HighSpeedColor;
            colorKeys[1].color = HighSpeedColor;
        }

        gradient.SetKeys(colorKeys, alphaKeys);

        // �K�p
        TrailRenderer.colorGradient = gradient;
    }
}
