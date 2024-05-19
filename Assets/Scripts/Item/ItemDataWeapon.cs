using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu]
public class ItemDataWeapon : ItemDataEquipment
{
    public enum RangeType
    {
        None,
        Melee,
        Range
    }

    public enum AttackStyle
    {
        Punch,
        Sword,
        Bow,
        Wand
    }
    
    [SerializeField] private AttackStyle _atkStyle;
    [SerializeField] private int _minAtk;
    [SerializeField] private int _maxAtk;
    [SerializeField] private float _atkRange;
    [SerializeField] private float _atkSpeed;
    [SerializeField] private int _maxAtkCount;

    public AttackStyle AtkStyle => _atkStyle;
    public int MinAtk => _minAtk;
    public int MaxAtk => _maxAtk;
    public float AtkRange => _atkRange;
    public float AtkSpeed => _atkSpeed;
    public int MaxAtkCount => _maxAtkCount;
    
    [SerializeField] private RangeType _rangeType;
    
    public override void Init()
    {
        type = Type.Weapon;

        if (_rangeType == RangeType.Melee) _atkRange = 2.5f;
    }
}
