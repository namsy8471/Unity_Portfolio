using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;

public abstract class Inventory_Base : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    protected ItemGrid _itemGrid;

    public ItemGrid ItemGrid => _itemGrid;
    
    public virtual void Init(int width, int height)
    {
        _itemGrid = new ItemGrid();
        _itemGrid.SetGrid(GetComponent<RectTransform>(), width, height);
    }

    public Vector2Int? FindSpaceForObject(InventoryItem itemToInsert)
    {
        return _itemGrid.FindSpaceForObject(itemToInsert);
    }

    public bool PlaceItem(InventoryItem itemToInsert, int x, int y, ref InventoryItem overlapItem)
    {
        return _itemGrid.PlaceItem(itemToInsert, x, y, ref overlapItem);
    }
    
    public void PlaceItem(InventoryItem itemToInsert, int x, int y)
    {
        _itemGrid.PlaceItem(itemToInsert, x, y);
    }

    public InventoryItem GetItem(int x, int y)
    {
        return _itemGrid.GetItem(x, y);
    }

    public bool BoundaryCheck(int x, int y, int selectedItemWidth, int selectedItemHeight)
    {
        return _itemGrid.BoundaryCheck(x, y, selectedItemWidth, selectedItemHeight);
    }
    
    public Vector2Int GetTileGridPosition(Vector3 position)
    {
        return _itemGrid.GetTileGridPosition(position);
    }

    public InventoryItem PickUpItem(int x, int y)
    {
        return _itemGrid.PickUpItem(x, y);
    }

    public Vector3 CalculatePositionOnGrid(InventoryItem targetItem, int x, int y)
    {
        return _itemGrid.CalculatePositionOnGrid(targetItem, x, y);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }
}
