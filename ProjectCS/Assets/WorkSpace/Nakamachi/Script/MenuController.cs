using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Button StartButton;
    public Button ExitButton;
    public Button OptionButton;
    private Button[] Buttons;
    private int SelectedIndex = 0;

    void Start()
    {
        Buttons = new Button[] { StartButton, ExitButton, OptionButton };
        UpdateButtonSelection();
    }

    void Update()
    {
        float HorizontalInput = Input.GetAxis("Horizontal");

        if (HorizontalInput > 0.5f)
        {
            SelectedIndex = (SelectedIndex + 1) % Buttons.Length;
            UpdateButtonSelection();
        }
        else if (HorizontalInput < -0.5f)
        {
            SelectedIndex = (SelectedIndex - 1 + Buttons.Length) % Buttons.Length;
            UpdateButtonSelection();
        }

        if (Input.GetButtonDown("Submit"))
        {
            Buttons[SelectedIndex].onClick.Invoke();
        }
    }

    void UpdateButtonSelection()
    {
        foreach (Button button in Buttons)
        {
            button.GetComponent<Image>().color = Color.white;
        }

        Buttons[SelectedIndex].GetComponent<Image>().color = Color.yellow;
    }
}
