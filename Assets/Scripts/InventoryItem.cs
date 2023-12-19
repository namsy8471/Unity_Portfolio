using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    // 아이템 객체 저장하는 스크립터블 오브젝트
    [SerializeField] private ItemData itemData;
    
    public ItemData ItemData => itemData;
    
    // 아이템 높이 및 길이
    public int Height
    {
        get
        {
            if (rotated == false)
            {
                return itemData.Height;
            }

            return itemData.Width;
        }
    }

    public int Width
    {
        get
        {
            if (rotated == false)
            {
                return itemData.Width;
            }

            return itemData.Height;
        }
    }

    // 인벤토리 창 (그리드) 내의 포지션)
    private int onGridPositionX;
    private int onGridPositionY;

    public int GridPosX
    {
        get =>onGridPositionX;
        set =>onGridPositionX = value;
    }
    
    public int GridPosY
    {
        get => onGridPositionY;
        set => onGridPositionY = value;
    }
    
    // 회전 확인
    private bool rotated = false;
    
    public void SetItemData(ItemData itemData)
    {
        this.itemData = itemData;
        
        GetComponent<Image>().sprite = itemData.ItemIcon;
        GetComponent<Image>().raycastTarget = false;
        this.itemData.SetGridSize();
        this.itemData.GetImageSize(out var sizeX, out var sizeY);
        GetComponent<RectTransform>().sizeDelta = new Vector2(sizeX, sizeY);
    }
    
    public void Rotate()
    {
        rotated = !rotated;

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.rotation = Quaternion.Euler(0,0, rotated ? 90f : 0f);
    }
}
