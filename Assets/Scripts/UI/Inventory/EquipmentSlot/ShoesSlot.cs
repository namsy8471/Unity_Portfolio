using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoesSlot : EquipmentSlot
{
    void Start()
    {
        Init();
    }

    protected override void Init()
    {        // TODO 각자 고유 Default 아이템을 넣기

        base.Init();
        EquipmentSlotType = ItemDataEquipment.Type.Shoes;
        DefaultEquipmentGameObject = GameObject.Find("Sword_1");

        Managers.Game.PoolingManager.EquipmentDictionary[EquipmentSlotType].Add("Default", DefaultEquipmentGameObject);
    }
}
