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

public class FadeManager : MonoBehaviour
{
    public CanvasGroup fadeImage;
    public float fadeDuration = 1f;
    private static FadeManager instance;
    private bool isFading = false; // �t�F�[�h�����ǂ����̃t���O
    private bool isInputBlocked = false; // ���͂��u���b�N����t���O
    private InputActionMap actionMap;

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

        // InputActionMap �̎擾
        actionMap = new InputActionMap("UI");
        // InputActions�Ƀo�C���h����K�v������΂����Őݒ�
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
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            fadeImage.alpha = Mathf.Clamp01(t / fadeDuration);
            yield return null;
        }
        fadeImage.alpha = 1f;

        DisableInput();
    }

    private IEnumerator FadeIn()
    {
        float t = fadeDuration;
        while (t > 0f)
        {
            t -= Time.unscaledDeltaTime;
            fadeImage.alpha = Mathf.Clamp01(t / fadeDuration);
            yield return null;
        }
        fadeImage.alpha = 0f;

        EnableInput();
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
    private void DisableInput()
    {
        if (actionMap != null)
        {
            actionMap.Disable();  // InputActionMap�𖳌���
        }
    }

    // ���͂�L�������郁�\�b�h
    private void EnableInput()
    {
        if (actionMap != null)
        {
            actionMap.Enable();  // InputActionMap��L����
        }
    }

    // ���͂��u���b�N����Ă��邩�ǂ������m�F���邽�߂̃��\�b�h
    public bool IsInputBlocked()
    {
        return isInputBlocked;
    }
}


