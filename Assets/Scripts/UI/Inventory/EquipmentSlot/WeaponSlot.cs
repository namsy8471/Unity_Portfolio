using System.Collections;
using System.Collections.Generic;
using Contents.Status;
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
        EquipmentSlotType = ItemDataEquipment.Type.Weapon;
        DefaultEquipmentGameObject = GameObject.Find("DefaultWeapon");
        
        Managers.Game.PoolingManager.EquipmentDictionary[EquipmentSlotType].Add("Default", DefaultEquipmentGameObject);
    }

    protected override void EquipItem(bool isSwitching)
    {
        base.EquipItem(isSwitching);

        var status = Managers.Game.Player.GetComponent<PlayerController>().Status;
        
        status.AtkStyle = (CurrentEquipmentItem.ItemData as ItemDataWeapon).AtkStyle;
        
        status.MinDmg = (CurrentEquipmentItem.ItemData as ItemDataWeapon).MinAtk;
        status.MaxDmg = (CurrentEquipmentItem.ItemData as ItemDataWeapon).MaxAtk;
        
        status.AtkRange = (CurrentEquipmentItem.ItemData as ItemDataWeapon).AtkRange;
        status.AtkSpeed = (CurrentEquipmentItem.ItemData as ItemDataWeapon).AtkSpeed;
        status.MaxAtkCount = (CurrentEquipmentItem.ItemData as ItemDataWeapon).MaxAtkCount;
    }

    protected override void UnequipItem(bool isSwitching)
    {
        var status = Managers.Game.Player.GetComponent<PlayerController>().Status;

        status.AtkStyle = ItemDataWeapon.AttackStyle.Punch;

        status.MinDmg = 5;
        status.MaxDmg = 10;
        
        status.AtkRange = 2.5f;
        status.AtkSpeed = 0.7f;
        status.MaxAtkCount = 3;
        
        base.UnequipItem(isSwitching);
    }
}
