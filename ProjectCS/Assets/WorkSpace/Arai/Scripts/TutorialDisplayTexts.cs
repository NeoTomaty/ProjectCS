//======================================================
// [TutorialDisplayTexts]
// 作成者：荒井修
// 最終更新日：06/27
// 
// [Log]
// 06/27　荒井　チュートリアル用のUIを表示させるように実装
//======================================================
using UnityEngine;

public class TutorialDisplayTexts : MonoBehaviour
{
    [Header("チュートリアル用のUI")]

    //表示するチュートリアルUI(Canvasなど)の配列
    [SerializeField] private GameObject[] TutorialUI;

    //現在表示しているチュートリアルのインデックス
    private int TutorialIndex = 0;

    //UIが表示中かどうかのフラグ(Updateでの判定に使用)
    [System.NonSerialized] public bool IsDisplayUI = false;

    [Header("参照")]

    //プレイヤーの状態を管理するコンポーネント(リフティング状態などを取得)
    [SerializeField] private PlayerStateManager PlayerStateManagerComponent;

    //リフティングパートに入ったかどうかのフラグ
    private bool IsLiftingPart = false;

    //ワープ関連のチュートリアルがすでに表示されたかどうか
    private bool IsWarp = false;

    [Header("SE関連")]

    //チュートリアル表示時に再生する効果音
    [SerializeField] private AudioClip TutorialSE;

    //効果音の音量(0.0～1.0)
    [SerializeField, Range(0.0f, 1.0f)] private float SEVolume = 0.5f;

    //効果音を再生するためのAudioSource
    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //チュートリアルUIを非表示にする
        foreach (var UI in TutorialUI)
        {
            UI.SetActive(false);
        }

        //AudioSourceの取得または追加
        audioSource = GetComponent<AudioSource>();

        if(audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        //音量設定
        audioSource.volume = SEVolume;
    }

    // Update is called once per frame
    void Update()
    {
        //UIが表示中のとき、キー入力で次のチュートリアルへ進む
        if (IsDisplayUI)
        {
            if (Input.anyKeyDown)
            {
                //現在のUIを非表示にし、次のインデックスへ
                TutorialUI[TutorialIndex].SetActive(false);
                TutorialIndex++;
                IsDisplayUI = false;

                //ゲームの時間を再開(UI表示中は停止していた)
                Time.timeScale = 1f;

                //チュートリアルの進行に応じて次のUIを表示
                switch(TutorialIndex)
                {
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        //4番目のUIを表示
                        DisplayTutorialUI4();
                        break;
                    case 4:
                        break;
                    case 5:
                        //6番目のUIを表示
                        DisplayTutorialUI6();
                        break;
                    case 6:
                        //7番目のUIを表示
                        DisplayTutorialUI7();
                        break;
                    default:
                        break;
                }

            }
        }

        //プレイヤーがリフティングパートに入ったら、対応するUIを表示
        if (!IsLiftingPart && PlayerStateManagerComponent.GetLiftingState() == PlayerStateManager.LiftingState.LiftingPart)
        {
            IsLiftingPart = true;

            //3番目のUIを表示
            DisplayTutorialUI3();
        }
    }

    //効果音を再生する処理
    private void PlaySE()
    {
        if(TutorialSE != null && audioSource != null)
        {
            audioSource.volume = SEVolume;
            audioSource.PlayOneShot(TutorialSE);
        }
    }

    //以下はそれぞれのチュートリアルUIを表示する関数
    public void DisplayTutorialUI1()
    {
        TutorialUI[0].SetActive(true);
        IsDisplayUI = true;

        //ゲームを一時停止
        Time.timeScale = 0f;
        PlaySE();
    }

    public void DisplayTutorialUI2()
    {
        TutorialUI[1].SetActive(true);
        IsDisplayUI = true;
        Time.timeScale = 0f;
        PlaySE();
    }

    public void DisplayTutorialUI3()
    {
        TutorialUI[2].SetActive(true);
        IsDisplayUI = true;
        Time.timeScale = 0f;
        PlaySE();
    }

    public void DisplayTutorialUI4()
    {
        TutorialUI[3].SetActive(true);
        IsDisplayUI = true;
        Time.timeScale = 0f;
        PlaySE();
    }

    public void DisplayTutorialUI5()
    {
        //ワープ関連のUIは一度しか表示しない
        if (IsWarp) return;
        IsWarp = true;
        TutorialUI[4].SetActive(true);
        IsDisplayUI = true;
        Time.timeScale = 0f;
        PlaySE();
    }

    public void DisplayTutorialUI6()
    {
        TutorialUI[5].SetActive(true);
        IsDisplayUI = true;
        Time.timeScale = 0f;
        PlaySE();
    }

    public void DisplayTutorialUI7()
    {
        TutorialUI[6].SetActive(true);
        IsDisplayUI = true;
        Time.timeScale = 0f;
        PlaySE();
    }
}
