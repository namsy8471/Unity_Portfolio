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
        inventoryController = FindObjectOfType(typeof(InventoryController)) as InventoryController;
    }

    public override void UseItem()
    {
        Debug.Log("HP 회복!");
        inventoryController.SendMessage("DeleteItemInHighlight", SendMessageOptions.DontRequireReceiver);
    }

    public override void DropItem()
    {
        Debug.Log("물약 버려짐");
    }
}
