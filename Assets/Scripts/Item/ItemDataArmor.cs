using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataArmor : ItemDataEquipment
{
    [SerializeField] protected int def;

    public int Def => def;
}
