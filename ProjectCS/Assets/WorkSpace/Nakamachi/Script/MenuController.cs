//MenuController.cs
//作成者:中町雷我
//最終更新日:2025/05/22
//アタッチ:StartButtonにアタッチ
//[Log]
//05/22　中町　タイトルのキーボード操作とコントローラー操作処理
//06/17　中町　ボタン選択時と決定時のSE実装

using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    //スタートボタン、終了ボタン、オプションボタンをインスペクターから設定
    public Button StartButton;
    public Button ExitButton;
    public Button OptionButton;

    //ボタンを配列で管理
    private Button[] Buttons;

    //現在選択されているボタンのインデックス
    private int SelectedIndex = 0;

    //入力の間隔(連続入力を防ぐためのディレイ)
    public float InputDelay = 0.3f;
    private float InputTimer = 0.0f;

    //SE用のAudioClipとAudioSourceを追加
    public AudioClip SelectSE;
    public AudioClip SubmitSE;
    private AudioSource audioSource;

    void Start()
    {
        //ボタンを配列に格納
        Buttons = new Button[] 
        {
            StartButton, ExitButton, OptionButton
        };

        //AudioSourceを取得
        audioSource = GetComponent<AudioSource>();

        if(audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        //初期選択状態を更新
        UpdateButtonSelection();
    }

    void Update()
    {
        //入力タイマーを減少させる
        InputTimer -= Time.deltaTime;

        //水平方向の入力を取得(キーボードの左右キーやゲームパッドのスティック)
        float HorizontalInput = Input.GetAxis("Horizontal");

        //入力ディレイが経過している場合のみ処理
        if (InputTimer <= 0f)
        {
            int PreviousIndex = SelectedIndex;
            int NextIndex = SelectedIndex;

            //右入力:次のボタンへ移動
            if (HorizontalInput > 0.5f)
            {
                if (SelectedIndex == 0)
                {
                    NextIndex = 1;
                }
                else if (SelectedIndex == 1)
                {
                    NextIndex = 2;
                }
            }
            //左入力:前のボタンへ移動
            else if (HorizontalInput < -0.5f)
            {
                if (SelectedIndex == 2)
                {
                    NextIndex = 1;
                }
                else if (SelectedIndex == 1)
                {
                    NextIndex = 0;
                }
            }

            //選択が変わったとき、見た目を更新
            if (NextIndex != SelectedIndex)
            {
                //前のボタンのスケールをリセット
                ResetButtonScale(SelectedIndex);

                //新しい選択に更新
                SelectedIndex = NextIndex;

                //入力ディレイをリセット
                InputTimer = InputDelay;

                //ボタンの見た目を更新
                UpdateButtonSelection();

                //選択時のSEを再生
                PlaySE(SelectSE);
            }
        }

        //決定ボタンが押されたら、現在選択中のボタンのクリックイベントを実行
        if (Input.GetButtonDown("Submit"))
        {
            Buttons[SelectedIndex].onClick.Invoke();

            //決定時のSE再生
            PlaySE(SubmitSE);
        }
    }

    //ボタンのスケールを元に戻す(選択解除時)
    void ResetButtonScale(int index)
    {
        Text ButtonText = Buttons[index].GetComponentInChildren<Text>();

        if (ButtonText != null)
        {
            ButtonText.transform.localScale = Vector3.one;
        }
    }

    //ボタンの選択状態に応じて色やスケールを変更
    void UpdateButtonSelection()
    {
        for (int i = 0; i < Buttons.Length; i++)
        {
            Image ButtonImage = Buttons[i].GetComponent<Image>();
            Text ButtonText = Buttons[i].GetComponentInChildren<Text>();

            if (i == SelectedIndex)
            {
                //選択中のボタン:文字を赤く、大きくする
                ButtonImage.color = Color.white;

                if (ButtonText != null)
                {
                    ButtonText.color = Color.red;
                    ButtonText.transform.localScale = Vector3.one * 1.3f;
                }
            }
            else
            {
                //非選択のボタン:文字を黒く、通常サイズに戻す
                ButtonImage.color = Color.white;

                if (ButtonText != null)
                {
                    ButtonText.color = Color.black;
                    ButtonText.transform.localScale = Vector3.one;
                }
            }
        }
    }

    void PlaySE(AudioClip clip)
    {
        if(clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
