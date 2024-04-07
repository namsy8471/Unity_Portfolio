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

    protected Type type;
    public Type EquipmentType => type;
    
    public float Durability { get; set; }
    
}
