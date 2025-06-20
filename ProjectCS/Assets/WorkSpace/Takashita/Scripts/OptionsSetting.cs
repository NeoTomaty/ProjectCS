using UnityEngine;
using UnityEngine.UI;

public class OptionsSetting : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private Button BGMButton;
    [SerializeField] private Button SEButton;
    [SerializeField] private Button SensButton;
    [SerializeField] private Button BackButton;

    [Header("Slider")]
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Slider SESlider;
    [SerializeField] private Slider SensSlider;

    [Header("�Q�[���S�̂̃T�E���h���Ǘ�����I�u�W�F�N�g")]
    [SerializeField] private GameObject GameSettingPrefab;

    private void Awake()
    {
        if (!GameSettingsManager.Instance)
        {
            Instantiate(GameSettingPrefab, Vector3.zero, Quaternion.identity);
        }

        BGMSlider.value = GameSettingsManager.Instance.BgmVolume;
        SESlider.value = GameSettingsManager.Instance.SeVolume;
        SensSlider.value = GameSettingsManager.Instance.Sensitivity;
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
