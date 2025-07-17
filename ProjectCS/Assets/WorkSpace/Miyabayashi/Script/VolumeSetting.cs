using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSetting : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider BgmVolumeSlider;
    public Slider SeVolumeSlider;

    void Start()
    {
        // 初期値の読み込み（例：最大音量）
        BgmVolumeSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1f);
        SetBGMVolume(BgmVolumeSlider.value);
        BgmVolumeSlider.onValueChanged.AddListener(SetBGMVolume);

        SeVolumeSlider.value = PlayerPrefs.GetFloat("SEVolume", 1f);
        SetSEVolume(SeVolumeSlider.value);
        SeVolumeSlider.onValueChanged.AddListener(SetSEVolume);
    }

    public void SetBGMVolume(float volume)
    {
        float dB = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20;
        audioMixer.SetFloat("BGMVolume", dB);
        PlayerPrefs.SetFloat("BGMVolume", volume); // 保存
    }
    public void SetSEVolume(float volume)
    {
        float dB = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20;
        audioMixer.SetFloat("SEVolume", dB);
        PlayerPrefs.SetFloat("SEVolume", volume); // 保存
    }
}
