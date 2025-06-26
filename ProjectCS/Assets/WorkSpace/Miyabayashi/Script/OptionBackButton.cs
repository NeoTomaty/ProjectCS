using UnityEngine.EventSystems;
using UnityEngine;

public class OptionBackButton : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject optionUI;
    [SerializeField] private GameObject pauseUI;               // ���݂��Ȃ��V�[��������
    [SerializeField] private GameObject firstPauseButton;      // Pause��ʂŖ߂����Ƃ��ɑI�ԃ{�^��
    [SerializeField] private GameObject firstTitleButton;      // Option�𒼐ڊJ�����Ƃ��ɖ߂��i�^�C�g���Ȃǁj

    public void CloseOption()
    {
        // Option��ʂ��\��
        if (optionUI != null)
            optionUI.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);

        if (pauseUI != null)
        {
            // PauseUI�����݂���i��Pause����Option���J�����j
            pauseUI.SetActive(true);
            if (firstPauseButton != null)
                EventSystem.current.SetSelectedGameObject(firstPauseButton);
        }
        else
        {
            // PauseUI�����݂��Ȃ��i��Option�𒼐ڊJ�����j
            if (firstTitleButton != null)
                EventSystem.current.SetSelectedGameObject(firstTitleButton);
        }
    }
}
