using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BgmSliderController : MonoBehaviour
{
    [SerializeField] private GameObject bgmVolumeButton; // �X���C�_�[�ɑΉ�����{�^��
    [SerializeField] private Slider bgmVolumeSlider;     // �X���C�_�[�{��
    [SerializeField] private InputActionReference adjustAction;

    [SerializeField] private VolumeSetting volumeSetting; // ���ʓK�p�p�i�C�Ӂj

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

        // ���ݑI������Ă���UI��bgmVolumeButton���ǂ������`�F�b�N
        if (EventSystem.current.currentSelectedGameObject == bgmVolumeButton)
        {
            Vector2 input = adjustAction.action.ReadValue<Vector2>();
            float horizontal = input.x;

            if (Mathf.Abs(horizontal) > 0.5f && adjustTimer <= 0f)
            {
                float moveAmount = 0.05f;
                float direction = Mathf.Sign(horizontal);

                bgmVolumeSlider.value = Mathf.Clamp(bgmVolumeSlider.value + direction * moveAmount, 0.0f, bgmVolumeSlider.maxValue);

                // �ۑ����K�p
                GameSettingsManager.Instance.SetBgmVolume(bgmVolumeSlider.value);
                if (volumeSetting != null)
                {
                    volumeSetting.SetBGMVolume(bgmVolumeSlider.value);
                }

                Debug.Log($"BGM Volume: {bgmVolumeSlider.value}");

                adjustTimer = adjustCooldown;
            }
        }
    }
}