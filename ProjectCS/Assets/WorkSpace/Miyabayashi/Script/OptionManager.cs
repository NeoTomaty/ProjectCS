//======================================================
// OptionManager スクリプト
// 作成者：宮林
// 最終更新日：5/6
//
// [Log]5/6 宮林　オプション画面を実装
//      5/8 宮林　カメラ感度受け渡しを追加
//======================================================
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject optionUI;

    [SerializeField] private GameObject firstOptionButton;
    [SerializeField] private GameObject CurrentOptionButton;
    [SerializeField] private GameObject firstPauseButton;
    [SerializeField] private GameObject bgmVolumeButton;
    [SerializeField] private GameObject seVolumeButton;
    [SerializeField] private GameObject sensitivityButton;

    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Slider seVolumeSlider;
    
   



    [Header("Manager Reference")]
    [SerializeField] private PauseManager pauseManager;

    [SerializeField] private CameraFunction cameraFunction;
    [SerializeField] private VolumeSetting volumeSetting;

    [Header("Input")]
    [SerializeField] private InputActionReference adjustAction;

    [SerializeField] private InputActionReference cancelAction;

    private bool isAdjustingBgmVolume = false;
    private bool isAdjustingSeVolume = false;
    private bool isAdjustingSensitivity = false;

    private float adjustCooldown = 0.15f;
    private float adjustTimer = 0f;

    private void OnEnable()
    {
        adjustAction.action.Enable();
        cancelAction.action.Enable();

        cancelAction.action.performed += OnCancel;
    }

    private void OnDisable()
    {
        cancelAction.action.performed -= OnCancel;

        adjustAction.action.Disable();
        cancelAction.action.Disable();
    }

    private void Start()
    {
        // スライダーに保存済み設定値を反映
        bgmVolumeSlider.value = GameSettingsManager.Instance.BgmVolume;
        seVolumeSlider.value = GameSettingsManager.Instance.SeVolume;
        sensitivitySlider.value = GameSettingsManager.Instance.Sensitivity;

        // volumeSetting が設定されている場合のみ反映
        if (volumeSetting != null)
        {
            volumeSetting.SetBGMVolume(bgmVolumeSlider.value);
        }

        // cameraFunction が設定されている場合のみ反映
        if (cameraFunction != null)
        {
            cameraFunction.SetRatio(sensitivitySlider.value);
        }
    }

    private void Update()
    {
        adjustTimer -= Time.unscaledDeltaTime;

        Vector2 input = adjustAction.action.ReadValue<Vector2>();
        float horizontal = input.x;

        if (Mathf.Abs(horizontal) > 0.5f && adjustTimer <= 0f)
        {
            float moveAmount = 0.05f;
            float direction = Mathf.Sign(horizontal);

            if (isAdjustingBgmVolume)
            {
                bgmVolumeSlider.value = Mathf.Clamp(bgmVolumeSlider.value + direction * moveAmount, 0.0f, bgmVolumeSlider.maxValue);

                GameSettingsManager.Instance.SetBgmVolume(bgmVolumeSlider.value); // 保存
                if (volumeSetting != null)
                {
                    volumeSetting.SetBGMVolume(bgmVolumeSlider.value);
                }
                Debug.Log($"BGM Volume: {bgmVolumeSlider.value}");

            }
            else if (isAdjustingSeVolume)
            {
                seVolumeSlider.value = Mathf.Clamp(seVolumeSlider.value + direction * moveAmount, 0.0f, seVolumeSlider.maxValue);

                GameSettingsManager.Instance.SetSeVolume(seVolumeSlider.value); // 保存

                if (volumeSetting != null)
                {
                    volumeSetting.SetSEVolume(seVolumeSlider.value);
                }

                Debug.Log($"SE Volume: {seVolumeSlider.value}");
            }
            else if (isAdjustingSensitivity)
            {
                sensitivitySlider.value = Mathf.Clamp(sensitivitySlider.value + direction * moveAmount, 0.5f, sensitivitySlider.maxValue);
                GameSettingsManager.Instance.SetSensitivity(sensitivitySlider.value); // 保存
                if (cameraFunction != null)
                {
                    cameraFunction.SetRatio(sensitivitySlider.value);
                }
                Debug.Log($"sensitivitySlider: {sensitivitySlider.value}");
            }

            adjustTimer = adjustCooldown;
        }
    }

    public float GetBgmVolume()
    {
        return GameSettingsManager.Instance.BgmVolume;
    }
    public float GetSeVolume()
    {
        return GameSettingsManager.Instance.SeVolume;
    }

    public float GetSensitivity()
    {
        return GameSettingsManager.Instance.Sensitivity;
    }

    public void CloseOption()
    {
        optionUI.SetActive(false);
        pauseManager.SetPauseUIVisible(true);

        // スライダー調整を終了
        isAdjustingBgmVolume = false;
        isAdjustingSensitivity = false;

        // ボタン選択を戻す
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstPauseButton);
    }

    public void OnBgmVolumeButtonClick()
    {
        isAdjustingBgmVolume = true;
        isAdjustingSensitivity = false;
        isAdjustingSeVolume = false;

        EventSystem.current.SetSelectedGameObject(null);
    }

    public void OnSEVolumeButtonClick()
    {
        isAdjustingBgmVolume = false;
        isAdjustingSensitivity = false;
        isAdjustingSeVolume = true;

        EventSystem.current.SetSelectedGameObject(null);
    }

    public void OnSensitivityButtonClick()
    {
        isAdjustingBgmVolume = false;
        isAdjustingSensitivity = true;

        EventSystem.current.SetSelectedGameObject(null);
    }

    private void OnCancel(InputAction.CallbackContext context)
    {
        if (isAdjustingBgmVolume || isAdjustingSensitivity || isAdjustingSeVolume)
        {
            isAdjustingBgmVolume = false;
            isAdjustingSensitivity = false;
            isAdjustingSeVolume = false;

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(CurrentOptionButton);
        }
    }


}