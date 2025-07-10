//======================================================
// PauseManager スクリプト
// 作成者：宮林
// 最終更新日：6/27
//
// [Log]5/5 宮林　ポーズ画面を実装
// 5/28　中町　メニュー開閉SE実装
// 6/26　森脇 フィニッシュ時のポーズ適応
// 6/27　荒井　チュートリアル用の処理を追加
// 6/27　中町 メニュー開閉SE音量調整実装
//======================================================
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject PlayerObject;

    //ポーズメニューのUIオブジェクト
    [SerializeField] private GameObject pauseUI;

    //オプションメニューのUIオブジェクト
    [SerializeField] private GameObject optionUI;

    //ポーズメニューで最初に選択されるボタン
    [SerializeField] private GameObject firstPauseButton;

    //オプションメニューで最初に選択されるボタン
    [SerializeField] private GameObject firstOptionButton;

    [Header("SE Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip OpenSE;
    [SerializeField] private AudioClip CloseSE;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float SEVolume = 0.5f;

    [Header("Inputs")]
    [SerializeField] private InputActionReference cancelAction;

    private bool isPaused = false;

    private PlayerInput playerInput;
    private InputAction pauseAction;
    private InputAction PlayerDecelerationAction;

    [Header("Reference to Countdown")]
    [SerializeField] private GameStartCountdown gameStartCountdown;

    [SerializeField] private Animator playerAnimator;
    [SerializeField] private BlownAway_Ver3 targetSnack;

    [Header("チュートリアル用（チュートリアル以外では割り当てNG）")]
    [SerializeField] private TutorialDisplayTexts TutorialDisplayTexts;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        if (playerInput != null)
        {
            pauseAction = playerInput.actions["Pause"];
        }
        else
        {
            Debug.LogError("PlayerInputが見つかりません！");
        }

        if (PlayerObject != null)
        {
            PlayerDecelerationAction = PlayerObject.GetComponent<PlayerInput>().actions["Deceleration"];
        }
    }

    private void OnEnable()
    {
        if (pauseAction != null)
        {
            pauseAction.performed += OnPausePerformed;
            pauseAction.Enable();
        }

        if (cancelAction != null)
        {
            cancelAction.action.performed += OnCancelPerformed;
            cancelAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (pauseAction != null && playerInput.actions != null)
        {
            pauseAction.performed -= OnPausePerformed;
            pauseAction.Disable();
        }

        if (cancelAction != null)
        {
            cancelAction.action.performed -= OnCancelPerformed;
            cancelAction.action.Disable();
        }
    }

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (TutorialDisplayTexts != null && TutorialDisplayTexts.IsDisplayUI) return;

        if (gameStartCountdown != null && gameStartCountdown.IsCountingDown)
        {
            Debug.Log("カウントダウン中なのでポーズ不可");
            return;
        }

        if (optionUI != null && optionUI.activeSelf) return;

        if (!isPaused)
        {
            if (pauseUI == null)
            {
                Time.timeScale = 0f;
                optionUI.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(firstOptionButton);
            }
            else
            {
                //if (!targetSnack.isHitStopActive)
                {
                    Time.timeScale = 0f;
                    if (playerAnimator != null)
                        playerAnimator.speed = 0f;
                    pauseUI.SetActive(true);
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(firstPauseButton);
                    isPaused = true;
                    PlaySE(OpenSE);
                }
            }

            if (PlayerDecelerationAction != null)
                PlayerDecelerationAction.Disable(); // ポーズ中は減速アクションを無効化
        }
        else
        {
            ResumeGame();
        }
    }
    private void OnCancelPerformed(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        // オプション画面が開いているとき
        if (optionUI != null && optionUI.activeSelf)
        {
            optionUI.SetActive(false);

            // PauseUIが存在する場合はポーズ画面に戻る
            if (pauseUI != null)
            {
                pauseUI.SetActive(true);

                if (firstPauseButton != null)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(firstPauseButton);
                }

                isPaused = true; // ここが大事：Pause状態に戻す
            }
            else
            {
                // タイトル画面など、Pauseが存在しないなら完全に閉じる
                Time.timeScale = 1f;
                isPaused = false;
            }

            PlaySE(CloseSE);
        }
    }

    public void OpenOptionStandalone()
    {
        if (pauseUI == null)
        {
            Time.timeScale = 0f;
            optionUI.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstOptionButton);
        }
    }

    public void OpenOption()
    {
        if (optionUI != null)
            optionUI.SetActive(true);

        if (pauseUI != null)
            pauseUI.SetActive(false);

        if (firstOptionButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstOptionButton);
        }
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;

        if (playerAnimator != null)
            playerAnimator.speed = 1f;

        pauseUI.SetActive(false);
        isPaused = false;

        if (PlayerDecelerationAction != null)
            PlayerDecelerationAction.Enable(); // ポーズ解除時に減速アクションを有効化

        PlaySE(CloseSE);
    }

    public bool IsPaused()
    {
        return isPaused;
    }

    public bool IsMenuOpen()
    {
        return (optionUI != null && optionUI.activeSelf) || (pauseUI != null && pauseUI.activeSelf);
    }

    public void SetPauseUIVisible(bool visible)
    {
        pauseUI.SetActive(visible);
    }

    private void PlaySE(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip, SEVolume);
        }
    }
}
