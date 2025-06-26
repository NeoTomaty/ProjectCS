using UnityEngine.EventSystems;
using UnityEngine;

public class OptionBackButton : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject optionUI;
    [SerializeField] private GameObject pauseUI;               // 存在しないシーンもある
    [SerializeField] private GameObject firstPauseButton;      // Pause画面で戻ったときに選ぶボタン
    [SerializeField] private GameObject firstTitleButton;      // Optionを直接開いたときに戻る先（タイトルなど）

    public void CloseOption()
    {
        // Option画面を非表示
        if (optionUI != null)
            optionUI.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);

        if (pauseUI != null)
        {
            // PauseUIが存在する（＝PauseからOptionを開いた）
            pauseUI.SetActive(true);
            if (firstPauseButton != null)
                EventSystem.current.SetSelectedGameObject(firstPauseButton);
        }
        else
        {
            // PauseUIが存在しない（＝Optionを直接開いた）
            if (firstTitleButton != null)
                EventSystem.current.SetSelectedGameObject(firstTitleButton);
        }
    }
}
