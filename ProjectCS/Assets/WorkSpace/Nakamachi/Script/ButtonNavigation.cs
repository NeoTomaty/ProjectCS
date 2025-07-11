//ButtonNavigation.cs
//作成者: 中町雷我
//最終更新日: 2025/07/11
//アタッチ対象: StartボタンなどのUIオブジェクト
//[Log]
//05/11 中町 メニュー選択&決定処理
//06/25 中町 コントローラーの修正&キーボード操作の修正&ボタンの拡大表示
//06/26 中町 メニュー選択&決定SE音量調整実装
//07/10 中町 矢印キーからWASDのキー操作に変更
//07/11 中町 ボタンの拡大表示を見やすく修正

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonNavigation : MonoBehaviour
{
    //メニューとして使用するボタンの配列
    public Button[] Buttons;

    //ボタン移動時に再生する効果音
    public AudioClip NavigationSE;

    //ボタン決定時に再生する効果音
    public AudioClip DecisionSE;

    //効果音を再生するためのAudioSource
    private AudioSource audioSource;

    //現在選択中のボタンのインデックス
    private int CurrentIndex = 0;

    //各ボタンの元のスケールを保存する配列
    private Vector3[] OriginalScales;

    //スティック操作のクールダウン時間
    private float StickCoolDown = 0.3f;

    //クールダウンタイマー
    private float StickTimer = 0.0f;

    //スティックがニュートラルに戻ったかどうか
    private bool StickReleased = true;

    //効果音の音量
    [Range(0.0f, 1.0f)]
    public float SEVolume = 0.5f;

    //オプション画面が開いているときは操作を無効化
    [Header("UI Blocks Input")]
    [SerializeField] private GameObject optionUI;

    void Start()
    {
        //AudioSourceを追加して音量を設定
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = SEVolume;

        //各ボタンの元のサイズを保存
        OriginalScales = new Vector3[Buttons.Length];
        for (int i = 0; i < Buttons.Length; i++)
        {
            OriginalScales[i] = Buttons[i].transform.localScale;
        }

        //最初のボタンを選択状態にし、拡大表示
        EventSystem.current.SetSelectedGameObject(Buttons[CurrentIndex].gameObject);
        EnlargeButton(Buttons[CurrentIndex]);
    }

    void Update()
    {
        //オプション画面が表示されているときは操作を無効化
        if (optionUI != null && optionUI.activeSelf) return;

        //クールダウンタイマーを減少
        StickTimer -= Time.unscaledDeltaTime;

        //垂直方向の入力(W/Sキーやスティック上下)
        float Vertical = Input.GetAxisRaw("Vertical");

        //矢印キーの入力は無視
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            return;
        }

        //スティックがニュートラルに戻ったか判定
        if (Mathf.Abs(Vertical) < 0.2f)
        {
            StickReleased = true;
        }

        //決定キー(EnterまたはSubmit)が押されたとき
        if (Input.GetButtonDown("Submit") || Input.GetKeyDown(KeyCode.Return))
        {
            //決定音を再生
            PlayDecisionSE();

            //現在のボタンを実行
            Buttons[CurrentIndex].onClick.Invoke();
        }

        //下方向(Sキーまたはスティック下)に入力されたとき
        if ((Input.GetKeyDown(KeyCode.S) || (Vertical < -0.5f && StickTimer <= 0.0f && StickReleased)))
        {
            StickReleased = false;
            StickTimer = StickCoolDown;
            int nextIndex = CurrentIndex + 1;

            //範囲外なら何もしない
            if (nextIndex >= Buttons.Length) return;

            //移動音を再生
            PlayNavigationSE();

            //現在のボタンを元のサイズに戻す
            ResetButton(Buttons[CurrentIndex]);

            //インデックス更新
            CurrentIndex = nextIndex;

            //新しいボタンを選択
            EventSystem.current.SetSelectedGameObject(Buttons[CurrentIndex].gameObject);

            //新しいボタンを拡大表示
            EnlargeButton(Buttons[CurrentIndex]);
        }

        //上方向(Wキーまたはスティック上)に入力されたとき
        if ((Input.GetKeyDown(KeyCode.W) || (Vertical > 0.5f && StickTimer <= 0.0f && StickReleased)))
        {
            StickReleased = false;
            StickTimer = StickCoolDown;
            int nextIndex = CurrentIndex - 1;

            //範囲外なら何もしない
            if (nextIndex < 0) return;

            PlayNavigationSE();
            ResetButton(Buttons[CurrentIndex]);
            CurrentIndex = nextIndex;
            EventSystem.current.SetSelectedGameObject(Buttons[CurrentIndex].gameObject);
            EnlargeButton(Buttons[CurrentIndex]);
        }
    }

    //選択されたボタンを拡大表示する処理
    void EnlargeButton(Button button)
    {
        int index = System.Array.IndexOf(Buttons, button);
        if (index >= 0)
        {
            button.transform.localScale = OriginalScales[index] * 1.5f;
        }
    }

    //ボタンのサイズを元に戻す処理
    void ResetButton(Button button)
    {
        int index = System.Array.IndexOf(Buttons, button);
        if (index >= 0)
        {
            button.transform.localScale = OriginalScales[index];
        }
    }

    //移動時の効果音を再生
    void PlayNavigationSE()
    {
        if (NavigationSE != null && audioSource != null)
        {
            audioSource.PlayOneShot(NavigationSE, SEVolume);
        }
    }

    //決定時の効果音を再生
    void PlayDecisionSE()
    {
        if (DecisionSE != null && audioSource != null)
        {
            audioSource.PlayOneShot(DecisionSE, SEVolume);
        }
    }

    //すべてのボタンのサイズを元に戻す
    void ResetAllButtons()
    {
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].transform.localScale = OriginalScales[i];
        }
    }
}
