using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_DraggableWindow : UI_PopUp
{
    protected Action changeMouseCursorToGrabbing;
    protected Action changeMouseCursorToNormal;
    
    protected Vector2 firstClickedPos;
    protected Vector2 distanceWithWindow;
    
    protected override void Init()
    {
        base.Init();
        
        changeMouseCursorToGrabbing += Managers.Cursor.ChangeCursorForGrabbing;
        changeMouseCursorToNormal += Managers.Cursor.BackNormalCursor;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        
        if(Managers.Game.InventoryController.SelectedItem != null) return;
        var newPos = new Vector2(eventData.position.x - distanceWithWindow.x, eventData.position.y - distanceWithWindow.y);
        
        rectTransform.position = newPos;
        changeMouseCursorToGrabbing?.Invoke();
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        changeMouseCursorToNormal?.Invoke();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            firstClickedPos = eventData.position;
            distanceWithWindow = new Vector2(firstClickedPos.x - rectTransform.position.x,
                firstClickedPos.y - rectTransform.position.y);
        }
    }
}
