using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class ItemDataPotion : ItemData
{
    private InventoryController inventoryController;
    public override void Init()
    {
        inventoryController = Managers.Game.InventoryController;
    }

    public override void UseItem()
    {
        Debug.Log("HP 회복!");
        inventoryController.DeleteItemInHighlight();
    }

    public override void DropItem()
    {
        Debug.Log("물약 버려짐");
    }
}
