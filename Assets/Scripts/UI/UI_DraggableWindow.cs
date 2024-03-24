using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_DraggableWindow : UI_PopUp
{
    protected Vector2 firstClickedPos;
    protected Vector2 distanceWithWindow;
    
    protected override void Init()
    {
        base.Init();
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        
        if(Managers.Game.InventoryController.SelectedItem != null) return;
        var newPos = new Vector2(eventData.position.x - distanceWithWindow.x, eventData.position.y - distanceWithWindow.y);
        
        rectTransform.position = newPos;
        Managers.Graphics.Cursor.SetIsDraggingNow(true);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        Managers.Graphics.Cursor.SetIsDraggingNow(false);
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
