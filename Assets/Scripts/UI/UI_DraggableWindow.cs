using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_DraggableWindow : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    protected RectTransform rectTransform;
    
    protected Action changeMouseCursorToGrabbing;
    protected Action changeMouseCursorToNormal;
    
    protected Vector2 firstClickedPos;
    protected Vector2 distanceWithWindow;
    
    protected virtual void Init()
    {
        changeMouseCursorToGrabbing += Managers.Cursor.ChangeCursorForGrabbing;
        changeMouseCursorToNormal += Managers.Cursor.BackNormalCursor;
        
        rectTransform = transform.parent.GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(Managers.Game.InventoryController.SelectedItem != null) return;
        var newPos = new Vector2(eventData.position.x - distanceWithWindow.x, eventData.position.y - distanceWithWindow.y);
        
        rectTransform.position = newPos;
        changeMouseCursorToGrabbing?.Invoke();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        changeMouseCursorToNormal?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            firstClickedPos = eventData.position;
            distanceWithWindow = new Vector2(firstClickedPos.x - rectTransform.position.x,
                firstClickedPos.y - rectTransform.position.y);
        }
    }
    
    public virtual void CloseButtonClick(){}
}
