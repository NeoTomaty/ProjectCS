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
        // �����l�̓ǂݍ��݁i��F�ő剹�ʁj
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
        PlayerPrefs.SetFloat("BGMVolume", volume); // �ۑ�
    }
    public void SetSEVolume(float volume)
    {
        float dB = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20;
        audioMixer.SetFloat("SEVolume", dB);
        PlayerPrefs.SetFloat("SEVolume", volume); // �ۑ�
    }
}
