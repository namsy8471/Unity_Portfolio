using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_SharedCloseButton : UI_Button
{
    public Action CloseOrOpen;
    
    public override void OnPointerClick(PointerEventData eventData)
    {
        CloseOrOpen?.Invoke();
    }
}
