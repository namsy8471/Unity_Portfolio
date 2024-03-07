using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    private GameObject _player;
    
    private Inventory_Main _mainInventory;
    private Canvas _InventoryUI;
    
    private ItemGrid _itemGrid;

    private List<Inventory_Base> _inventories = new List<Inventory_Base>();
    
    private InventoryController _inventoryController = new InventoryController();
    private TargetingSystem _targetingSystem = new TargetingSystem();
    
    private bool isInventoryClosed = false;

    public GameObject Player => _player;
    public Inventory_Main MainInventory => _mainInventory;
    public Canvas InventoryUICanvas => _InventoryUI;
    public List<Inventory_Base> InventoryList => _inventories;
    public InventoryController InventoryController => _inventoryController;
    public TargetingSystem TargetingSystem => _targetingSystem;
    
    public void Init()
    {
        _InventoryUI = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/Inventory/UI_MainInventory"))
            .GetComponent<Canvas>();
        
        _mainInventory = _InventoryUI.GetComponentInChildren<Inventory_Main>();
        _mainInventory.Init(8, 10);
        
        _inventories.Add(_mainInventory);
        
        _player = GameObject.FindWithTag("Player");

        Managers.Input.KeyButtonPressed.Add(Managers.Input.InventoryKey, CloseOrOpenInventory);
        _targetingSystem.Init();
        _inventoryController.Init();
    }

    public void Update()
    {
        // if (Input.GetKeyDown(KeyCode.I))
        // {
        //     CloseOrOpenInventory(_keyCode);
        // }
        
        _targetingSystem.Update();
        _inventoryController.Update();
    }

    private void CloseOrOpenInventory()
    {
        isInventoryClosed = !isInventoryClosed;
        foreach (var inven in _inventories)
        {
            inven.gameObject.transform.root.gameObject.SetActive(isInventoryClosed);
        }
    }
}
