using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ItemGrid
{
    // 타일 하나 당 픽셀 크기 정의
    public struct TileSize
    {
        public const float Width = 32;
        public const float Height = 32;
    }
    
    // 그리드(셀) 크기 (기본 20 X 10)
    struct GridSize
    {
        public int width;
        public int height;
    }
    private GridSize _gridSize;
    
    private InventoryItem[,] _inventoryItemSlot;
    private RectTransform _rectTransform;

    public RectTransform Rect => _rectTransform;
    
    private Vector2 _positionOnTheGrid = new Vector2();
    private Vector2Int _tileGridPosition = new Vector2Int();
    
    private void Init(int width, int height)
    {
        _inventoryItemSlot = new InventoryItem[width, height];
        _rectTransform.sizeDelta = new Vector2(width * TileSize.Width, height * TileSize.Height);
    }

    public void SetGrid(RectTransform rect, int width, int height)
    {
        _rectTransform = rect;
        _gridSize.width = width;
        _gridSize.height = height;
        Init(_gridSize.width, _gridSize.height);
    }
    
    public Vector2Int GetTileGridPosition(Vector2 mousePos)
    {
        var rectPos = _rectTransform.position; 
        
        _positionOnTheGrid.x = mousePos.x - rectPos.x;
        _positionOnTheGrid.y = rectPos.y - mousePos.y;

        _tileGridPosition.x = (int)(_positionOnTheGrid.x / TileSize.Width);
        _tileGridPosition.y = (int)(_positionOnTheGrid.y / TileSize.Height);

        return _tileGridPosition;
    }

    public bool PlaceItem(InventoryItem inventoryItem, int posX, int posY, ref InventoryItem overlapItem)
    {
        if (BoundaryCheck(posX, posY, inventoryItem.Width,
                inventoryItem.Height) == false)
        {
            return false;
        }

        if (OverlapCheck(posX, posY, inventoryItem.Width, inventoryItem.Height,
                ref overlapItem) == false)
        {
            overlapItem = null;
            return false;
        }

        if (overlapItem != null)
        {
            CleanGridReference(overlapItem);
        }
        
        PlaceItem(inventoryItem, posX, posY);

        return true;
    }

    public void PlaceItem(InventoryItem inventoryItem, int posX, int posY)
    {
        var rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(_rectTransform);

        for (int x = 0; x < inventoryItem.Width; x++)
        {
            for (int y = 0; y < inventoryItem.Height; y++)
            {
                _inventoryItemSlot[posX + x, posY + y] = inventoryItem;
            }
        }

        inventoryItem.GridPosX = posX;
        inventoryItem.GridPosY = posY;

        var position = CalculatePositionOnGrid(inventoryItem, posX, posY);

        rectTransform.localPosition = position;
    }

    public Vector2 CalculatePositionOnGrid(InventoryItem inventoryItem, int posX, int posY)
    {
        var position = new Vector2();
        position.x = posX * TileSize.Width + TileSize.Width * inventoryItem.Width / 2;
        position.y = -(posY * TileSize.Height + TileSize.Height * inventoryItem.Height / 2);
        return position;
    }

    private bool OverlapCheck(int posX, int posY, int width, int height, ref InventoryItem overlapItem)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (_inventoryItemSlot[posX + x, posY + y] != null)
                {
                    if (overlapItem == null)
                    {
                        overlapItem = _inventoryItemSlot[posX + x, posY + y];
                    }
                    else
                    {
                        if (overlapItem != _inventoryItemSlot[posX + x, posY + y])
                        {
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }
    
    private bool CheckAvailableSpace(int posX, int posY, int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (_inventoryItemSlot[posX + x, posY + y] != null)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public InventoryItem PickUpItem(int posX, int posY)
    {
        var toReturn = _inventoryItemSlot[posX, posY];

        if (!toReturn)
        {
            return null;
        }
        
        CleanGridReference(toReturn);
        
        return toReturn;
    }

    private void CleanGridReference(InventoryItem item)
    {
        for (int x = 0; x < item.Width; x++)
        {
            for (int y = 0; y < item.Height; y++)
            {
                _inventoryItemSlot[item.GridPosX + x, item.GridPosY + y] = null;
            }
        }
    }

    bool PositionCheck(int posX, int posY)
    {
        if (posX < 0 || posY < 0 || posX >= _gridSize.width || posY >= _gridSize.height)
        {
            Debug.Log("posX : "+ posX + " posY : " + posY + " GridSize.Width : " + _gridSize.width + " GridSize.Height : " + _gridSize.height );
            return false;
        }
        return true;
    }

    public bool BoundaryCheck(int posX, int posY, int width, int height)
    {
        if (PositionCheck(posX, posY) == false)
        {
            return false;
        }

        posX += width - 1;
        posY += height - 1;

        if (PositionCheck(posX, posY) == false)
        {
            return false;
        }
        
        return true;
    }

    public InventoryItem GetItem(int x, int y)
    {
        return _inventoryItemSlot[x, y];
    }

    public Vector2Int? FindSpaceForObject(InventoryItem itemToInsert)
    {
        int height = _gridSize.height - itemToInsert.Height + 1;
        int width = _gridSize.width - itemToInsert.Width + 1;
        
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (CheckAvailableSpace(x, y, itemToInsert.Width,
                        itemToInsert.Height) == true)
                {
                    return new Vector2Int(x, y);
                }
            }
        }

        return null;
    }
}
