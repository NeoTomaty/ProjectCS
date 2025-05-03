using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseUI; // ポーズ画面UI

    private bool isPaused = false;

    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        // "Pause" アクション取得してイベント登録
        playerInput.actions["Pause"].performed += OnPausePerformed;
    }

    private void OnEnable()
    {
        if (playerInput != null)
            playerInput.actions["Pause"].performed += OnPausePerformed;
    }

    private void OnDisable()
    {
        if (playerInput != null)
            playerInput.actions["Pause"].performed -= OnPausePerformed;
    }

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!isPaused)
            {
                Time.timeScale = 0f;
                pauseUI.SetActive(true);
                isPaused = true;
            }
            else
            {
                ResumeGame(); // 再開
            }
        }
    }

    // UIボタンから呼べる再開処理
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseUI.SetActive(false);
        isPaused = false;
    }
}
