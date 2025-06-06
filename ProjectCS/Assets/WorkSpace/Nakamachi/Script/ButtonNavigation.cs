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

    //テキストの元のスケール(拡大・縮小用)
    private Vector3 OriginalScale;

    //テキストの元の色(選択色切り替え用)
    private Color OriginalColor;

    void Start()
    {
        //AudioSourceをこのオブジェクトに追加(SE再生用)
        audioSource = gameObject.AddComponent<AudioSource>();

        //最初のボタンを選択状態に設定
        EventSystem.current.SetSelectedGameObject(Buttons[CurrentIndex].gameObject);

        //選択中ボタンのテキストの元のスケールと色を保存
        OriginalScale = Buttons[CurrentIndex].GetComponentInChildren<Text>().transform.localScale;
        OriginalColor = Buttons[CurrentIndex].GetComponentInChildren<Text>().color;

        //最初のボタンのテキストを強調表示(拡大＋色変更)
        EnlargeText(Buttons[CurrentIndex]);
        ChangeTextColor(Buttons[CurrentIndex], Color.red);
    }

    void Update()
    {
        //←キーが押されたら前のボタンに移動
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //SE再生
            PlayNavigationSE();

            //現在のボタンの強調を解除
            ResetText(Buttons[CurrentIndex]);

            //インデックスを前に移動(ループ)
            CurrentIndex = (CurrentIndex - 1 + Buttons.Length) % Buttons.Length;

            //新しいボタンを選択
            EventSystem.current.SetSelectedGameObject(Buttons[CurrentIndex].gameObject);

            //新しいボタンを強調
            EnlargeText(Buttons[CurrentIndex]);
            ChangeTextColor(Buttons[CurrentIndex], Color.red);
        }

        //→キーが押されたら次のボタンに移動
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //SE再生
            PlayNavigationSE();

            //現在のボタンの強調を解除
            ResetText(Buttons[CurrentIndex]);

            //インデックスを次に移動(ループ)
            CurrentIndex = (CurrentIndex + 1) % Buttons.Length;

            //新しいボタンを選択
            EventSystem.current.SetSelectedGameObject(Buttons[CurrentIndex].gameObject);

            //新しいボタンを強調
            EnlargeText(Buttons[CurrentIndex]);
            ChangeTextColor(Buttons[CurrentIndex], Color.red);
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

    //テキストを拡大表示する処理
    void EnlargeText(Button button)
    {
        button.GetComponentInChildren<Text>().transform.localScale = OriginalScale * 1.2f;
    }

    //テキストの拡大と色を元に戻す処理
    void ResetText(Button button)
    {
        button.GetComponentInChildren<Text>().transform.localScale = OriginalScale;
        button.GetComponentInChildren<Text>().color = OriginalColor;
    }

    //テキストの色を変更する処理
    void ChangeTextColor(Button button, Color color)
    {
        button.GetComponentInChildren<Text>().color = color;
    }

    //ボタン移動時の効果音を再生する処理
    void PlayNavigationSE()
    {
        if(NavigationSE != null && audioSource != null)
        {
            audioSource.PlayOneShot(NavigationSE);
        }
    }

    //ボタン決定時の効果音を再生する処理
    void PlayDecisionSE()
    {
        if(DecisionSE != null && audioSource != null)
        {
            audioSource.PlayOneShot(DecisionSE);
        }
    }
}
