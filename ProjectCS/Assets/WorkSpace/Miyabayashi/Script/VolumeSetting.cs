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
        volumeSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1f);
        SetBGMVolume(volumeSlider.value);
        volumeSlider.onValueChanged.AddListener(SetBGMVolume);
    }

    public void SetBGMVolume(float volume)
    {
        float dB = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20;
        audioMixer.SetFloat("BGMVolume", dB);
        PlayerPrefs.SetFloat("BGMVolume", volume); // �ۑ�
    }
}
