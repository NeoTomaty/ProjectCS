//======================================================
// PauseManager スクリプト
// 作成者：宮林
// 最終更新日：5/28
// 
// [Log]5/5 宮林　ポーズ画面を実装
// 5/28　中町　メニュー開閉SE実装
//======================================================
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    //ポーズメニューのUIオブジェクト
    [SerializeField] private GameObject pauseUI;

    //オプションメニューのUIオブジェクト
    [SerializeField] private GameObject optionUI;

    //ポーズメニューで最初に選択されるボタン
    [SerializeField] private GameObject firstPauseButton;

    //オプションメニューで最初に選択されるボタン
    [SerializeField] private GameObject firstOptionButton;

    [Header("SE Settings")]

    //効果音を再生するためのAudioSource
    [SerializeField] private AudioSource audioSource;

    //ポーズメニューを開いたときに鳴らす効果音
    [SerializeField] private AudioClip OpenSE;

    //ポーズメニューを閉じたときに鳴らす効果音
    [SerializeField] private AudioClip CloseSE;

    //現在ポーズ中かどうかを示すフラグ
    private bool isPaused = false;

    //プレイヤーの入力を管理するコンポーネント
    private PlayerInput playerInput;

    //「Pause」アクション(Escキー)を取得するための変数
    private InputAction pauseAction;

    [Header("Reference to Countdown")]
    [SerializeField] private GameStartCountdown gameStartCountdown;

    //ゲーム開始時に呼ばれる(初期化処理)
    private void Awake()
    {
        //PlayerInputコンポーネントを取得
        playerInput = GetComponent<PlayerInput>();

        if (playerInput != null)
        {
            //入力アクションマップから「Pause」アクションを取得
            pauseAction = playerInput.actions["Pause"];
        }
        else
        {
            Debug.LogError("PlayerInputが見つかりません！");
        }
    }

    //オブジェクトが有効になったときに呼ばれる
    private void OnEnable()
    {
        if (pauseAction != null)
        {
            //Pauseアクションが実行されたときのイベントを登録
            pauseAction.performed += OnPausePerformed;
            pauseAction.Enable();
        }
    }

    //オブジェクトが無効になったときに呼ばれる
    private void OnDisable()
    {
        if (pauseAction != null && playerInput.actions != null)
        {
            //イベント登録を解除
            pauseAction.performed -= OnPausePerformed;
            pauseAction.Disable();
        }
    }

    //Pauseアクションが実行されたときに呼ばれる処理
    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        // カウントダウン中ならポーズ禁止
        if (gameStartCountdown != null && gameStartCountdown.IsCountingDown)
        {
            Debug.Log("カウントダウン中なのでポーズ不可");
            return;
        }

        if (optionUI != null && optionUI.activeSelf) return;

        if (!isPaused)
        {
            Time.timeScale = 0f;
            pauseUI.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstPauseButton);
            isPaused = true;
            PlaySE(OpenSE);
        }
        else
        {
            ResumeGame();
        }
    }

    //オプションメニューを開く処理
    public void OpenOption()
    {
        //オプションUIを表示し、ポーズUIを非表示に
        optionUI.SetActive(true);
        pauseUI.SetActive(false);

        //最初に選択されるオプションボタンを選択状態にする
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstOptionButton);
    }

    //ポーズを解除してゲームを再開する処理
    public void ResumeGame()
    {
        //ゲーム内の時間を再開
        Time.timeScale = 1f;

        //ポーズUIを非表示に
        pauseUI.SetActive(false);

        //ポーズ状態を解除
        isPaused = false;

        //閉じるときの効果音を再生
        PlaySE(CloseSE);
    }

    //現在ポーズ中かどうかを外部から取得するための関数
    public bool IsPaused()
    {
        return isPaused;
    }

    //ポーズUIの表示・非表示を切り替える関数
    public void SetPauseUIVisible(bool visible)
    {
        pauseUI.SetActive(visible);
    }

    //効果音を再生する共通関数
    private void PlaySE(AudioClip clip)
    {
        if(audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}