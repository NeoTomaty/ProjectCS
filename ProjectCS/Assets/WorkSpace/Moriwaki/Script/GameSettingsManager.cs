using UnityEngine;

public class GameSettingsManager : MonoBehaviour
{
    public static GameSettingsManager Instance { get; private set; }

    public float Volume { get; private set; } = 1.0f;
    public float Sensitivity { get; private set; } = 1.0f;

    private const string VolumeKey = "Volume";
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

    public void SetVolume(float value)
    {
        Volume = value;
        PlayerPrefs.SetFloat(VolumeKey, value);
        PlayerPrefs.Save();
        Debug.Log($"[GameSettingsManager] 音量を保存: {value}");
    }

    public void SetSensitivity(float value)
    {
        Sensitivity = value;
        PlayerPrefs.SetFloat(SensitivityKey, value);
        PlayerPrefs.Save();
        Debug.Log($"[GameSettingsManager] 感度を保存: {value}");
    }

    public void LoadSettings()
    {
        Volume = PlayerPrefs.GetFloat(VolumeKey, 1.0f);
        Sensitivity = PlayerPrefs.GetFloat(SensitivityKey, 1.0f);
        Debug.Log($"[GameSettingsManager] 設定をロード: 音量 = {Volume}, 感度 = {Sensitivity}");
    }
}