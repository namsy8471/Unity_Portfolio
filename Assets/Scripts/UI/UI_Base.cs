using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Base : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler,
    IPointerEnterHandler, IPointerExitHandler
{
    protected RectTransform RectTransform;
    private bool _isDragging;
    
    protected virtual void Init()
    {
        RectTransform = transform.parent.GetComponent<RectTransform>();
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        _isDragging = true;
        Managers.Input.RemovePlayerMouseActions();
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        _isDragging = false;
        
        Managers.Input.RemovePlayerMouseActions();
        Managers.Input.RollbackPlayerMouseActions();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        Managers.Input.RemovePlayerMouseActions();
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        Managers.Input.RemovePlayerMouseActions();
        Managers.Input.RollbackPlayerMouseActions();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isDragging) return;
        
        Managers.Input.RemovePlayerMouseActions();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isDragging) return;
        
        Managers.Input.RemovePlayerMouseActions();
        Managers.Input.RollbackPlayerMouseActions();
    }
}
    
