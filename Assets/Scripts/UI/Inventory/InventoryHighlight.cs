using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class InventoryHighlight
{
    private RectTransform _highlighter;

    public RectTransform Highlighter => _highlighter;
    public void Init()
    {
        _highlighter = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/Inventory/UI_Highlighter")).GetComponent<RectTransform>();
        _highlighter.transform.SetParent(Managers.Game.InventoryUICanvas.transform);
    }
    
    public void Show(bool value)
    {
        _highlighter.gameObject.SetActive(value);
    }
    
    public void SetSize(InventoryItem targetItem)
    {
        var size = new Vector2();
        size.x = targetItem.Width * ItemGrid.TileSize.Width;
        size.y = targetItem.Height * ItemGrid.TileSize.Height;

        _highlighter.sizeDelta = size;
    }
    
    public void SetParent(Inventory_Base targetInventory)
    {
        if (targetInventory == null) return;
        _highlighter.SetParent(targetInventory.transform);
    }

    public void SetPosition(Inventory_Base targetInventory, InventoryItem targetItem)
    {
        _highlighter.localPosition =
            targetInventory.CalculatePositionOnGrid(targetItem, targetItem.GridPosX, targetItem.GridPosY);
    }
    
    public void SetPosition(Inventory_Base targetInventory, InventoryItem targetItem, int posX, int posY)
    {
        _highlighter.localPosition = targetInventory.CalculatePositionOnGrid(targetItem, posX, posY);
    }
}
