//======================================================
// FadeManager  スクリプト
// 作成者：宮林
// 最終更新日：4/25
// 
// [Log]4/25 宮林　fade処理の管理
//          最初のシーンにfadeCanvasのPrefabを置く
//                 
//======================================================
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using System.Collections;
using System.Collections.Generic;

public class FadeManager : MonoBehaviour
{
    public CanvasGroup fadeImage;
    public float fadeDuration = 1f;

    // シーン内の PlayerInput を動的に取得して管理する
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
        fadeImage.alpha = 1f; // 最初は黒
        SceneManager.sceneLoaded += OnSceneLoaded;

        // 初期シーン内の入力取得
        RefreshPlayerInputs();
        RefreshUIInputModule();

        StartCoroutine(FadeIn());
    }

    public void FadeToScene(string sceneName)
    {
        if (!isFading)
        {
            RefreshPlayerInputs(); // 念のため
            RefreshUIInputModule();
            StartCoroutine(FadeAndLoadScene(sceneName));
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RefreshPlayerInputs();
        RefreshUIInputModule();

        StartCoroutine(FadeIn());
        isFading = false;
        isInputBlocked = false;
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

        Time.timeScale = 1f;

        yield return StartCoroutine(FadeOut());
        SceneManager.LoadScene(sceneName);
    }

    // そのシーンの PlayerInput を取得
    private void RefreshPlayerInputs()
    {
        playerInputs.Clear();
        playerInputs.AddRange(FindObjectsByType<PlayerInput>(FindObjectsSortMode.None));
    }

    // そのシーンの UIInputModule を取得
    private void RefreshUIInputModule()
    {
        uiInputModule = FindFirstObjectByType<InputSystemUIInputModule>();
    }
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

    public bool IsInputBlocked()
    {
        return isInputBlocked;
    }
}

