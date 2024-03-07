using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private InventoryController _inventoryController;
    private Inventory_Base _inventory;
    
    private void Start()
    {
        _inventoryController = Managers.Game.InventoryController;
        _inventory = GetComponent<Inventory_Base>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _inventoryController.SelectedInventory = _inventory;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _inventoryController.SelectedInventory = null;
    }
}
