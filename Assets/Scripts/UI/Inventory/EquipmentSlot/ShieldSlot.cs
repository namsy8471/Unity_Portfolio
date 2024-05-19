using System.Collections;
using System.Collections.Generic;
using Contents.Status;
using UnityEngine;

public class ShieldSlot : EquipmentSlot
{
    void Start()
    {
        Init();
    }

    protected override void Init()
    {        
        base.Init();
        EquipmentSlotType = ItemDataEquipment.Type.Shield;
        DefaultEquipmentGameObject = GameObject.Find("DefaultShield");

        Managers.Game.PoolingManager.EquipmentDictionary[EquipmentSlotType].Add("Default", DefaultEquipmentGameObject);
    }

    protected override void EquipItem(bool isSwitching)
    {
        base.EquipItem(isSwitching);
        
        var status = Managers.Game.Player.GetComponent<PlayerController>().Status;
        
        status.Def += (CurrentEquipmentItem.ItemData as ItemDataArmor).Def;
    }

    protected override void UnequipItem(bool isSwitching)
    {
        var status = Managers.Game.Player.GetComponent<PlayerController>().Status;

        status.Def -= (CurrentEquipmentItem.ItemData as ItemDataArmor).Def;
        
        base.UnequipItem(isSwitching);
    }
}