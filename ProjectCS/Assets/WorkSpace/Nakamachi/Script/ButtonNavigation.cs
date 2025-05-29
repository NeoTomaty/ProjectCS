//ButtonNavigation.cs
//作成者:中町雷我
//最終更新日:2025/05/11
//アタッチ:Startボタンにアタッチ
//[Log]
//05/11　中町　メニュー決定SE

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonNavigation : MonoBehaviour
{
    public Button[] Buttons; // ボタンの配列
    private int CurrentIndex = 0;
    private Vector3 OriginalScale;
    private Color OriginalColor;

    void Start()
    {
        // 最初のボタンを選択
        EventSystem.current.SetSelectedGameObject(Buttons[CurrentIndex].gameObject);
        OriginalScale = Buttons[CurrentIndex].GetComponentInChildren<Text>().transform.localScale;
        OriginalColor = Buttons[CurrentIndex].GetComponentInChildren<Text>().color;
        EnlargeText(Buttons[CurrentIndex]);
        ChangeTextColor(Buttons[CurrentIndex], Color.red);
    }

    void Update()
    {
        // 左矢印キーで前のボタンを選択
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ResetText(Buttons[CurrentIndex]);
            CurrentIndex = (CurrentIndex - 1 + Buttons.Length) % Buttons.Length;
            EventSystem.current.SetSelectedGameObject(Buttons[CurrentIndex].gameObject);
            EnlargeText(Buttons[CurrentIndex]);
            ChangeTextColor(Buttons[CurrentIndex], Color.red);
        }

        // 右矢印キーで次のボタンを選択
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ResetText(Buttons[CurrentIndex]);
            CurrentIndex = (CurrentIndex + 1) % Buttons.Length;
            EventSystem.current.SetSelectedGameObject(Buttons[CurrentIndex].gameObject);
            EnlargeText(Buttons[CurrentIndex]);
            ChangeTextColor(Buttons[CurrentIndex], Color.red);
        }

        // Returnキーでボタンをクリック
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
}
