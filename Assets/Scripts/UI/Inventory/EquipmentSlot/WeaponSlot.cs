using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlot : EquipmentSlot
{
    void Start()
    {
        Init();
    }

    protected override void Init()
    {
        base.Init();
        equipmentSlotType = ItemDataEquipment.Type.Weapon;
        defaultEquipmentGameObject = GameObject.Find("Sword_1");
    }
}
