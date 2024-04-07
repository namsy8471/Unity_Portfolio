using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataArmor : ItemDataEquipment
{
    [SerializeField] protected float def;

    public float Def => def;
}
