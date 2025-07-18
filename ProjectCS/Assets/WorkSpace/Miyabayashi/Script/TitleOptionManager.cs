using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class TitleOptionManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject optionUI;
    [SerializeField] private GameObject firstOptionButton;

    [Header("タイトルのUIルート（全体を無効化）")]
    [SerializeField] private GameObject titleUIRoot;

    [Header("効果音")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openSE;
    [SerializeField] private AudioClip closeSE;

    [Range(0f, 1f)]
    [SerializeField] private float SEVolume = 0.5f;

    private PlayerInput playerInput;
    private InputAction pauseAction;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            pauseAction = playerInput.actions["Pause"];
        }
        else
        {
            Debug.LogError("PlayerInput が見つかりません！");
        }
    }

    void OnEnable()
    {
        if (pauseAction != null)
        {
            pauseAction.performed += OnPausePressed;
            pauseAction.Enable();
        }
    }

    void OnDisable()
    {
        if (pauseAction != null)
        {
            pauseAction.performed -= OnPausePressed;
            pauseAction.Disable();
        }
    }

    private void OnPausePressed(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        // 状態確認に isOpen ではなく optionUI.activeSelf を使用
        if (optionUI != null && optionUI.activeSelf)
        {
            CloseOption();
        }
        else
        {
            OpenOption();
        }
    }

    public void OpenOption()
    {
        if (optionUI == null || optionUI.activeSelf) return;

        optionUI.SetActive(true);
        if (titleUIRoot != null)
            titleUIRoot.SetActive(false); // タイトルUIを無効化

        if (firstOptionButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstOptionButton);
        }

        PlaySE(openSE);
    }

    public void CloseOption()
    {
        if (optionUI == null || !optionUI.activeSelf) return;

        optionUI.SetActive(false);
        if (titleUIRoot != null)
            titleUIRoot.SetActive(true); // タイトルUIを有効化

        EventSystem.current.SetSelectedGameObject(null);

        PlaySE(closeSE);
    }

    public bool IsOpen()
    {
        return optionUI != null && optionUI.activeSelf;
    }

    private void PlaySE(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip, SEVolume);
        }
    }
}
