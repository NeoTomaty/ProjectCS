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

public class FadeManager : MonoBehaviour
{
    public CanvasGroup fadeImage;
    public float fadeDuration = 1f;
    private static FadeManager instance;
    private bool isFading = false; // フェード中かどうかのフラグ
    private bool isInputBlocked = false; // 入力をブロックするフラグ
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
        fadeImage.alpha = 1f; // 最初は黒く
        SceneManager.sceneLoaded += OnSceneLoaded;
        StartCoroutine(FadeOut());

        // InputActionMap の取得
        actionMap = new InputActionMap("UI");
        // InputActionsにバインドする必要があればここで設定
    }

    public void FadeToScene(string sceneName)
    {
        if (!isFading)
        {
            StartCoroutine(FadeAndLoadScene(sceneName));
        }
    }

    public IEnumerator FadeAndLoadScene(string sceneName)
    {
        isFading = true;
        isInputBlocked = true; // フェード中は入力を無効にする
        yield return StartCoroutine(FadeIn()); // 黒くする
        SceneManager.LoadScene(sceneName);     // 黒いままシーン切り替え
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FadeOut()); // 新しいシーンに入ったら明るくする
        isFading = false;
        isInputBlocked = false; // フェードが終わったら入力を再開
    }

    private IEnumerator FadeIn()
    {
      
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeImage.alpha = Mathf.Clamp01(t / fadeDuration);
            yield return null;
        }
        fadeImage.alpha = 1f;

        // 入力を無効化
        DisableInput();
    }

    private IEnumerator FadeOut()
    {
       
        float t = fadeDuration;
        while (t > 0f)
        {
            t -= Time.deltaTime;
            fadeImage.alpha = Mathf.Clamp01(t / fadeDuration);
            yield return null;
        }
        fadeImage.alpha = 0f;

        // 入力を有効化
        EnableInput();
    }

    // 入力を無効化するメソッド
    private void DisableInput()
    {
        if (actionMap != null)
        {
            actionMap.Disable();  // InputActionMapを無効化
        }
    }

    // 入力を有効化するメソッド
    private void EnableInput()
    {
        if (actionMap != null)
        {
            actionMap.Enable();  // InputActionMapを有効化
        }
    }

    // 入力がブロックされているかどうかを確認するためのメソッド
    public bool IsInputBlocked()
    {
        return isInputBlocked;
    }
}


