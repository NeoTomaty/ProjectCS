//ButtonNavigation.cs
//作成者:中町雷我
//最終更新日:2025/05/11
//アタッチ:Startボタンにアタッチ
//[Log]
//05/11　中町　メニュー選択&決定処理

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

    //効果音再生用のAudioSource
    private AudioSource audioSource;

    //現在選択中のボタンのインデックス
    private int CurrentIndex = 0;

    //各ボタンの元のスケールを保存する配列(拡大・縮小の基準)
    private Vector3[] OriginalScales;

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


        //最初のボタンを選択状態に設定
        EventSystem.current.SetSelectedGameObject(Buttons[CurrentIndex].gameObject);

        //最初のボタンを拡大表示(選択中の強調)
        EnlargeButton(Buttons[CurrentIndex]);
    }

    void Update()
    {
        //←キーが押されたら前のボタンに移動
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //SE再生
            PlayNavigationSE();

            //現在のボタンの拡大を解除
            ResetButton(Buttons[CurrentIndex]);

            //インデックスを前に移動(ループ)
            CurrentIndex = (CurrentIndex - 1 + Buttons.Length) % Buttons.Length;

            //新しいボタンを選択
            EventSystem.current.SetSelectedGameObject(Buttons[CurrentIndex].gameObject);

            //新しいボタンを拡大表示
            EnlargeButton(Buttons[CurrentIndex]);
        }

        //→キーが押されたら次のボタンに移動
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //SE再生
            PlayNavigationSE();

            //現在のボタンの拡大を解除
            ResetButton(Buttons[CurrentIndex]);

            //インデックスを次に移動(ループ)
            CurrentIndex = (CurrentIndex + 1) % Buttons.Length;

            //新しいボタンを選択
            EventSystem.current.SetSelectedGameObject(Buttons[CurrentIndex].gameObject);

            //新しいボタンを拡大表示
            EnlargeButton(Buttons[CurrentIndex]);
        }

        //Enterキーが押されたら現在のボタンを実行(クリック)
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //決定SEを再生
            PlayDecisionSE();

            //ボタンのクリックイベントを呼び出す
            Buttons[CurrentIndex].onClick.Invoke();
        }
    }

    //選択中のボタンを拡大表示する処理(画像も含めて拡大)
    void EnlargeButton(Button button)
    {
        button.transform.localScale = OriginalScales[CurrentIndex] * 1.2f;
    }

    //選択解除されたボタンのスケールを元に戻す処理
    void ResetButton(Button button)
    {
        //配列内のインデックスを取得して元のスケールに戻す
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
