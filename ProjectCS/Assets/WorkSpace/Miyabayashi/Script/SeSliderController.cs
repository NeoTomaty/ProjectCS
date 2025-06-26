using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SeSliderController : MonoBehaviour
{
    [SerializeField] private GameObject seVolumeButton;   // SE用の選択ボタン
    [SerializeField] private Slider seVolumeSlider;       // SE音量スライダー
    [SerializeField] private InputActionReference adjustAction;
    [SerializeField] private VolumeSetting volumeSetting; // 任意（SE音量適用用）

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

        // SEボタンが選択されていなければ処理しない
        if (EventSystem.current.currentSelectedGameObject == seVolumeButton)
        {
            Vector2 input = adjustAction.action.ReadValue<Vector2>();
            float horizontal = input.x;

            if (Mathf.Abs(horizontal) > 0.5f && adjustTimer <= 0f)
            {
                float moveAmount = 0.05f;
                float direction = Mathf.Sign(horizontal);

                seVolumeSlider.value = Mathf.Clamp(seVolumeSlider.value + direction * moveAmount, 0.0f, seVolumeSlider.maxValue);

                // 設定保存と反映
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