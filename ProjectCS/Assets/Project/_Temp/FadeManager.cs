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
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem.UI;

public class FadeManager : MonoBehaviour
{
     public CanvasGroup fadeImage;
    public float fadeDuration = 1f;
    // 複数の PlayerInput を登録できるように
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
        fadeImage.alpha = 1f; // 最初は黒く
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
        StartCoroutine(FadeIn()); // 新しいシーンに入ったら明るくする
        isFading = false;
        isInputBlocked = false; // フェードが終わったら入力を再開
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

        Time.timeScale = 1f; // ← ここで解除！

        yield return StartCoroutine(FadeOut());
        SceneManager.LoadScene(sceneName);
    }


    // 入力を無効化するメソッド

    // 入力を無効化
    private void DisableInput()
    {
        foreach (var input in playerInputs)
        {
            if (input != null)
                input.enabled = false;
        }
        if (uiInputModule != null)
            uiInputModule.enabled = false; // UI無効化
    }

    // 入力を有効化
    private void EnableInput()
    {
        foreach (var input in playerInputs)
        {
            if (input != null)
                input.enabled = true;
        }

        if (uiInputModule != null)
            uiInputModule.enabled = true; // UI有効化
    }

    // 入力がブロックされているかどうかを確認するためのメソッド
    public bool IsInputBlocked()
    {
        return isInputBlocked;
    }
}


