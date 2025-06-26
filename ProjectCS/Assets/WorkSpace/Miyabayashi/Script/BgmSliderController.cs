using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BgmSliderController : MonoBehaviour
{
    [SerializeField] private GameObject bgmVolumeButton; // スライダーに対応するボタン
    [SerializeField] private Slider bgmVolumeSlider;     // スライダー本体
    [SerializeField] private InputActionReference adjustAction;

    [SerializeField] private VolumeSetting volumeSetting; // 音量適用用（任意）

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

        // 現在選択されているUIがbgmVolumeButtonかどうかをチェック
        if (EventSystem.current.currentSelectedGameObject == bgmVolumeButton)
        {
            Vector2 input = adjustAction.action.ReadValue<Vector2>();
            float horizontal = input.x;

            if (Mathf.Abs(horizontal) > 0.5f && adjustTimer <= 0f)
            {
                float moveAmount = 0.05f;
                float direction = Mathf.Sign(horizontal);

                bgmVolumeSlider.value = Mathf.Clamp(bgmVolumeSlider.value + direction * moveAmount, 0.0f, bgmVolumeSlider.maxValue);

                // 保存＆適用
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