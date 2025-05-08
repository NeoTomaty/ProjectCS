using UnityEngine;
using UnityEngine.EventSystems;

public class ImageButtonEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 OriginalScale;

    void Start()
    {
        OriginalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData EventData)
    {
        transform.localScale = OriginalScale * 1.2f;
    }

    public void OnPointerExit(PointerEventData EventData)
    {
        transform.localScale = OriginalScale;
    }
}