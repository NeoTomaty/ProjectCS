using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSetting : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider volumeSlider;

    void Start()
    {
        // �����l�̓ǂݍ��݁i��F�ő剹�ʁj
        volumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        SetVolume(volumeSlider.value);
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        float dB = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20;
        audioMixer.SetFloat("Volume", dB);
        PlayerPrefs.SetFloat("MasterVolume", volume); // �ۑ�
    }
}
