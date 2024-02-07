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
        _highlighter.transform.parent = Managers.Game.MainInventoryUICanvas.transform;
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
    
    public void SetParent(ItemGrid targetGrid)
    {
        if (targetGrid == null) return;
        _highlighter.SetParent(targetGrid.GetComponent<RectTransform>());
    }

    public void SetPosition(ItemGrid targetGrid, InventoryItem targetItem)
    {
        _highlighter.localPosition =
            targetGrid.CalculatePositionOnGrid(targetItem, targetItem.GridPosX, targetItem.GridPosY);
    }
    
    public void SetPosition(ItemGrid targetGrid, InventoryItem targetItem, int posX, int posY)
    {
        _highlighter.localPosition = targetGrid.CalculatePositionOnGrid(targetItem, posX, posY);
    }
}
