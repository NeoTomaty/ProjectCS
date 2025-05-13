//====================================================
// �X�N���v�g���FBGMManager
// �쐬�ҁF�X�e
// ���e�FBGM
// �ŏI�X�V���F05/07
//
// [Log]
// 05/07 �X�e �X�N���v�g�쐬
//====================================================

using UnityEngine;
using UnityEngine.Audio;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    public AudioMixer audioMixer;         // Inspector�� AudioMixer ��ݒ�
    public string exposedParam = "BGMVolume"; // Expose�����p�����[�^��

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // AudioSource�ǉ�
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("BGM")[0];
    }

    public void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;

        if (audioSource.clip == clip && audioSource.isPlaying)
            return;

        audioSource.clip = clip;
        audioSource.Play();
    }

    public void StopBGM()
    {
        audioSource.Stop();
    }

    
    // ���ʐݒ�i0.0�`1.0�j �� dB�ɕϊ�����Mixer�ɔ��f
    public void SetVolume(float volume)
    {
        float dB = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat(exposedParam, dB);
    }
}
