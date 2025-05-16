//======================================================
// FadeManager  �X�N���v�g
// �쐬�ҁF�{��
// �ŏI�X�V���F4/25
// 
// [Log]4/25 �{�с@fade�����̊Ǘ�
//          �ŏ��̃V�[����fadeCanvas��Prefab��u��
//                 
//======================================================
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem.UI;

public class FadeManager : MonoBehaviour
{
     public CanvasGroup fadeImage;
    public float fadeDuration = 1f;
    // ������ PlayerInput ��o�^�ł���悤��
    [SerializeField]
    private List<PlayerInput> playerInputs = new List<PlayerInput>();
    public InputSystemUIInputModule uiInputModule;

    private static FadeManager instance;
    private bool isFading = false;
    private bool isInputBlocked = false;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        fadeImage.alpha = 1f; // �ŏ��͍���
        SceneManager.sceneLoaded += OnSceneLoaded;
        StartCoroutine(FadeIn());

    }

    public void FadeToScene(string sceneName)
    {
        if (!isFading)
        {
            StartCoroutine(FadeAndLoadScene(sceneName));
        }
    }

   
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FadeIn()); // �V�����V�[���ɓ������疾�邭����
        isFading = false;
        isInputBlocked = false; // �t�F�[�h���I���������͂��ĊJ
    }

    private IEnumerator FadeOut()
    {
        float t = 0f;
        DisableInput();
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            fadeImage.alpha = Mathf.Clamp01(t / fadeDuration);
            yield return null;
        }
        fadeImage.alpha = 1f;

       
    }

    private IEnumerator FadeIn()
    {
        float t = fadeDuration; 
        EnableInput();

        while (t > 0f)
        {
            t -= Time.unscaledDeltaTime;
            fadeImage.alpha = Mathf.Clamp01(t / fadeDuration);
            yield return null;
        }
        fadeImage.alpha = 0f;

        
    }

    public IEnumerator FadeAndLoadScene(string sceneName)
    {
        isFading = true;
        isInputBlocked = true;

        Time.timeScale = 1f; // �� �����ŉ����I

        yield return StartCoroutine(FadeOut());
        SceneManager.LoadScene(sceneName);
    }


    // ���͂𖳌������郁�\�b�h

    // ���͂𖳌���
    private void DisableInput()
    {
        foreach (var input in playerInputs)
        {
            if (input != null)
                input.enabled = false;
        }
        if (uiInputModule != null)
            uiInputModule.enabled = false; // UI������
    }

    // ���͂�L����
    private void EnableInput()
    {
        foreach (var input in playerInputs)
        {
            if (input != null)
                input.enabled = true;
        }

        if (uiInputModule != null)
            uiInputModule.enabled = true; // UI�L����
    }

    // ���͂��u���b�N����Ă��邩�ǂ������m�F���邽�߂̃��\�b�h
    public bool IsInputBlocked()
    {
        return isInputBlocked;
    }
}


