using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu]
public class ItemDataBackpack : ItemData
{
    [SerializeField] private GameObject _inventoryPrefab;
    [SerializeField] private GameObject _inventory;

    [SerializeField] private int _inventoryWidth;
    [SerializeField] private int _inventoryHeight;
    
    private ItemGrid _itemGrid;
    
    private bool _isOpen;
    
    public override void Init()
    {
        _isOpen = false;
        
        //가방 만들기
        //if (_inventory == null)
        {
            _inventory = Instantiate(_inventoryPrefab, Managers.Game.MainInventory.gameObject.transform.position + Vector3.up * 200, Quaternion.identity,
                Managers.Game.InventoryUICanvas.transform);
            _inventory.transform.SetAsFirstSibling();
            _inventory.GetComponentInChildren<Inventory_Sub>().Init(_inventoryWidth, _inventoryHeight);
            Managers.Game.InventoryList.Add(_inventory.GetComponentInChildren<Inventory_Sub>());
            _inventory.gameObject.SetActive(_isOpen);
        }
        
        Debug.Log("가방 만들어짐!");

    }
    public override void UseItem()
    {
        // 가방 열기 및 닫기
        _isOpen = !_isOpen;
        _inventory.gameObject.SetActive(_isOpen);
        Debug.Log("가방 사용됨!");
    }

    public override void DropItem()
    {
        Debug.Log("가방 버려짐!");
        Managers.Game.InventoryList.Remove(_inventory.GetComponentInChildren<Inventory_Sub>());
        Destroy(_inventory.gameObject);
    }

    public override void PickUpItem()
    {
        _isOpen = false;
        _inventory.gameObject.SetActive(_isOpen);
    }
}
