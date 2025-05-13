//====================================================
// スクリプト名：BGMManager
// 作成者：森脇
// 内容：BGM
// 最終更新日：05/07
//
// [Log]
// 05/07 森脇 スクリプト作成
//====================================================

using UnityEngine;
using UnityEngine.Audio;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    public AudioMixer audioMixer;         // Inspectorで AudioMixer を設定
    public string exposedParam = "BGMVolume"; // Exposeしたパラメータ名

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

        // AudioSource追加
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

    
    // 音量設定（0.0～1.0） → dBに変換してMixerに反映
    public void SetVolume(float volume)
    {
        float dB = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat(exposedParam, dB);
    }
}
