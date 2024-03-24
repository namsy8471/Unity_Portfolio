using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class InventoryController
{
    private Inventory_Base _selectedInventory;

    public Inventory_Base SelectedInventory
    {
        get => _selectedInventory;
        set
        {
            _selectedInventory = value; 
            _inventoryHighlight.SetParent(value);
        }
    }

    private InventoryItem _selectedItem;
    public InventoryItem SelectedItem => _selectedItem;
    private InventoryItem _overlapItem;

    private RectTransform _rectTransform;

    private List<ItemData> _items;
    
    private Dictionary<string, ItemData> _itemData = new Dictionary<string, ItemData>();

    private GameObject _itemPrefab;
    private GameObject _dropItemModeling;
    private GameObject _itemToLoot;
    
    private Transform _canvasTransform;
    
    private InventoryHighlight _inventoryHighlight = new InventoryHighlight();
    private InventoryItem _itemToHighlight;
    private Vector2Int _oldPosition;
    
    public void Init()
    {
        _itemData.Add("Backpack", Resources.Load<ItemData>("ItemData/Backpack"));
        _itemData.Add("HpPotion", Resources.Load<ItemData>("ItemData/HpPotion"));
        _itemData.Add("LongSword", Resources.Load<ItemData>("ItemData/LongSword"));
        _itemData.Add("ShortSword", Resources.Load<ItemData>("ItemData/ShortSword"));

        #region KeyBinding

        Managers.Input.AddAction(Managers.Input.KeyButtonDown, Managers.Input.InventoryItemRotateKey, RotateItem);

        Managers.Input.LMBDown += LeftMouseButtonPress;
        Managers.Input.RMBDown += UseItem;
        
        #endregion
        
        _inventoryHighlight.Init();
        
        _canvasTransform = Managers.Game.InventoryUICanvas.gameObject.transform;
    }

    public void Update()
    {
        ItemDrag();
        HandleHighlight();
    }
    
    private void LeftMouseButtonPress()
    {
        var tileGridPosition = GetTileGridPosition();
        
        Debug.Log("포지션 = " + tileGridPosition);
        
        if (tileGridPosition == new Vector2Int(-1, -1))
        {
            if (_selectedItem != null)
            {
                DropItemOnTheGround();
            }
            else
            {
                LootingItemFromGround();
            }
        }
        else
        {
            if (_selectedItem != null)
            {
                PlaceItem(tileGridPosition);
            }
            else
            {
                PickUpItem(tileGridPosition);
            }   
        }
    }
    
    private void LootingItemFromGround()
    {
        if (_selectedItem != null || EventSystem.current.IsPointerOverGameObject()) return;
        
        if (Managers.Ray.RayHitCollider.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            if (Input.GetMouseButton(0))
            {
                _itemToLoot = Managers.Ray.RayHitCollider.transform.gameObject;
                //Managers.Game.Player.GetComponent<PlayerController>().MoveState.DestPos = _itemToLoot.transform.position;
            }
            
            if (_itemToLoot == Managers.Ray.RayHitCollider.transform.gameObject && Vector3.Distance(Managers.Ray.RayHitPoint, Managers.Game.Player.transform.position) < 2.0f)
            {
                LootingItem(Managers.Ray.RayHitCollider.transform.name);
                Object.Destroy(Managers.Ray.RayHitCollider.transform.gameObject);
            }
        }
    }

    private void RotateItem()
    {
        if (_selectedItem == null) return;

        _selectedItem.Rotate();
    }

    private void InsertItem(InventoryItem itemToInsert)
    {
        Vector2Int? posOnGrid = _selectedInventory.FindSpaceForObject(itemToInsert);
        
        if(posOnGrid == null) return;

        _selectedInventory.PlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
    }

    private void HandleHighlight()
    {
        Vector2Int positionOnGrid = GetTileGridPosition();
        
        if (_oldPosition == positionOnGrid) return;
        if (positionOnGrid == new Vector2Int(-1, -1))
        {
            _inventoryHighlight.Show(false);
            return;
        }

        _oldPosition = positionOnGrid;
        
        if (_selectedItem == null)
        {
            _itemToHighlight = _selectedInventory.GetItem(positionOnGrid.x, positionOnGrid.y);

            if (_itemToHighlight != null)
            {
                _inventoryHighlight.Show(true);
                _inventoryHighlight.SetSize(_itemToHighlight);
                _inventoryHighlight.SetParent(_selectedInventory);
                _inventoryHighlight.SetPosition(_selectedInventory,_itemToHighlight);
            }
            else
            {
                _inventoryHighlight.Show(false);
            }
        }
        else
        {
            _inventoryHighlight.Show(_selectedInventory.BoundaryCheck(
                positionOnGrid.x,
                positionOnGrid.y,
                _selectedItem.Width,
                _selectedItem.Height
                )
            );
            _inventoryHighlight.SetSize(_selectedItem);
            _inventoryHighlight.SetParent(_selectedInventory);
            _inventoryHighlight.SetPosition(_selectedInventory, _selectedItem, positionOnGrid.x, positionOnGrid.y);
        }
    }



    private Vector2Int GetTileGridPosition()
    {
        var position = Input.mousePosition;

        if (_selectedItem != null)
        {
            position.x -= (_selectedItem.Width) * ItemGrid.TileSize.Width / 2;
            position.y += (_selectedItem.Height) * ItemGrid.TileSize.Height / 2;
        }

        Vector2Int tileGridPosition = _selectedInventory?.GetTileGridPosition(position) ?? new Vector2Int(-1, -1);
        
        return tileGridPosition;
    }

    private void PlaceItem(Vector2Int tileGridPosition)
    {
        bool result = _selectedInventory.PlaceItem(_selectedItem, tileGridPosition.x, tileGridPosition.y, ref _overlapItem);
        if (result)
        {
            _selectedItem = null;
            if (_overlapItem != null)
            {
                _selectedItem = _overlapItem;
                _overlapItem = null;
                _rectTransform = _selectedItem.GetComponent<RectTransform>();
                _rectTransform.SetAsLastSibling();
            }
            
        }
    }

    private void PickUpItem(Vector2Int tileGridPosition)
    {
        _selectedItem = _selectedInventory.PickUpItem(tileGridPosition.x, tileGridPosition.y);
        _selectedItem.ItemData.PickUpItem();
        
        if(_selectedItem != null)
            _rectTransform = _selectedItem.GetComponent<RectTransform>();
    }

    private void ItemDrag()
    {
        if (_selectedItem != null)
        {
            var position = Input.mousePosition;

            position.x -= (_selectedItem.Width);
            position.y += (_selectedItem.Height);
            
            _rectTransform.position = position;

            if (_selectedInventory != null)
            {
                _selectedItem.transform.SetParent(_selectedInventory.GetComponent<RectTransform>());
            }
        }
    }

    private void LootingItem(string itemName)
    {
        GameObject pickUpItemInstance = GameObject.Instantiate(new GameObject { name = $"{itemName} Icon" });
        InventoryItem pickedUpItem = pickUpItemInstance.AddComponent<InventoryItem>();
        pickedUpItem.AddComponent<Image>();
        
        _rectTransform = pickedUpItem.transform as RectTransform;
        _rectTransform.SetParent(_canvasTransform);
        
        pickedUpItem.SetItemData(_itemData[itemName]);
        //pickedUpItem.name = pickedUpItem.ItemData.ItemName;
        pickedUpItem.ItemData.Init();
        
        _selectedItem = pickedUpItem;
    }

    // 아이템 드랍
    private void DropItemOnTheGround()
    {
        if (_selectedItem == null) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;
        
        var pos = Managers.Ray.RayHitPoint;
        if (Vector3.Distance(pos, Managers.Game.Player.transform.position) > 1f) return;
            
        pos.y = _selectedItem.ItemData.GroundYOffset;
        
        // 아이템 정보 얻기
        _dropItemModeling = _selectedItem.ItemData.ItemModelingForDropping;
        
        var droppedItem = GameObject.Instantiate(_dropItemModeling, pos, _dropItemModeling.transform.rotation);
        droppedItem.GetComponent<ItemNameTag>().ItemData = _selectedItem.ItemData;
        droppedItem.name = droppedItem.GetComponent<ItemNameTag>().ItemData.ItemName;
        
        // 아이템 프리펩 생성
        _selectedItem.ItemData.DropItem();
        _inventoryHighlight.Highlighter.SetParent(_canvasTransform);
        GameObject.Destroy(_selectedItem.gameObject);
        _selectedItem = null;
    }

    private void UseItem()
    {
        if (_itemToHighlight == null) return;
        
        _itemToHighlight.ItemData.UseItem();
    }
    
    public void DeleteItemInHighlight()
    {
        GameObject.Destroy(_itemToHighlight.gameObject);
        _itemToHighlight = null;
    }
    
    // 아이템 장착 등 현재 마우스에 쥐고 있는 아이템을 없애는 SendMassage
    public void MakeSelectedItemToNull()
    {
        _selectedItem = null;
        _rectTransform = null;
    }
    
    // 장착된 장비를 마우스로 옮기는 SendMassage
    public void GetItemFromEquipmentSlot(InventoryItem inventoryItem)
    {
        _selectedItem = inventoryItem;
        _rectTransform = _selectedItem.GetComponent<RectTransform>();
    }
}
