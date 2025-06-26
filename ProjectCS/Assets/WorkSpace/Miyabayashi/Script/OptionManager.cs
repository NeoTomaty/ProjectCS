//======================================================
// OptionManager �X�N���v�g
// �쐬�ҁF�{��
// �ŏI�X�V���F5/6
//
// [Log]5/6 �{�с@�I�v�V������ʂ�����
//      5/8 �{�с@�J�������x�󂯓n����ǉ�
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
        // �X���C�_�[�ɕۑ��ςݐݒ�l�𔽉f
        bgmVolumeSlider.value = GameSettingsManager.Instance.BgmVolume;
        seVolumeSlider.value = GameSettingsManager.Instance.SeVolume;
        sensitivitySlider.value = GameSettingsManager.Instance.Sensitivity;

        // volumeSetting ���ݒ肳��Ă���ꍇ�̂ݔ��f
        if (volumeSetting != null)
        {
            volumeSetting.SetBGMVolume(bgmVolumeSlider.value);
        }

        // cameraFunction ���ݒ肳��Ă���ꍇ�̂ݔ��f
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

                GameSettingsManager.Instance.SetBgmVolume(bgmVolumeSlider.value); // �ۑ�
                if (volumeSetting != null)
                {
                    volumeSetting.SetBGMVolume(bgmVolumeSlider.value);
                }
                Debug.Log($"BGM Volume: {bgmVolumeSlider.value}");

            }
            else if (isAdjustingSeVolume)
            {
                seVolumeSlider.value = Mathf.Clamp(seVolumeSlider.value + direction * moveAmount, 0.0f, seVolumeSlider.maxValue);

                GameSettingsManager.Instance.SetSeVolume(seVolumeSlider.value); // �ۑ�

                if (volumeSetting != null)
                {
                    volumeSetting.SetSEVolume(seVolumeSlider.value);
                }

                Debug.Log($"SE Volume: {seVolumeSlider.value}");
            }
            else if (isAdjustingSensitivity)
            {
                sensitivitySlider.value = Mathf.Clamp(sensitivitySlider.value + direction * moveAmount, 0.5f, sensitivitySlider.maxValue);
                GameSettingsManager.Instance.SetSensitivity(sensitivitySlider.value); // �ۑ�
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

        // �X���C�_�[�������I��
        isAdjustingBgmVolume = false;
        isAdjustingSensitivity = false;

        // �{�^���I����߂�
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