using UnityEngine;

public class GameSettingsManager : MonoBehaviour
{
    public static GameSettingsManager Instance { get; private set; }

    public float BgmVolume { get; private set; } = 1.0f;
    public float SeVolume { get; private set; } = 1.0f;
    public float Sensitivity { get; private set; } = 1.0f;

    private const string BgmVolumeKey = "BGMVolume";
    private const string SeVolumeKey = "SEVolume";
    private const string SensitivityKey = "Sensitivity";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadSettings();
    }

    public void SetBgmVolume(float value)
    {
        BgmVolume = value;
        PlayerPrefs.SetFloat(BgmVolumeKey, value);
        PlayerPrefs.Save();
        Debug.Log($"[GameSettingsManager] BGM���ʂ�ۑ�: {value}");
    }

    public void SetSeVolume(float value)
    {
        SeVolume = value;
        PlayerPrefs.SetFloat(SeVolumeKey, value);
        PlayerPrefs.Save();
        Debug.Log($"[GameSettingsManager] SE���ʂ�ۑ�: {value}");
    }

    public void SetSensitivity(float value)
    {
        Sensitivity = value;
        PlayerPrefs.SetFloat(SensitivityKey, value);
        PlayerPrefs.Save();
        Debug.Log($"[GameSettingsManager] ���x��ۑ�: {value}");
    }

    public void LoadSettings()
    {
        BgmVolume = PlayerPrefs.GetFloat(BgmVolumeKey, 1.0f);
        SeVolume = PlayerPrefs.GetFloat(SeVolumeKey, 1.0f);
        Sensitivity = PlayerPrefs.GetFloat(SensitivityKey, 1.0f);
        Debug.Log($"[GameSettingsManager] �ݒ�����[�h: ���� = {BgmVolume}, ���x = {Sensitivity}");
    }
}