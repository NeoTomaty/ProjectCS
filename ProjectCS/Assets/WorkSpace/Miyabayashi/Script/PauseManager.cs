//======================================================
// PauseManager �X�N���v�g
// �쐬�ҁF�{��
// �ŏI�X�V���F5/5
// 
// [Log]5/5 �{�с@�|�[�Y��ʂ�����
//======================================================
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject optionUI; 
    private bool isPaused = false;
    private PlayerInput playerInput;
    [SerializeField] private GameObject firstPauseButton;
    [SerializeField] private GameObject firstOptionButton;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        if (playerInput != null)
        {
            playerInput.actions["Pause"].performed += OnPausePerformed;
        }
    }

    private void OnDestroy()
    {
        if (playerInput != null)
        {
            playerInput.actions["Pause"].performed -= OnPausePerformed;
        }
    }

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        // ���ǉ��F�I�v�V�������J���Ă�����|�[�Y�̐؂�ւ�����
        if (optionUI != null && optionUI.activeSelf)
            return;

        if (!isPaused)
        {
            EventSystem.current.SetSelectedGameObject(firstPauseButton);
            Time.timeScale = 0f;
            pauseUI.SetActive(true);
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
