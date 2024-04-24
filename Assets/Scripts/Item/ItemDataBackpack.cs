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
    
    public override void Init()
    {
        _inventory = Instantiate(_inventoryPrefab, Managers.Graphics.UI.MainInventory.gameObject.transform.position + Vector3.up * 200, Quaternion.identity,
            Managers.Graphics.UI.InventoryUICanvas.transform);
        _inventory.transform.SetAsFirstSibling();
        _inventory.GetComponentInChildren<Inventory_Sub>().Init(_inventoryWidth, _inventoryHeight);
        Managers.Graphics.UI.InventoryList.Add(_inventory.GetComponentInChildren<Inventory_Sub>());
        _inventory.gameObject.SetActive(false);
        
        Debug.Log("가방 만들어짐!");
    }
    
    public override void UseItem()
    {
        _inventory.gameObject.SetActive(!_inventory.gameObject.activeSelf);
        Debug.Log("가방 사용됨!");
    }

    public override void DropItem()
    {
        Debug.Log("가방 버려짐!");
        Managers.Graphics.UI.InventoryList.Remove(_inventory.GetComponentInChildren<Inventory_Sub>());
        Destroy(_inventory.gameObject);
    }

    public override void PickUpItem()
    {
        _inventory.gameObject.SetActive(false);
    }
}
