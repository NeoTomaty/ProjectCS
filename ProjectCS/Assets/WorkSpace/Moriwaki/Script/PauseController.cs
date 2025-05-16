//======================================================
// [ReflectingNPC]
// �쐬�ҁF�X�e
// �ŏI�X�V���F05/16
//
// [Log]
// 05/16�@�X�e �|�[�Y�̊O������
//======================================================

using UnityEngine;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour
{
    [SerializeField] private PauseManager pauseManager;

    private InputAction resumeAction;

    private void Awake()
    {
        // InputAction�iESC�L�[��XBOX�̃��j���[�{�^�������m�j
        resumeAction = new InputAction(type: InputActionType.Button);

        // �L�[�{�[�h��ESC�A�Q�[���p�b�h��Start�i�߃{�^���j�Ƀo�C���h
        resumeAction.AddBinding("<Keyboard>/escape");
        resumeAction.AddBinding("<Gamepad>/start");

        resumeAction.performed += OnResumeTriggered;
    }

    private void OnEnable()
    {
        resumeAction.Enable();
    }

    private void OnDisable()
    {
        resumeAction.Disable();
    }

    private void OnDestroy()
    {
        resumeAction.performed -= OnResumeTriggered;
    }

    private void OnResumeTriggered(InputAction.CallbackContext context)
    {
        if (pauseManager != null && pauseManager.IsPaused())
        {
            pauseManager.ResumeGame();
        }
    }

    // �O��UI�{�^���p
    public void OnResumeButtonPressed()
    {
        if (pauseManager != null && pauseManager.IsPaused())
        {
            pauseManager.ResumeGame();
        }
    }
}