//======================================================
// FadeManager  �X�N���v�g
// �쐬�ҁF�{��
// �ŏI�X�V���F4/25
// 
// [Log]4/25 �{�с@fade�����̊Ǘ�
//          �ŏ��̃V�[����fadeCanvas��Prefab��u��
// 5/29 ���� �t�F�[�h�C���E�t�F�[�h�A�E�gSE����
//======================================================
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using System.Collections;
using System.Collections.Generic;

public class FadeManager : MonoBehaviour
{
    //�t�F�[�h�p��CanvasGroup(�����摜�Ȃǂ��g���ăt�F�[�h���o���s��)
    public CanvasGroup fadeImage;

    //�t�F�[�h�ɂ����鎞��(�b)
    public float fadeDuration = 1f;

    // �V�[������ PlayerInput �𓮓I�Ɏ擾���ĊǗ�����
    private List<PlayerInput> playerInputs = new List<PlayerInput>();

    //UI����p��InputModule(UI�̓��͂𐧌䂷��)
    public InputSystemUIInputModule uiInputModule;

    //�V���O���g���C���X�^���X(FadeManager��1�������݂�����)
    private static FadeManager instance;

    //�t�F�[�h�����ǂ����̃t���O
    private bool isFading = false;

    //���͂��u���b�N���Ă��邩�ǂ����̃t���O
    private bool isInputBlocked = false;

    //�t�F�[�h�C���E�t�F�[�h�A�E�g���ɖ炷SE(AudioClip)
    public AudioClip FadeInSE;
    public AudioClip FadeOutSE;

    //SE�Đ��p��AudioSource
    private AudioSource audioSource;

    //����������(�V���O���g���̐ݒ��AudioSource�̒ǉ�)
    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        if (instance != null)
        {
            //���łɑ��݂��Ă���Δj��
            Destroy(gameObject);
            return;
        }

        instance = this;

        //�V�[�����܂����ł��j������Ȃ��悤�ɂ���
        DontDestroyOnLoad(gameObject);
    }

    //�Q�[���J�n���̏���
    void Start()
    {
        fadeImage.alpha = 1f; // �ŏ��͍�

        //�V�[���ǂݍ��ݎ��̃C�x���g�o�^
        SceneManager.sceneLoaded += OnSceneLoaded;

        // �����V�[�����̓��͎擾
        RefreshPlayerInputs();

        //UI���̓��W���[�����擾
        RefreshUIInputModule();

        //�t�F�[�h�C���J�n
        StartCoroutine(FadeIn());
    }

    //�w�肵���V�[���փt�F�[�h�t���őJ�ڂ���
    public void FadeToScene(string sceneName)
    {
        if (!isFading)
        {
            RefreshPlayerInputs(); // �O�̂���
            RefreshUIInputModule();
            StartCoroutine(FadeAndLoadScene(sceneName));
        }
    }

    //�V�[�����ǂݍ��܂ꂽ�Ƃ��ɌĂ΂��
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RefreshPlayerInputs();
        RefreshUIInputModule();

        //�V�����V�[���Ńt�F�[�h�C��
        StartCoroutine(FadeIn());
        isFading = false;
        isInputBlocked = false;
    }

    //�t�F�[�h�A�E�g����(��ʂ����X�ɍ�������)
    private IEnumerator FadeOut()
    {
        float t = 0f;

        //���͂𖳌���
        DisableInput();

        //�t�F�[�h�A�E�gSE���Đ�
        PlaySE(FadeOutSE);

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            fadeImage.alpha = Mathf.Clamp01(t / fadeDuration);
            yield return null;
        }
        fadeImage.alpha = 1f;
    }

    //�t�F�[�h�C������(��ʂ����X�ɖ��邭����)
    private IEnumerator FadeIn()
    {
        float t = fadeDuration;

        //���͂�L����
        EnableInput();

        //�t�F�[�h�C��SE���Đ�
        PlaySE(FadeInSE);

        while (t > 0f)
        {
            t -= Time.unscaledDeltaTime;
            fadeImage.alpha = Mathf.Clamp01(t / fadeDuration);
            yield return null;
        }
        fadeImage.alpha = 0f;
    }

    //�t�F�[�h�A�E�g���V�[���ǂݍ��݂̈�A�̏���
    public IEnumerator FadeAndLoadScene(string sceneName)
    {
        isFading = true;
        isInputBlocked = true;

        //�O�̂��ߎ��Ԃ�ʏ�ɖ߂�
        Time.timeScale = 1f;

        yield return StartCoroutine(FadeOut());

        //�V�[����ǂݍ���
        SceneManager.LoadScene(sceneName);
    }

    // ���̃V�[���� PlayerInput ���擾
    private void RefreshPlayerInputs()
    {
        playerInputs.Clear();
        playerInputs.AddRange(FindObjectsByType<PlayerInput>(FindObjectsSortMode.None));
    }

    // ���̃V�[���� UIInputModule ���擾
    private void RefreshUIInputModule()
    {
        uiInputModule = FindFirstObjectByType<InputSystemUIInputModule>();
    }

    //���͂𖳌���(�t�F�[�h���Ȃ�)
    private void DisableInput()
    {
        foreach (var input in playerInputs)
        {
            if (input != null)
                input.enabled = false;
        }

        if (uiInputModule != null)
            uiInputModule.enabled = false;
    }

    //���͂�L����(�t�F�[�h�I����Ȃ�)
    private void EnableInput()
    {
        foreach (var input in playerInputs)
        {
            if (input != null)
                input.enabled = true;
        }

        if (uiInputModule != null)
            uiInputModule.enabled = true;
    }

    //���͂��u���b�N����Ă��邩�ǂ�����Ԃ�
    public bool IsInputBlocked()
    {
        return isInputBlocked;
    }

    //SE���Đ����鋤�ʏ���
    private void PlaySE(AudioClip clip)
    {
        if(clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}