using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SeSliderController : MonoBehaviour
{
    [SerializeField] private GameObject seVolumeButton;   // SE�p�̑I���{�^��
    [SerializeField] private Slider seVolumeSlider;       // SE���ʃX���C�_�[
    [SerializeField] private InputActionReference adjustAction;
    [SerializeField] private VolumeSetting volumeSetting; // �C�ӁiSE���ʓK�p�p�j

    private float adjustCooldown = 0.15f;
    private float adjustTimer = 0f;

    private void OnEnable()
    {
        adjustAction.action.Enable();
    }

    private void OnDisable()
    {
        adjustAction.action.Disable();
    }

    private void Update()
    {
        adjustTimer -= Time.unscaledDeltaTime;

        // SE�{�^�����I������Ă��Ȃ���Ώ������Ȃ�
        if (EventSystem.current.currentSelectedGameObject == seVolumeButton)
        {
            Vector2 input = adjustAction.action.ReadValue<Vector2>();
            float horizontal = input.x;

            if (Mathf.Abs(horizontal) > 0.5f && adjustTimer <= 0f)
            {
                float moveAmount = 0.05f;
                float direction = Mathf.Sign(horizontal);

                seVolumeSlider.value = Mathf.Clamp(seVolumeSlider.value + direction * moveAmount, 0.0f, seVolumeSlider.maxValue);

                // �ݒ�ۑ��Ɣ��f
                GameSettingsManager.Instance.SetSeVolume(seVolumeSlider.value);

                if (volumeSetting != null)
                {
                    volumeSetting.SetSEVolume(seVolumeSlider.value);
                }

                Debug.Log($"SE Volume: {seVolumeSlider.value}");

                adjustTimer = adjustCooldown;
            }
        }
    }
}