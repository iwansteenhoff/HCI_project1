using UnityEngine;
using UnityEngine.EventSystems;

public class MenuArea : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isMouseOverMenu = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOverMenu = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOverMenu = false;
    }
}
