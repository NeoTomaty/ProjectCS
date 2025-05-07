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

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    public AudioSource audioSource;

    private void Awake()
    {
        // すでにインスタンスがあれば破棄（シングルトン）
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // シーン間で破棄しない
    }

    public void PlayBGM(AudioClip clip, float volume = 1f)
    {
        if (audioSource.clip == clip && audioSource.isPlaying) return;

        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void StopBGM()
    {
        audioSource.Stop();
    }
}