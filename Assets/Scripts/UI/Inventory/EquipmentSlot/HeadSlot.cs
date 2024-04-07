using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadSlot : EquipmentSlot
{
    void Start()
    {
        Init();
    }

    protected override void Init()
    {        
        base.Init();
        EquipmentSlotType = ItemDataEquipment.Type.Head;
        DefaultEquipmentGameObject = GameObject.Find("Hair");

        Managers.Game.PoolingManager.EquipmentDictionary[EquipmentSlotType].Add("Default", DefaultEquipmentGameObject);
    }
}
