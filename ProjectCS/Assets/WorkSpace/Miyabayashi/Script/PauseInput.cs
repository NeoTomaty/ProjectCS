using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseUI; // �|�[�Y���UI

    private bool isPaused = false;

    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        // "Pause" �A�N�V�����擾���ăC�x���g�o�^
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
                ResumeGame(); // �ĊJ
            }
        }
    }

    // UI�{�^������Ăׂ�ĊJ����
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseUI.SetActive(false);
        isPaused = false;
    }
}
