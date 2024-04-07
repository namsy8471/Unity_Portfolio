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
        EquipmentSlotType = ItemDataEquipment.Type.Weapon;
        DefaultEquipmentGameObject = GameObject.Find("DefaultWeapon");
        
        Managers.Game.PoolingManager.EquipmentDictionary[EquipmentSlotType].Add("Default", DefaultEquipmentGameObject);
    }

    protected override void EquipItem(bool isSwitching)
    {
        base.EquipItem(isSwitching);
        
        (Managers.Game.Player.GetComponent<Stat>() as PlayerStat).AtkStyle =
            (CurrentEquipmentItem.ItemData as ItemDataWeapon).AtkStyle;
        
        Managers.Game.Player.GetComponent<Stat>().MinDmg = 
            (CurrentEquipmentItem.ItemData as ItemDataWeapon).MinAtk;
        
        Managers.Game.Player.GetComponent<Stat>().MaxDmg = 
            (CurrentEquipmentItem.ItemData as ItemDataWeapon).MaxAtk;
        
        Managers.Game.Player.GetComponent<Stat>().AtkRange = 
            (CurrentEquipmentItem.ItemData as ItemDataWeapon).AtkRange;
        
        Managers.Game.Player.GetComponent<Stat>().AtkSpeed = 
            (CurrentEquipmentItem.ItemData as ItemDataWeapon).AtkSpeed;
        
        Managers.Game.Player.GetComponent<Stat>().MaxAtkCount = 
            (CurrentEquipmentItem.ItemData as ItemDataWeapon).MaxAtkCount;
    }

    protected override void UnequipItem(bool isSwitching)
    {
        (Managers.Game.Player.GetComponent<Stat>() as PlayerStat).AtkStyle = ItemDataWeapon.AttackStyle.Punch;

        Managers.Game.Player.GetComponent<Stat>().MinDmg = 5f;
        Managers.Game.Player.GetComponent<Stat>().MaxDmg = 10f;
        
        Managers.Game.Player.GetComponent<Stat>().AtkRange = 2.5f;
        Managers.Game.Player.GetComponent<Stat>().AtkSpeed = 0.7f;
        Managers.Game.Player.GetComponent<Stat>().MaxAtkCount = 3;
        
        base.UnequipItem(isSwitching);
    }
}
