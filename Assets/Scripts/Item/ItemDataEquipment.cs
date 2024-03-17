using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemDataEquipment : ItemData
{
    public enum Type
    {
        None,
        Head,
        Weapon,
        Shield,
        Shoes,
        Body
    }

    // 장비 종류
    [SerializeField] private Type type;
    public Type EquipmentType => type;
    
    // 무기 능력치
    private float minAtk;
    private float maxAtk;
    private float def;
    private float durability;
    
    public override void Init()
    {
        switch (type)
        {
            case Type.Weapon:
                def = 0;
                break;
            
            case Type.Shield:
            case Type.Head:
            case Type.Shoes:
            case Type.Body:
                maxAtk = 0;
                minAtk = 0;
                break;

            case Type.None:
            default:
                break;
        }
    }

    public override void UseItem()
    {
        EquipItem();
    }
    
    private void EquipItem()
    {
        switch (type)
        {
            case Type.Head:
                
                break;
            case Type.Weapon:
                break;
            case Type.Shield:
                break;
            case Type.Shoes:
                break;
            case Type.Body:
                break;
            
            case Type.None:
            default:
                break;
        }
    }

}
