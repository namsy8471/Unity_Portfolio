using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHighlight : MonoBehaviour
{
    [SerializeField] private RectTransform highlighter;

    public void Show(bool value)
    {
        highlighter.gameObject.SetActive(value);
    }
    
    public void SetSize(InventoryItem targetItem)
    {
        var size = new Vector2();
        size.x = targetItem.Width * ItemGrid.TileSize.Width;
        size.y = targetItem.Height * ItemGrid.TileSize.Height;

        highlighter.sizeDelta = size;
    }
    
    public void SetParent(ItemGrid targetGrid)
    {
        if (targetGrid == null) return;
        highlighter.SetParent(targetGrid.GetComponent<RectTransform>());
    }

    public void SetPosition(ItemGrid targetGrid, InventoryItem targetItem)
    {
        highlighter.localPosition =
            targetGrid.CalculatePositionOnGrid(targetItem, targetItem.GridPosX, targetItem.GridPosY);
    }
    
    public void SetPosition(ItemGrid targetGrid, InventoryItem targetItem, int posX, int posY)
    {
        highlighter.localPosition = targetGrid.CalculatePositionOnGrid(targetItem, posX, posY);
    }
}
