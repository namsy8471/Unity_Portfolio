using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Draggable : UI_PopUp
{
    private Vector2 _firstClickedPos;
    private Vector2 _distanceWithWindow;

    private void Start()
    {
        Init();
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        
        if(Managers.Game.InventoryController.SelectedItem != null) return;
        var newPos = new Vector2(eventData.position.x - _distanceWithWindow.x, eventData.position.y - _distanceWithWindow.y);
        
        RectTransform.position = newPos;
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
            _firstClickedPos = eventData.position;
            _distanceWithWindow = new Vector2(_firstClickedPos.x - RectTransform.position.x,
                _firstClickedPos.y - RectTransform.position.y);
        }
    }
}
