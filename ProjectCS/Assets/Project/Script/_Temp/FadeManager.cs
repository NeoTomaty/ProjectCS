//======================================================
// FadeManager  スクリプト
// 作成者：宮林
// 最終更新日：6/26
// 
// [Log]4/25 宮林　fade処理の管理
//          最初のシーンにfadeCanvasのPrefabを置く
// 5/29 中町 フェードイン・フェードアウトSE実装
// 6/26 中町 フェードイン・フェードアウトSE音量調整実装
//======================================================
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using System.Collections;
using System.Collections.Generic;

public class FadeManager : MonoBehaviour
{
    //フェード用のCanvasGroup(黒い画像などを使ってフェード演出を行う)
    public CanvasGroup fadeImage;

    //フェードにかける時間(秒)
    public float fadeDuration = 1f;

    // シーン内の PlayerInput を動的に取得して管理する
    private List<PlayerInput> playerInputs = new List<PlayerInput>();

    //UI操作用のInputModule(UIの入力を制御する)
    public InputSystemUIInputModule uiInputModule;

    //シングルトンインスタンス(FadeManagerは1つだけ存在させる)
    private static FadeManager instance;

    //フェード中かどうかのフラグ
    public bool isFading = false;

    //入力をブロックしているかどうかのフラグ
    private bool isInputBlocked = false;

    //フェードイン・フェードアウト時に鳴らすSE(AudioClip)
    public AudioClip FadeInSE;
    public AudioClip FadeOutSE;

    //SE再生用のAudioSource
    private AudioSource audioSource;

    //フェードイン時のSE音量(0.0～1.0)
    [Range(0.0f, 1.0f)]
    public float FadeInSEVolume = 0.5f;

    //フェードアウト時のSE音量(0.0～1.0)
    [Range(0.0f, 1.0f)]
    public float FadeOutSEVolume = 0.5f;

    //初期化処理(シングルトンの設定とAudioSourceの追加)
    void Awake()
    {
        //AudioSourceを追加し、SE再生に使用
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        //すでにインスタンスが存在するときは破棄
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        //このインスタンスを唯一のものとして保持
        instance = this;

        //シーンをまたいでも破棄されないようにする
        DontDestroyOnLoad(gameObject);
    }

    //ゲーム開始時の処理
    void Start()
    {
        fadeImage.alpha = 1f; // 最初は黒

        //シーン読み込み時のイベント登録
        SceneManager.sceneLoaded += OnSceneLoaded;

        // 初期シーン内の入力取得
        RefreshPlayerInputs();

        //UI入力モジュールを取得
        RefreshUIInputModule();

        //フェードイン開始
        StartCoroutine(FadeIn());
    }

    //指定したシーンへフェード付きで遷移する
    public void FadeToScene(string sceneName)
    {
        if (!isFading)
        {
            RefreshPlayerInputs(); // 念のため
            RefreshUIInputModule();
            StartCoroutine(FadeAndLoadScene(sceneName));
        }
    }

    //シーンが読み込まれたときに呼ばれる
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RefreshPlayerInputs();
        RefreshUIInputModule();

        //新しいシーンでフェードイン
        StartCoroutine(FadeIn());
        isFading = false;
        isInputBlocked = false;
    }

    //フェードアウト処理(画面を徐々に黒くする)
    private IEnumerator FadeOut()
    {
        float t = 0f;

        //入力を無効化
        DisableInput();

        //フェードアウトSEを再生
        PlaySE(FadeOutSE,FadeOutSEVolume);

        //時間経過に応じてアルファ値を増加
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            fadeImage.alpha = Mathf.Clamp01(t / fadeDuration);
            yield return null;
        }

        //完全に黒にする
        fadeImage.alpha = 1f;
    }

    //フェードイン処理(画面を徐々に明るくする)
    private IEnumerator FadeIn()
    {
        float t = fadeDuration;

        //入力を有効化
        EnableInput();

        //フェードインSEを再生
        PlaySE(FadeInSE,FadeInSEVolume);

        //時間経過に応じてアルファ値を減少
        while (t > 0f)
        {
            t -= Time.unscaledDeltaTime;
            fadeImage.alpha = Mathf.Clamp01(t / fadeDuration);
            yield return null;
        }

        //完全に透明にする
        fadeImage.alpha = 0f;
    }

    //フェードアウト→シーン読み込みの一連の処理
    public IEnumerator FadeAndLoadScene(string sceneName)
    {
        isFading = true;
        isInputBlocked = true;

        //念のため時間を通常に戻す
        Time.timeScale = 1f;

        //フェードアウト完了を待つ
        yield return StartCoroutine(FadeOut());

        //シーンを読み込む
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

    //入力を無効化(フェード中など)
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

    //入力を有効化(フェード終了後など)
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

    //入力がブロックされているかどうかを返す
    public bool IsInputBlocked()
    {
        return isInputBlocked;
    }

    //SEを再生する共通処理(音量指定付き)
    private void PlaySE(AudioClip clip,float volume)
    {
        if(clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip,volume);
        }
    }
}