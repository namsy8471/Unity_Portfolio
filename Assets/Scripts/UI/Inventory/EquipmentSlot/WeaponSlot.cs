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
        
        (Managers.Game.Player.GetComponent<Status>() as PlayerStatus).AtkStyle =
            (CurrentEquipmentItem.ItemData as ItemDataWeapon).AtkStyle;
        
        Managers.Game.Player.GetComponent<Status>().MinDmg = 
            (CurrentEquipmentItem.ItemData as ItemDataWeapon).MinAtk;
        
        Managers.Game.Player.GetComponent<Status>().MaxDmg = 
            (CurrentEquipmentItem.ItemData as ItemDataWeapon).MaxAtk;
        
        Managers.Game.Player.GetComponent<Status>().AtkRange = 
            (CurrentEquipmentItem.ItemData as ItemDataWeapon).AtkRange;
        
        Managers.Game.Player.GetComponent<Status>().AtkSpeed = 
            (CurrentEquipmentItem.ItemData as ItemDataWeapon).AtkSpeed;
        
        Managers.Game.Player.GetComponent<Status>().MaxAtkCount = 
            (CurrentEquipmentItem.ItemData as ItemDataWeapon).MaxAtkCount;
    }

    protected override void UnequipItem(bool isSwitching)
    {
        (Managers.Game.Player.GetComponent<Status>() as PlayerStatus).AtkStyle = ItemDataWeapon.AttackStyle.Punch;

        Managers.Game.Player.GetComponent<Status>().MinDmg = 5f;
        Managers.Game.Player.GetComponent<Status>().MaxDmg = 10f;
        
        Managers.Game.Player.GetComponent<Status>().AtkRange = 2.5f;
        Managers.Game.Player.GetComponent<Status>().AtkSpeed = 0.7f;
        Managers.Game.Player.GetComponent<Status>().MaxAtkCount = 3;
        
        base.UnequipItem(isSwitching);
    }
}
