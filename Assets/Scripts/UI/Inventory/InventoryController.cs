using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class InventoryController
{
    private ItemGrid _selectedItemGrid;

    public ItemGrid SelectedItemGrid
    {
        get => _selectedItemGrid;
        set
        {
            _selectedItemGrid = value; 
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
    private Transform _canvasTransform;
    
    private InventoryHighlight _inventoryHighlight = new InventoryHighlight();
    private InventoryItem _itemToHighlight;
    private Vector2Int _oldPosition;
    
    private Action _changeCursorForGrab;
    private Action _changeCursorForGrabbing;
    private Action _changeCursorForNormal;
    
    private GraphicRaycaster gr;
    
    public void Init()
    {
        _itemData.Add("Backpack", Resources.Load<ItemData>("ItemData/Backpack"));
        _itemData.Add("HpPotion", Resources.Load<ItemData>("ItemData/HpPotion"));
        _itemData.Add("LongSword", Resources.Load<ItemData>("ItemData/LongSword"));
        _itemData.Add("ShortSword", Resources.Load<ItemData>("ItemData/ShortSword"));
        
        _changeCursorForGrab += Managers.Cursor.ChangeCursorForGrab;
        _changeCursorForGrabbing += Managers.Cursor.ChangeCursorForGrabbing;
        _changeCursorForNormal += Managers.Cursor.BackNormalCursor;
        
        _inventoryHighlight.Init();
        
        _canvasTransform = Managers.Game.MainInventoryUICanvas.transform;
    }

    public void Update()
    {
        LootingItemFromGround();
        ItemDrag();
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(_selectedItem == null)
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
        
        if (!_selectedItemGrid)
        {
            _inventoryHighlight.Show(false);
            if (Input.GetMouseButtonDown(0))
            {
                if(_selectedItem != null) 
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

    private void LootingItemFromGround()
    {
        if (_selectedItem == null && EventSystem.current.IsPointerOverGameObject() == false)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, 1 << LayerMask.NameToLayer("Item")))
            {
                //TODO 아이템 루팅 픽업 만들기
                
                if(Vector3.Distance(hit.point, Managers.Game.Player.transform.position) < 1.0f)
                    LootingItem(hit.transform.name);
                
                if(Input.GetMouseButton(0))
                    _changeCursorForGrabbing?.Invoke();
                else
                    _changeCursorForGrab?.Invoke();
            }
            else
            {
                _changeCursorForNormal?.Invoke();
            }
        }
    }

    private void RotateItem()
    {
        if (_selectedItem == null)
        {
            return;
        }

        _selectedItem.Rotate();
    }

    private void InsertRandomItem()
    {
        if (_selectedItemGrid == null)
        {
            return;
        }
        
        CreateRandomItem();
        InventoryItem itemToInsert = _selectedItem;
        _selectedItem = null;
        InsertItem(itemToInsert);
    }

    private void InsertItem(InventoryItem itemToInsert)
    {
        Vector2Int? posOnGrid = _selectedItemGrid.FindSpaceForObject(itemToInsert);
        
        if(posOnGrid == null) return;

        _selectedItemGrid.PlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
    }

    private void HandleHighlight()
    {
        Vector2Int positionOnGrid = GetTileGridPosition();
        if (_oldPosition == positionOnGrid) return;

        _oldPosition = positionOnGrid;
        
        if (_selectedItem == null)
        {
            _itemToHighlight = _selectedItemGrid.GetItem(positionOnGrid.x, positionOnGrid.y);

            if (_itemToHighlight != null)
            {
                _inventoryHighlight.Show(true);
                _inventoryHighlight.SetSize(_itemToHighlight);
                _inventoryHighlight.SetParent(_selectedItemGrid);
                _inventoryHighlight.SetPosition(_selectedItemGrid,_itemToHighlight);
            }
            else
            {
                _inventoryHighlight.Show(false);
            }
        }
        else
        {
            _inventoryHighlight.Show(_selectedItemGrid.BoundaryCheck(
                positionOnGrid.x,
                positionOnGrid.y,
                _selectedItem.Width,
                _selectedItem.Height
                )
            );
            _inventoryHighlight.SetSize(_selectedItem);
            _inventoryHighlight.SetParent(_selectedItemGrid);
            _inventoryHighlight.SetPosition(_selectedItemGrid, _selectedItem, positionOnGrid.x, positionOnGrid.y);
        }
    }

    private void CreateRandomItem()
    {
        InventoryItem inventoryItem = GameObject.Instantiate(_itemPrefab).AddComponent<InventoryItem>();
        _selectedItem = inventoryItem;
        
        _rectTransform = inventoryItem.GetComponent<RectTransform>();
        _rectTransform.SetParent(_canvasTransform);
        _rectTransform.SetAsLastSibling();

        var randomItemID = Random.Range(0, _items.Count);
        inventoryItem.SetItemData(_items[randomItemID]);
    }

    private void LeftMouseButtonPress()
    {
        var tileGridPosition = GetTileGridPosition();

        Debug.Log(tileGridPosition);
        
        if (_selectedItem == null)
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

        if (_selectedItem != null)
        {
            position.x -= (_selectedItem.Width) * ItemGrid.TileSize.Width / 2;
            position.y += (_selectedItem.Height) * ItemGrid.TileSize.Height / 2;
        }

        Vector2Int tileGridPosition = _selectedItemGrid.GetTileGridPosition(position);
        return tileGridPosition;
    }

    private void PlaceItem(Vector2Int tileGridPosition)
    {
        bool result = _selectedItemGrid.PlaceItem(_selectedItem, tileGridPosition.x, tileGridPosition.y, ref _overlapItem);
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
        _selectedItem = _selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
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

            if (_selectedItemGrid)
            {
                _selectedItem.transform.SetParent(_selectedItemGrid.transform);
            }
        }
    }
    
    // 아이템 줍기
    public void LootingItem(string itemName)
    {
        InventoryItem pickedUpItem = GameObject.Instantiate(new GameObject {name = $"{itemName}Icon"}).AddComponent<InventoryItem>();
        pickedUpItem.AddComponent<Image>();
        _selectedItem = pickedUpItem;
        
        _rectTransform = pickedUpItem.transform as RectTransform;
        _rectTransform.SetParent(_canvasTransform);
        //_rectTransform.SetAsLastSibling();
        
        pickedUpItem.SetItemData(_itemData[itemName]);
        pickedUpItem.name = pickedUpItem.ItemData.ItemName;
        pickedUpItem.ItemData.Init();
    }

    // 아이템 드랍
    private void DropItemOnTheGround()
    {
        if (_selectedItem == null) return;
        
        // UI 레이캐스팅
        var results = GraphicRaycastResults();
        if (results.Count > 0) return;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        var pos = Vector3.zero;

        if (Physics.Raycast(ray, out hit, 100f, 1 << LayerMask.NameToLayer("Ground")))
            pos = hit.point;

        if (Vector3.Distance(pos, GameObject.Find("Player").transform.position) > 1f) return;
            
        pos.y = _selectedItem.ItemData.GroundYOffset;
        
        // 아이템 정보 얻기
        _dropItemModeling = _selectedItem.ItemData.ItemModelingForDropping;
        
        var droppedItem = GameObject.Instantiate(_dropItemModeling, pos, _dropItemModeling.transform.rotation);
        droppedItem.GetComponent<DroppedItem>().ItemData = _selectedItem.ItemData;
        
        // 아이템 프리펩 생성
        _selectedItem.ItemData.DropItem();
        _inventoryHighlight.Highlighter.SetParent(_canvasTransform);
        GameObject.Destroy(_selectedItem.gameObject);
        _selectedItem = null;
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
    public void UseItem()
    {
        _itemToHighlight.ItemData.UseItem();
    }
    
    // 아이템 삭제용 샌드메시지
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
