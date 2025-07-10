// ButtonNavigation.cs
// 作成者: 中町雷我
// 最終更新日: 2025/06/26
// アタッチ対象: StartボタンなどのUIオブジェクト
// [Log]
// 05/11 中町 メニュー選択&決定処理
// 06/25 中町 コントローラーの修正&キーボード操作の修正&ボタンの拡大表示
// 06/26 中町 メニュー選択&決定SE音量調整実装
// 07/10 中町 矢印キーからWASDのキー操作に変更

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonNavigation : MonoBehaviour
{
    // メニューとして使用するボタンの配列
    public Button[] Buttons;

    // ボタン移動時に再生する効果音
    public AudioClip NavigationSE;

    // ボタン決定時に再生する効果音
    public AudioClip DecisionSE;

    // 効果音を再生するためのAudioSource
    private AudioSource audioSource;

    // 現在選択中のボタンのインデックス
    private int CurrentIndex = 0;

    // 各ボタンの元のスケールを保存する配列
    private Vector3[] OriginalScales;

    // スティック操作のクールダウン時間
    private float StickCoolDown = 0.3f;

    // クールダウンタイマー
    private float StickTimer = 0.0f;

    // スティックがニュートラルに戻ったかどうか
    private bool StickReleased = true;

    // 効果音の音量
    [Range(0.0f, 1.0f)]
    public float SEVolume = 0.5f;

    [Header("UI Blocks Input")]
    [SerializeField] private GameObject optionUI; // Option UIをインスペクターから設定

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = SEVolume;

        OriginalScales = new Vector3[Buttons.Length];
        for (int i = 0; i < Buttons.Length; i++)
        {
            OriginalScales[i] = Buttons[i].transform.localScale;
        }

        EventSystem.current.SetSelectedGameObject(Buttons[CurrentIndex].gameObject);
        EnlargeButton(Buttons[CurrentIndex]);
    }

    void Update()
    {
        // Option画面がアクティブならキー操作をすべて無効化
        if (optionUI != null && optionUI.activeSelf) return;

        StickTimer -= Time.unscaledDeltaTime;
        float Vertical = Input.GetAxisRaw("Vertical");

        //矢印キーの入力を無視
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            return;
        }

        if (Mathf.Abs(Vertical) < 0.2f)
        {
            StickReleased = true;
        }

        if (Input.GetButtonDown("Submit") || Input.GetKeyDown(KeyCode.Return))
        {
            PlayDecisionSE();
            Buttons[CurrentIndex].onClick.Invoke();
        }

        if ((Input.GetKeyDown(KeyCode.S) || (Vertical < -0.5f && StickTimer <= 0.0f && StickReleased)))
        {
            StickReleased = false;
            StickTimer = StickCoolDown;
            int nextIndex = CurrentIndex + 1;
            if (nextIndex >= Buttons.Length) return;

            PlayNavigationSE();
            ResetButton(Buttons[CurrentIndex]);
            CurrentIndex = nextIndex;
            EventSystem.current.SetSelectedGameObject(Buttons[CurrentIndex].gameObject);
            EnlargeButton(Buttons[CurrentIndex]);
        }

        if ((Input.GetKeyDown(KeyCode.W) || (Vertical > 0.5f && StickTimer <= 0.0f && StickReleased)))
        {
            StickReleased = false;
            StickTimer = StickCoolDown;
            int nextIndex = CurrentIndex - 1;
            if (nextIndex < 0) return;

            PlayNavigationSE();
            ResetButton(Buttons[CurrentIndex]);
            CurrentIndex = nextIndex;
            EventSystem.current.SetSelectedGameObject(Buttons[CurrentIndex].gameObject);
            EnlargeButton(Buttons[CurrentIndex]);
        }
    }

    void EnlargeButton(Button button)
    {
        int index = System.Array.IndexOf(Buttons, button);
        if (index >= 0)
        {
            button.transform.localScale = OriginalScales[index] * 1.2f;
        }
    }

    void ResetButton(Button button)
    {
        int index = System.Array.IndexOf(Buttons, button);
        if (index >= 0)
        {
            button.transform.localScale = OriginalScales[index];
        }
    }

    void PlayNavigationSE()
    {
        if (NavigationSE != null && audioSource != null)
        {
            audioSource.PlayOneShot(NavigationSE, SEVolume);
        }
    }

    void PlayDecisionSE()
    {
        if (DecisionSE != null && audioSource != null)
        {
            audioSource.PlayOneShot(DecisionSE, SEVolume);
        }
    }

    void ResetAllButtons()
    {
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].transform.localScale = OriginalScales[i];
        }
    }
}
