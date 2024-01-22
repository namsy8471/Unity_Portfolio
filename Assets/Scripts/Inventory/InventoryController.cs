using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class InventoryController : MonoBehaviour
{
    private ItemGrid selectedItemGrid;

    public ItemGrid SelectedItemGrid
    {
        get => selectedItemGrid;
        set
        {
            selectedItemGrid = value; 
            inventoryHighlight.SetParent(value);
        }
    }

    [SerializeField] private InventoryItem selectedItem;
    public InventoryItem SelectedItem => selectedItem;
    [SerializeField] private InventoryItem overlapItem;

    private RectTransform rectTransform;

    [SerializeField] private List<ItemData> items;
    
    private Dictionary<string, ItemData> itemData;

    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private GameObject dropItemModeling;
    [SerializeField] private Transform canvasTransform;
    
    private InventoryHighlight inventoryHighlight;
    private InventoryItem itemToHighlight;
    private Vector2Int oldPosition;

    private GraphicRaycaster gr;
    private void Awake()
    {
        itemData = new Dictionary<string, ItemData>();
        
        itemData.Add("Backpack", Resources.Load<ItemData>("ItemData/Backpack"));
        itemData.Add("HpPotion", Resources.Load<ItemData>("ItemData/HpPotion"));
        itemData.Add("LongSword", Resources.Load<ItemData>("ItemData/LongSword"));
        itemData.Add("ShortSword", Resources.Load<ItemData>("ItemData/ShortSword"));
    }

    private void Start()
    {
        inventoryHighlight = GetComponent<InventoryHighlight>();
        var canvas = GameObject.Find("Canvas");
        gr = canvas.GetComponent<GraphicRaycaster>();
    }

    private void Update()
    {
        ItemDrag();
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(selectedItem == null)
                CreateRandomItem();
        }

        if (Input.GetKey(KeyCode.Z))
        {
            InsertRandomItem();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateItem();
        }
        
        if (!selectedItemGrid)
        {
            inventoryHighlight.Show(false);
            if (Input.GetMouseButtonDown(0))
            {
                if(selectedItem != null) 
                    DropItemOnTheGround();
            }
            return;
        }

        HandleHighlight();

        if (Input.GetMouseButtonDown(0))
        {
            LeftMouseButtonPress();
        }

        if (Input.GetMouseButtonDown(1))
        {
            UseItem();
        }
    }

    private void RotateItem()
    {
        if (selectedItem == null)
        {
            return;
        }

        selectedItem.Rotate();
    }

    private void InsertRandomItem()
    {
        if (selectedItemGrid == null)
        {
            return;
        }
        
        CreateRandomItem();
        InventoryItem itemToInsert = selectedItem;
        selectedItem = null;
        InsertItem(itemToInsert);
    }

    private void InsertItem(InventoryItem itemToInsert)
    {
        Vector2Int? posOnGrid = selectedItemGrid.FindSpaceForObject(itemToInsert);
        
        if(posOnGrid == null) return;

        selectedItemGrid.PlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
    }

    private void HandleHighlight()
    {
        Vector2Int positionOnGrid = GetTileGridPosition();
        if (oldPosition == positionOnGrid) return;

        oldPosition = positionOnGrid;
        
        if (selectedItem == null)
        {
            itemToHighlight = selectedItemGrid.GetItem(positionOnGrid.x, positionOnGrid.y);

            if (itemToHighlight != null)
            {
                inventoryHighlight.Show(true);
                inventoryHighlight.SetSize(itemToHighlight);
                inventoryHighlight.SetParent(selectedItemGrid);
                inventoryHighlight.SetPosition(selectedItemGrid,itemToHighlight);
            }
            else
            {
                inventoryHighlight.Show(false);
            }
        }
        else
        {
            inventoryHighlight.Show(selectedItemGrid.BoundaryCheck(
                positionOnGrid.x,
                positionOnGrid.y,
                selectedItem.Width,
                selectedItem.Height
                )
            );
            inventoryHighlight.SetSize(selectedItem);
            inventoryHighlight.SetParent(selectedItemGrid);
            inventoryHighlight.SetPosition(selectedItemGrid, selectedItem, positionOnGrid.x, positionOnGrid.y);
        }
    }

    private void CreateRandomItem()
    {
        InventoryItem inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        selectedItem = inventoryItem;
        
        rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(canvasTransform);
        rectTransform.SetAsLastSibling();

        var randomItemID = Random.Range(0, items.Count);
        inventoryItem.SetItemData(items[randomItemID]);
    }

    private void LeftMouseButtonPress()
    {
        var tileGridPosition = GetTileGridPosition();

        Debug.Log(tileGridPosition);
        
        if (selectedItem == null)
        {
            PickUpItem(tileGridPosition);
        }
        else
        {
            PlaceItem(tileGridPosition);
        }
    }

    private Vector2Int GetTileGridPosition()
    {
        var position = Input.mousePosition;

        if (selectedItem != null)
        {
            position.x -= (selectedItem.Width) * ItemGrid.TileSize.Width / 2;
            position.y += (selectedItem.Height) * ItemGrid.TileSize.Height / 2;
        }

        Vector2Int tileGridPosition = selectedItemGrid.GetTileGridPosition(position);
        return tileGridPosition;
    }

    private void PlaceItem(Vector2Int tileGridPosition)
    {
        bool result = selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y, ref overlapItem);
        if (result)
        {
            selectedItem = null;
            if (overlapItem != null)
            {
                selectedItem = overlapItem;
                overlapItem = null;
                rectTransform = selectedItem.GetComponent<RectTransform>();
                rectTransform.SetAsLastSibling();
            }
        }
    }

    private void PickUpItem(Vector2Int tileGridPosition)
    {
        selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
        selectedItem.ItemData.PickUpItem();
        
        if(selectedItem != null)
            rectTransform = selectedItem.GetComponent<RectTransform>();
    }

    private void ItemDrag()
    {
        if (selectedItem != null)
        {
            var position = Input.mousePosition;

            position.x -= (selectedItem.Width);
            position.y += (selectedItem.Height);
            
            rectTransform.position = position;

            if (selectedItemGrid)
            {
                selectedItem.transform.SetParent(selectedItemGrid.transform);
            }
        }
    }
    
    // 아이템 줍기
    private void RootingItem(string itemName)
    {
        InventoryItem pickedUpItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        selectedItem = pickedUpItem;
        
        rectTransform = pickedUpItem.GetComponent<RectTransform>();
        rectTransform.SetParent(canvasTransform);
        rectTransform.SetAsLastSibling();
        
        pickedUpItem.SetItemData(itemData[itemName]);
        pickedUpItem.name = pickedUpItem.ItemData.ItemName;
        pickedUpItem.ItemData.Init();
    }

    // 아이템 드랍
    private void DropItemOnTheGround()
    {
        if (selectedItem == null) return;
        
        // UI 레이캐스팅
        var results = GraphicRaycastResults();
        if (results.Count > 0) return;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        var pos = Vector3.zero;

        if (Physics.Raycast(ray, out hit, 100f, 1 << LayerMask.NameToLayer("Ground")))
            pos = hit.point;

        if (Vector3.Distance(pos, GameObject.Find("Player").transform.position) > 1f) return;
            
        pos.y = selectedItem.ItemData.GroundYOffset;
        
        // 아이템 정보 얻기
        dropItemModeling = selectedItem.ItemData.ItemModelingForDropping;
        
        var droppedItem = Instantiate(dropItemModeling, pos, dropItemModeling.transform.rotation);
        droppedItem.GetComponent<DroppedItem>().ItemData = selectedItem.ItemData;
        
        // 아이템 프리펩 생성
        selectedItem.ItemData.DropItem();
        inventoryHighlight.transform.SetParent(canvasTransform);
        Destroy(selectedItem.gameObject);
        selectedItem = null;
    }

    private List<RaycastResult> GraphicRaycastResults()
    {
        var ped = new PointerEventData(null);
        ped.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(ped, results);
        return results;
    }

    // 아이템 사용
    private void UseItem()
    {
        itemToHighlight.ItemData.UseItem();
    }
    
    // 아이템 삭제용 샌드메시지
    private void DeleteItemInHighlight()
    {
        Destroy(itemToHighlight.gameObject);
        itemToHighlight = null;
    }
    
    // 아이템 장착 등 현재 마우스에 쥐고 있는 아이템을 없애는 SendMassage
    private void MakeSelectedItemToNull()
    {
        selectedItem = null;
        rectTransform = null;
    }
    
    // 장착된 장비를 마우스로 옮기는 SendMassage
    private void GetItemFromEquipmentSlot(InventoryItem inventoryItem)
    {
        selectedItem = inventoryItem;
        rectTransform = selectedItem.GetComponent<RectTransform>();
    }
    
}
