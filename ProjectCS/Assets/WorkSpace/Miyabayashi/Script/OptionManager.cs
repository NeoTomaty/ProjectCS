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
    [SerializeField] private GameObject volumeButton;
    [SerializeField] private GameObject sensitivityButton;

    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider sensitivitySlider;

    [Header("Manager Reference")]
    [SerializeField] private PauseManager pauseManager;

    [SerializeField] private CameraFunction cameraFunction;
    [SerializeField] private VolumeSetting volumeSetting;

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

    private void Start()
    {
        // �X���C�_�[�ɕۑ��ςݐݒ�l�𔽉f
        volumeSlider.value = GameSettingsManager.Instance.Volume;
        sensitivitySlider.value = GameSettingsManager.Instance.Sensitivity;

        // volumeSetting ���ݒ肳��Ă���ꍇ�̂ݔ��f
        if (volumeSetting != null)
        {
            volumeSetting.SetBGMVolume(volumeSlider.value);
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

            if (isAdjustingVolume)
            {
                volumeSlider.value = Mathf.Clamp(volumeSlider.value + direction * moveAmount, 0.0f, volumeSlider.maxValue);

                GameSettingsManager.Instance.SetVolume(volumeSlider.value); // �ۑ�
                if (volumeSetting != null)
                {
                    volumeSetting.SetBGMVolume(volumeSlider.value);
                }
                Debug.Log($"Volume: {volumeSlider.value}");

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

    public float GetVolume()
    {
        return GameSettingsManager.Instance.Volume;
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
        isAdjustingVolume = false;
        isAdjustingSensitivity = false;

        // �{�^���I����߂�
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
            // �X���C�_�[������ �� �{�^���I���ɖ߂�
            isAdjustingVolume = false;
            isAdjustingSensitivity = false;

            // �߂鎞�́A���̃{�^���i��F���ʃ{�^���j���đI��
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(CurrentOptionButton);
        }
    }

    //public float GetVolume() // ���ʂ̒l
    //{
    //    return volumeSlider.value;
    //}

    //public float GetSensitivity() // ���x�̒l
    //{
    //    return sensitivitySlider.value;
    //}
}