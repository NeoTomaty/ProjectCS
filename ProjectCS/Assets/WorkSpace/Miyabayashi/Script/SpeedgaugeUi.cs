//======================================================
// SpeedGaugeUI�X�N���v�g
// �쐬�ҁF�{��
// �ŏI�X�V���F5/7
// 
// [Log]5/7 �{�с@�X�s�[�h�Q�[�W�𓮂�������
//======================================================
using UnityEngine;
using UnityEngine.UI;

public class SpeedGaugeUI : MonoBehaviour
{
    [SerializeField] private PlayerSpeedManager playerSpeedManager;
    [SerializeField] private Slider speedSlider;
    [SerializeField] private Text speedText; // �� UI Text �������Ɋ��蓖�Ă�

    void Update()
    {
        // �Q�[�W�X�V
        speedSlider.value = playerSpeedManager.GetSpeedRatio();

        // ���l�𕶎���ŕ\��
        float currentSpeed = playerSpeedManager.GetPlayerSpeed;
        speedText.text = "Speed: " + Mathf.RoundToInt(currentSpeed).ToString();
    }
}
