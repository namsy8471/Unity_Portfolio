using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class ItemDataPotion : ItemData
{
    public override void Init()
    {
        
    }

    public override void UseItem()
    {
        Debug.Log("HP 50 증가!");
        Managers.Game.Player.GetComponent<PlayerController>().Status.MaxHp += 50;
        InventoryController.DeleteItemInHighlight();
    }

    public override void DropItem()
    {
        Debug.Log("물약 버려짐");
    }
}
