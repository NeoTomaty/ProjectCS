//======================================================
// SpeedGaugeUI�X�N���v�g
// �쐬�ҁF�{��
// �ŏI�X�V���F5/7
// 
// [Log]5/7 �{�с@�X�s�[�h�Q�[�W�𓮂�������
//      6/6 �{�с@�Q�[�W�̑����̕�Ԃ�ǉ�
//======================================================
using UnityEngine;
using UnityEngine.UI;

public class SpeedGaugeUI : MonoBehaviour
{
    [SerializeField] private PlayerSpeedManager playerSpeedManager;
    [SerializeField] private Slider speedSlider;
    [SerializeField] private Text speedText;

    [SerializeField] private float smoothSpeed = 5f; // ��Ԃ̑����i�傫���قǑ����Ǐ]�j

    private float currentGaugeValue = 0f;

    void Start()
    {
        // �������i���݂̑��x�ɍ��킹�Ă����j
        currentGaugeValue = playerSpeedManager.GetSpeedRatio();
        speedSlider.value = currentGaugeValue;
    }

    void Update()
    {
        float targetValue = playerSpeedManager.GetSpeedRatio();

        // �X���[�Y�ɕ�Ԃ���
        currentGaugeValue = Mathf.Lerp(currentGaugeValue, targetValue, Time.deltaTime * smoothSpeed);

        // �Q�[�W�X�V
        speedSlider.value = currentGaugeValue;

        // ���l�\��
        float currentSpeed = playerSpeedManager.GetPlayerSpeed;
        speedText.text = "Speed: " + Mathf.RoundToInt(currentSpeed).ToString();
    }
}