using UnityEngine;
using UnityEngine.EventSystems;

public class Preview : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    public bool isBeeingHovered = false;
    public void OnPointerEnter(PointerEventData eventData)
    {
        isBeeingHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isBeeingHovered = false;
    }
}
