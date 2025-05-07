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

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    public AudioSource audioSource;

    private void Awake()
    {
        // ���łɃC���X�^���X������Δj���i�V���O���g���j
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // �V�[���ԂŔj�����Ȃ�
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