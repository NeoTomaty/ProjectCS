using UnityEngine;
using UnityEngine.EventSystems;

public class MenuSelector : MonoBehaviour
{
    public GameObject firstSelected;

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }
}
