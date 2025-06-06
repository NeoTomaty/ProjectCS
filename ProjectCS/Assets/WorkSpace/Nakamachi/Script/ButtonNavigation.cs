//ButtonNavigation.cs
//作成者:中町雷我
//最終更新日:2025/05/11
//アタッチ:Startボタンにアタッチ
//[Log]
//05/11　中町　メニュー選択&決定処理

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonNavigation : MonoBehaviour
{
    public Button[] Buttons;
    public AudioClip NavigationSE;
    private AudioSource audioSource;

    private int CurrentIndex = 0;
    private Vector3 OriginalScale;
    private Color OriginalColor;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        EventSystem.current.SetSelectedGameObject(Buttons[CurrentIndex].gameObject);
        OriginalScale = Buttons[CurrentIndex].GetComponentInChildren<Text>().transform.localScale;
        OriginalColor = Buttons[CurrentIndex].GetComponentInChildren<Text>().color;
        EnlargeText(Buttons[CurrentIndex]);
        ChangeTextColor(Buttons[CurrentIndex], Color.red);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PlayNavigationSE();
            ResetText(Buttons[CurrentIndex]);
            CurrentIndex = (CurrentIndex - 1 + Buttons.Length) % Buttons.Length;
            EventSystem.current.SetSelectedGameObject(Buttons[CurrentIndex].gameObject);
            EnlargeText(Buttons[CurrentIndex]);
            ChangeTextColor(Buttons[CurrentIndex], Color.red);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            PlayNavigationSE();
            ResetText(Buttons[CurrentIndex]);
            CurrentIndex = (CurrentIndex + 1) % Buttons.Length;
            EventSystem.current.SetSelectedGameObject(Buttons[CurrentIndex].gameObject);
            EnlargeText(Buttons[CurrentIndex]);
            ChangeTextColor(Buttons[CurrentIndex], Color.red);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Buttons[CurrentIndex].onClick.Invoke();
        }
    }

    void EnlargeText(Button button)
    {
        button.GetComponentInChildren<Text>().transform.localScale = OriginalScale * 1.2f;
    }

    void ResetText(Button button)
    {
        button.GetComponentInChildren<Text>().transform.localScale = OriginalScale;
        button.GetComponentInChildren<Text>().color = OriginalColor;
    }

    void ChangeTextColor(Button button, Color color)
    {
        button.GetComponentInChildren<Text>().color = color;
    }

    void PlayNavigationSE()
    {
        if(NavigationSE != null && audioSource != null)
        {
            audioSource.PlayOneShot(NavigationSE);
        }
    }
}
