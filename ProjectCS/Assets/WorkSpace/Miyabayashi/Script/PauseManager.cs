//======================================================
// PauseManager スクリプト
// 作成者：宮林
// 最終更新日：5/5
// 
// [Log]5/5 宮林　ポーズ画面を実装
//======================================================
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject optionUI;
    [SerializeField] private GameObject firstPauseButton;
    [SerializeField] private GameObject firstOptionButton;

    private bool isPaused = false;
    private PlayerInput playerInput;
    private InputAction pauseAction;

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
    }

    private void OnEnable()
    {
        if (pauseAction != null)
        {
            pauseAction.performed += OnPausePerformed;
            pauseAction.Enable();
        }
    }

    private void OnDisable()
    {
        if (pauseAction != null && playerInput.actions != null)
        {
            pauseAction.performed -= OnPausePerformed;
            pauseAction.Disable();
        }
    }

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        Debug.Log("esc");
        if (!context.performed) return;

        // オプションが開いていたらポーズ切り替え無効
        if (optionUI != null && optionUI.activeSelf) return;

        if (!isPaused)
        {
            Time.timeScale = 0f;
            pauseUI.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstPauseButton);
            isPaused = true;
        }
        else
        {
            ResumeGame();
        }
    }

    public void OpenOption()
    {
        optionUI.SetActive(true);
        pauseUI.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstOptionButton);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseUI.SetActive(false);
        isPaused = false;
    }

    public bool IsPaused()
    {
        return isPaused;
    }

    public void SetPauseUIVisible(bool visible)
    {
        pauseUI.SetActive(visible);
    }
}