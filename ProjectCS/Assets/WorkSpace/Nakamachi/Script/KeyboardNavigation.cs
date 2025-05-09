using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class KeyboardNavigation : MonoBehaviour
{
    public Button[] buttons;
    private int selectedIndex = 0;
    private Vector3[] originalScales;
    private Color[] originalColors;

    void Start()
    {
        originalScales = new Vector3[buttons.Length];
        originalColors = new Color[buttons.Length];

        for (int i = 0; i < buttons.Length; i++)
        {
            Text buttonText = buttons[i].GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                originalScales[i] = buttonText.transform.localScale;
                originalColors[i] = buttonText.color;
            }
            else
            {
                originalScales[i] = buttons[i].transform.localScale;
            }
        }

        SelectButton(selectedIndex);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectedIndex = (selectedIndex + 1) % buttons.Length;
            SelectButton(selectedIndex);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectedIndex = (selectedIndex - 1 + buttons.Length) % buttons.Length;
            SelectButton(selectedIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            buttons[selectedIndex].onClick.Invoke();
        }
    }

    void SelectButton(int index)
    {
        // Reset previous button
        ResetButton(selectedIndex);

        // Select new button
        selectedIndex = index;
        EventSystem.current.SetSelectedGameObject(buttons[selectedIndex].gameObject);

        // Apply effects to selected button
        ApplyEffects(selectedIndex);
    }

    void ApplyEffects(int index)
    {
        Text buttonText = buttons[index].GetComponentInChildren<Text>();
        if (buttonText != null)
        {
            buttonText.transform.localScale = originalScales[index] * 1.2f;
            buttonText.color = Color.red; // ëIëíÜÇÃêF
        }
        else if (buttons[index].name == "Option")
        {
            buttons[index].transform.localScale = originalScales[index] * 1.2f;
        }
        else
        {
            buttons[index].transform.localScale = originalScales[index] * 1.2f;
        }
    }

    void ResetButton(int index)
    {
        Text buttonText = buttons[index].GetComponentInChildren<Text>();
        if (buttonText != null)
        {
            buttonText.transform.localScale = originalScales[index];
            buttonText.color = originalColors[index]; // å≥ÇÃêFÇ…ñﬂÇ∑
        }
        else
        {
            buttons[index].transform.localScale = originalScales[index];
        }
    }
}
