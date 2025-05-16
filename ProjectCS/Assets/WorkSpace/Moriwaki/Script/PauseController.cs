//======================================================
// [ReflectingNPC]
// 作成者：森脇
// 最終更新日：05/16
//
// [Log]
// 05/16　森脇 ポーズの外部操作
//======================================================

using UnityEngine;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour
{
    [SerializeField] private PauseManager pauseManager;

    private InputAction resumeAction;

    private void Awake()
    {
        // InputAction（ESCキーとXBOXのメニューボタンを検知）
        resumeAction = new InputAction(type: InputActionType.Button);

        // キーボードのESC、ゲームパッドのStart（≡ボタン）にバインド
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

    // 外部UIボタン用
    public void OnResumeButtonPressed()
    {
        if (pauseManager != null && pauseManager.IsPaused())
        {
            pauseManager.ResumeGame();
        }
    }
}