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
    [SerializeField] private GameObject volumeButton;
    [SerializeField] private GameObject sensitivityButton;

    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider sensitivitySlider;

    [Header("Manager Reference")]
    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private CameraFunction CameraFunction;

    [Header("Input")]
    [SerializeField] private InputActionReference adjustAction;
    [SerializeField] private InputActionReference cancelAction;

    private bool isAdjustingVolume = false;
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

    void Start()
    {
        // スライダーの初期値を1に設定
        volumeSlider.value = 1.0f;
        sensitivitySlider.value = 1.0f;

    }

    void Update()
    {
        adjustTimer -= Time.unscaledDeltaTime;

        Vector2 input = adjustAction.action.ReadValue<Vector2>(); // 修正
        float horizontal = input.x;

        if (Mathf.Abs(horizontal) > 0.5f && adjustTimer <= 0f)
        {
            float moveAmount = 0.05f;
            float direction = Mathf.Sign(horizontal);

            if (isAdjustingVolume)
            {
                volumeSlider.value = Mathf.Clamp(volumeSlider.value + direction * moveAmount, 0f, volumeSlider.maxValue);
                
            }
            else if (isAdjustingSensitivity)
            {
                sensitivitySlider.value = Mathf.Clamp(sensitivitySlider.value + direction * moveAmount, 0.5f, sensitivitySlider.maxValue);

                CameraFunction.SetRatio(sensitivitySlider.value);
            }

            adjustTimer = adjustCooldown;
        }
    }

    public void CloseOption()
    {
        optionUI.SetActive(false);
        pauseManager.SetPauseUIVisible(true);

        // スライダー調整を終了
        isAdjustingVolume = false;
        isAdjustingSensitivity = false;

        // ボタン選択を戻す
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstPauseButton);
    }

    public void OnVolumeButtonClick()
    {
        isAdjustingVolume = true;
        isAdjustingSensitivity = false;

        EventSystem.current.SetSelectedGameObject(null);
    }

    public void OnSensitivityButtonClick()
    {
        isAdjustingVolume = false;
        isAdjustingSensitivity = true;

        EventSystem.current.SetSelectedGameObject(null);
    }

    private void OnCancel(InputAction.CallbackContext context)
    {
        if (isAdjustingVolume || isAdjustingSensitivity)
        {
            // スライダー調整中 → ボタン選択に戻る
            isAdjustingVolume = false;
            isAdjustingSensitivity = false;

            // 戻る時は、元のボタン（例：音量ボタン）を再選択
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(CurrentOptionButton);
        }
    }

    public float GetVolume() // 音量の値
    {
        return volumeSlider.value;
    }

    public float GetSensitivity() // 感度の値
    {
        return sensitivitySlider.value;
    }
}
