using UnityEngine;
using UnityEngine.EventSystems;

public class ImageButtonEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
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

    public void OnSelect(BaseEventData EventData)
    {
        transform.localScale = OriginalScale * 1.2f;
    }

    public void OnDeselect(BaseEventData EventData)
    {
        transform.localScale = OriginalScale;
    }
}
