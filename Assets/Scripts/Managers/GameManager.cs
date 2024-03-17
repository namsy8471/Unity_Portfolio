using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    private GameObject _player;
    
    private Inventory_Main _mainInventory;
    private Canvas _inventoryUI;
    
    private ItemGrid _itemGrid;

    private List<Inventory_Base> _inventories = new List<Inventory_Base>();
    
    private InventoryController _inventoryController = new InventoryController();
    private TargetingSystem _targetingSystem = new TargetingSystem();
    
    private bool _isInventoryClosed = false;

    public GameObject Player => _player;
    public Inventory_Main MainInventory => _mainInventory;
    public Canvas InventoryUICanvas => _inventoryUI;
    public List<Inventory_Base> InventoryList => _inventories;
    public InventoryController InventoryController => _inventoryController;
    public TargetingSystem TargetingSystem => _targetingSystem;
    
    public void Init()
    {
        _inventoryUI = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/Inventory/UI_MainInventory"))
            .GetComponent<Canvas>();
        
        _mainInventory = _inventoryUI.GetComponentInChildren<Inventory_Main>();
        _mainInventory.Init();
        
        _inventories.Add(_mainInventory);
        
        _player = GameObject.FindWithTag("Player");

        Managers.Input.AddAction(Managers.Input.KeyButtonDown, Managers.Input.InventoryKey, CloseOrOpenInventory);
        
        _targetingSystem.Init();
        _inventoryController.Init();
    }

    public void Update()
    {
        _targetingSystem.Update();
        _inventoryController.Update();
    }

    private void CloseOrOpenInventory()
    {
        _isInventoryClosed = !_isInventoryClosed;
        foreach (var inven in _inventories)
        {
            inven.gameObject.transform.parent.gameObject.SetActive(_isInventoryClosed);
            Managers.Input.RemovePlayerMouseActions();
            Managers.Input.RollbackPlayerMouseActions();
        }
    }
}
