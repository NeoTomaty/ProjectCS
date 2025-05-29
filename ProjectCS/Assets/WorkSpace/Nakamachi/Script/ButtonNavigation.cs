//ButtonNavigation.cs
//�쐬��:��������
//�ŏI�X�V��:2025/05/11
//�A�^�b�`:Start�{�^���ɃA�^�b�`
//[Log]
//05/11�@�����@���j���[����SE

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonNavigation : MonoBehaviour
{
    public Button[] Buttons; // �{�^���̔z��
    private int CurrentIndex = 0;
    private Vector3 OriginalScale;
    private Color OriginalColor;

    void Start()
    {
        // �ŏ��̃{�^����I��
        EventSystem.current.SetSelectedGameObject(Buttons[CurrentIndex].gameObject);
        OriginalScale = Buttons[CurrentIndex].GetComponentInChildren<Text>().transform.localScale;
        OriginalColor = Buttons[CurrentIndex].GetComponentInChildren<Text>().color;
        EnlargeText(Buttons[CurrentIndex]);
        ChangeTextColor(Buttons[CurrentIndex], Color.red);
    }

    void Update()
    {
        // �����L�[�őO�̃{�^����I��
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ResetText(Buttons[CurrentIndex]);
            CurrentIndex = (CurrentIndex - 1 + Buttons.Length) % Buttons.Length;
            EventSystem.current.SetSelectedGameObject(Buttons[CurrentIndex].gameObject);
            EnlargeText(Buttons[CurrentIndex]);
            ChangeTextColor(Buttons[CurrentIndex], Color.red);
        }

        // �E���L�[�Ŏ��̃{�^����I��
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ResetText(Buttons[CurrentIndex]);
            CurrentIndex = (CurrentIndex + 1) % Buttons.Length;
            EventSystem.current.SetSelectedGameObject(Buttons[CurrentIndex].gameObject);
            EnlargeText(Buttons[CurrentIndex]);
            ChangeTextColor(Buttons[CurrentIndex], Color.red);
        }

        // Return�L�[�Ń{�^�����N���b�N
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
