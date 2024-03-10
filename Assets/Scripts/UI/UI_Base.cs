using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Base : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerEnterHandler,IPointerExitHandler
{
    protected RectTransform rectTransform;
    
    protected virtual void Init()
    {
        rectTransform = transform.parent.GetComponent<RectTransform>();
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        Managers.Input.RemovePlayerMouseActions();
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        Managers.Input.RemovePlayerMouseActions();
        Managers.Input.RollbackPlayerMouseActions();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        Managers.Input.RemovePlayerMouseActions();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Managers.Input.RemovePlayerMouseActions();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Managers.Input.RemovePlayerMouseActions();
        Managers.Input.RollbackPlayerMouseActions();
    }
}
