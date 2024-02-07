using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager
{
    private GameObject _player;

    public GameObject Player => _player;
    
    private GameObject _mainInventoryUI;
    private ItemGrid _itemGrid;
    private InventoryController _inventoryController = new InventoryController();
    private TargetingSystem _targetingSystem = new TargetingSystem();
    
    private bool isInventoryClosed = false;

    public Canvas MainInventoryUICanvas => _mainInventoryUI.GetComponent<Canvas>();
    public InventoryController InventoryController => _inventoryController;
    public TargetingSystem TargetingSystem => _targetingSystem;
    private DroppedItem droppedItem;

    public void Init()
    {
        _mainInventoryUI = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/Inventory/UI_MainInventory"));
        _itemGrid = _mainInventoryUI.GetComponentInChildren<ItemGrid>();
        _itemGrid.SetGrid(8, 10);

        _player = GameObject.FindWithTag("Player");
        
        _targetingSystem.Init();
        _inventoryController.Init();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isInventoryClosed = !isInventoryClosed;
            _mainInventoryUI.gameObject.SetActive(isInventoryClosed);
        }
        
        _targetingSystem.Update();
        _inventoryController.Update();
    }
}
