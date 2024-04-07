using System;
using UnityEngine.EventSystems;

public class UI_InventoryButton : UI_Base, IPointerClickHandler
{
    public Action CloseOrOpenInventory;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        CloseOrOpenInventory?.Invoke();
    }
}
