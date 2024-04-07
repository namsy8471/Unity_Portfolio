using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager
{

    #region Equipment Pool
    
    private Dictionary<ItemDataEquipment.Type, Dictionary<string, GameObject>> _equipmentDictionary;
    
    private Dictionary<string, GameObject> _weaponPool;
    private Dictionary<string, GameObject> _bodyArmorPool;
    private Dictionary<string, GameObject> _shieldPool;
    private Dictionary<string, GameObject> _shoesPool;
    private Dictionary<string, GameObject> _headGearPool;
    
    public Dictionary<ItemDataEquipment.Type, Dictionary<string, GameObject>> EquipmentDictionary =>
        _equipmentDictionary;
    public Dictionary<string, GameObject> WeaponPool => _weaponPool;
    public Dictionary<string, GameObject> BodyArmorPool => _bodyArmorPool;
    public Dictionary<string, GameObject> ShieldPool => _shieldPool;
    public Dictionary<string, GameObject> ShoesPool => _shoesPool;
    public Dictionary<string, GameObject> HeadGearPool => _headGearPool;
    
    #endregion
    
    public void Init()
    {
        _equipmentDictionary = new Dictionary<ItemDataEquipment.Type, Dictionary<string, GameObject>>();
        _weaponPool = new Dictionary<string, GameObject>();
        _bodyArmorPool = new Dictionary<string, GameObject>();
        _shieldPool = new Dictionary<string, GameObject>();
        _shoesPool = new Dictionary<string, GameObject>();
        _headGearPool = new Dictionary<string, GameObject>();

        _equipmentDictionary.Add(ItemDataEquipment.Type.Weapon, _weaponPool);
        _equipmentDictionary.Add(ItemDataEquipment.Type.Body, _bodyArmorPool);
        _equipmentDictionary.Add(ItemDataEquipment.Type.Shield, _shieldPool);
        _equipmentDictionary.Add(ItemDataEquipment.Type.Shoes, _shoesPool);
        _equipmentDictionary.Add(ItemDataEquipment.Type.Head, _headGearPool);
    }

    public void SwitchEquipmentModeling(ItemDataEquipment.Type type, ItemDataEquipment currentEquipment, ItemDataEquipment newEquipment)
    {
        SetActiveCurrentEquipmentFalse(type, currentEquipment);
        SetActiveNewEquipmentTrue(type, newEquipment);
    }
    
    private void SetActiveCurrentEquipmentFalse(ItemDataEquipment.Type type, ItemDataEquipment currentEquipment)
    {
        if (currentEquipment == null)
        {
            EquipmentDictionary[type]["Default"].SetActive(false);
        }
        
        else
        {
            EquipmentDictionary[type][currentEquipment.ItemName].SetActive(false);
        }
    }

    private void SetActiveNewEquipmentTrue(ItemDataEquipment.Type type, ItemDataEquipment newEquipment)
    {
        if (newEquipment == null)
        {
            EquipmentDictionary[type]["Default"].SetActive(true);   
        }
        else
        {
            if (EquipmentDictionary[type].ContainsKey(newEquipment.ItemName))
            {
                EquipmentDictionary[type][newEquipment.ItemName].SetActive(true);
            }
            else
            {
                GameObject newWeaponGameObject = GameObject.Instantiate(newEquipment.ItemModelingForEquipment,
                    EquipmentDictionary[type]["Default"].transform.parent);
                EquipmentDictionary[type].Add(newEquipment.ItemName, newWeaponGameObject);
            }
        }
    }
}
