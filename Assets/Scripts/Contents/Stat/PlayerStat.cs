using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    private int _exp;
    private int _gold;

    public int Exp
    {
        get => _exp;
        set
        {
            _exp = value;
        }
    }

    [SerializeField] private ItemDataWeapon.AttackStyle _attackStyle;

    public ItemDataWeapon.AttackStyle AtkStyle
    {
        get => _attackStyle;
        set => _attackStyle = value;
    }
    
    void Start()
    {
        MaxHp = 100;
        MaxMp = 50;
        MaxStamina = 50;

        Str = 20;
        Dex = 20;
        Int = 20;
        Will = 20;
        Luck = 20;

        AtkStyle = ItemDataWeapon.AttackStyle.Punch;
        
        MinDmg = 5;
        MaxDmg = 10;
        
        AtkRange = 2.5f;
        AtkSpeed = 0.7f;
        
        MaxAtkCount = 3;
        Def = 10;

        DownGaugeFromHit = 0;
        
        MoveSpeed = 200.0f;

        Managers.Graphics.UI.BindingSliderWithPlayerStatus();
    }

}
