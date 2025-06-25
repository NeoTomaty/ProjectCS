// ButtonNavigation.cs
// 作成者: 中町雷我
// 最終更新日: 2025/05/11
// アタッチ対象: StartボタンなどのUIオブジェクト
// [Log]
// 05/11 中町 メニュー選択＆決定処理

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonNavigation : MonoBehaviour
{
    //メニューとして使用するボタンの配列
    public Button[] Buttons;

    //ボタン移動時に再生する効果音(AudioClip)
    public AudioClip NavigationSE;

    //ボタン決定時に再生する効果音(AudioClip)
    public AudioClip DecisionSE;

    //効果音を再生するためのAudioSource(スクリプト内で自動追加)
    private AudioSource audioSource;

    //現在選択中のボタンのインデックス(配列の何番目か)
    private int CurrentIndex = 0;

    //各ボタンの元のスケールを保存する配列(拡大・縮小の基準)
    private Vector3[] OriginalScales;

    //スティック操作のクールダウン時間(秒)
    private float StickCoolDown = 1.0f;

    //クールダウンタイマー(時間経過で減少)
    private float StickTimer = 0.0f;

    //スティックがニュートラルに戻ったかどうかの判定
    private bool StickReleased = true;

    void Start()
    {
        //AudioSourceをこのオブジェクトに追加(SE再生用)
        audioSource = gameObject.AddComponent<AudioSource>();

        //各ボタンの元のスケールを保存(後で元に戻すため)
        OriginalScales = new Vector3[Buttons.Length];
        for (int i = 0; i < Buttons.Length; i++)
        {
            OriginalScales[i] = Buttons[i].transform.localScale;
        }

        //最初のボタンを選択状態に設定(EventSystemでUIの選択状態を管理)
        EventSystem.current.SetSelectedGameObject(Buttons[CurrentIndex].gameObject);

        //最初のボタンを拡大表示(選択中の強調)
        EnlargeButton(Buttons[CurrentIndex]);
    }

    void Update()
    {
        //クールダウンタイマーを減少させる
        StickTimer -= Time.unscaledDeltaTime;

        //左スティックの横方向の入力値を取得
        float Horizontal = Input.GetAxis("Horizontal");

        //スティックがニュートラルに戻ったらフラグを立てる
        if (Mathf.Abs(Horizontal) < 0.2f)
        {
            StickReleased = true;
        }

        //Aボタン(Submit)またはEnterキーで決定
        if (Input.GetButtonDown("Submit") || Input.GetKeyDown(KeyCode.Return))
        {
            //決定SEを再生
            PlayDecisionSE();

            //ボタンのクリックイベントを呼び出す
            Buttons[CurrentIndex].onClick.Invoke();
        }

        //←キーまたは左スティック左で前のボタンに移動
        if ((Input.GetKeyDown(KeyCode.LeftArrow) || (Horizontal < -0.5f && StickTimer <= 0.0f && StickReleased)))
        {
            StickReleased = false;
            StickTimer = StickCoolDown;

            int nextIndex = CurrentIndex - 1;

            //Option(1)→Start(0)の移動を禁止
            if (nextIndex < 0 || (CurrentIndex == 1 && nextIndex == 0)) return;

            //移動SEを再生
            PlayNavigationSE();

            //現在のボタンの拡大を解除
            ResetButton(Buttons[CurrentIndex]);
            CurrentIndex = nextIndex;

            //新しいボタンを選択
            EventSystem.current.SetSelectedGameObject(Buttons[CurrentIndex].gameObject);
            EnlargeButton(Buttons[CurrentIndex]);
        }

        //→キーまたは左スティック右で次のボタンに移動
        if ((Input.GetKeyDown(KeyCode.RightArrow) || (Horizontal > 0.5f && StickTimer <= 0.0f && StickReleased)))
        {
            StickReleased = false;
            StickTimer = StickCoolDown;

            int nextIndex = CurrentIndex + 1;

            //Start(0)→Option(1)の移動を禁止
            if (nextIndex >= Buttons.Length || (CurrentIndex == 0 && nextIndex == 1)) return;

            //移動SEを再生
            PlayNavigationSE();

            //現在のボタンの拡大を解除
            ResetButton(Buttons[CurrentIndex]);
            CurrentIndex = nextIndex;

            //新しいボタンを選択
            EventSystem.current.SetSelectedGameObject(Buttons[CurrentIndex].gameObject);

            //新しいボタンを拡大表示
            EnlargeButton(Buttons[CurrentIndex]);
        }
    }

    //選択中のボタンを拡大表示する処理(画像も含めて拡大)
    void EnlargeButton(Button button)
    {
        int index = System.Array.IndexOf(Buttons, button);
        if (index >= 0)
        {
            button.transform.localScale = OriginalScales[index] * 1.2f;
        }
    }

    //選択解除されたボタンのスケールを元に戻す処理
    void ResetButton(Button button)
    {
        int index = System.Array.IndexOf(Buttons, button);
        if (index >= 0)
        {
            button.transform.localScale = OriginalScales[index];
        }
    }

    //ボタン移動時の効果音を再生する処理
    void PlayNavigationSE()
    {
        if (NavigationSE != null && audioSource != null)
        {
            audioSource.PlayOneShot(NavigationSE);
        }
    }

    //ボタン決定時の効果音を再生する処理
    void PlayDecisionSE()
    {
        if (DecisionSE != null && audioSource != null)
        {
            audioSource.PlayOneShot(DecisionSE);
        }
    }
}
