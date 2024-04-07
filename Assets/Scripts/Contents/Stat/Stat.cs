using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Stat : MonoBehaviour
{
    // 기초 스테이터스
    [SerializeField]protected float _hp;
    [SerializeField]protected float _maxHp;
    [SerializeField]protected float _mp;
    [SerializeField]protected float _maxMp;
    [SerializeField]protected float _stamina;
    [SerializeField]protected float _maxStamina;

    [SerializeField]protected float _str;
    [SerializeField]protected float _int;
    [SerializeField]protected float _dex;
    [SerializeField]protected float _will;
    [SerializeField]protected float _luck;

    // 전투 스테이터스
    [SerializeField]protected float _minDmg;
    [SerializeField]protected float _maxDmg;
    [SerializeField]protected float _atkRange;
    [SerializeField]protected float _atkSpeed;
    [SerializeField]protected float _maxAtkCount;
    [SerializeField]protected float _def;

    [SerializeField]protected float _downGaugeFromHit;
    [SerializeField]protected float _downGaugeToHit;
    
    [SerializeField]protected float _moveSpeed;


    public float Hp
    {
        get => _hp;
        set
        {
            _hp = value;
            if (_hp >= MaxHp) _hp = MaxHp;
            Managers.Graphics.UI.ChangeSliderValue(Managers.Graphics.UI.HpBar, value);
        }
    }

    public float MaxHp
    {
        get => _maxHp;
        set
        {
            _maxHp = value;
            Hp = value;
        }
    }

    public float Mp
    {
        get => _mp;
        set
        {
            _mp = value;
            if (_mp >= MaxMp) _mp = MaxMp;
            Managers.Graphics.UI.ChangeSliderValue(Managers.Graphics.UI.MpBar, value);
        }
    }

    public float MaxMp
    {
        get => _maxMp;
        set
        {
            _maxMp = value;
            Mp = value;
        }
    }

    public float Stamina
    {
        get => _stamina;
        set
        {
            _stamina = value;
            if (_stamina >= MaxStamina) _stamina = MaxStamina;
            Managers.Graphics.UI.ChangeSliderValue(Managers.Graphics.UI.StaminaBar, value);
        }
    }

    public float MaxStamina
    {
        get => _maxStamina;
        set
        {
            _maxStamina = value;
            _stamina = value;
        }
    }

    public float Str
    {
        get => _str;
        set => _str = value;
    }
    
    public float Int
    {
        get => _int;
        set => _int = value;
    }
    
    public float Dex
    {
        get => _dex;
        set => _dex = value;
    }
    
    public float Will
    {
        get => _will;
        set => _will = value;
    }
    
    public float Luck
    {
        get => _luck;
        set => _luck = value;
    }
    
    public float MinDmg
    {
        get => _minDmg;
        set => _minDmg = value;
    }
    
    public float MaxDmg
    {
        get => _maxDmg;
        set => _maxDmg = value;
    }
    
    public float AtkRange
    {
        get => _atkRange;
        set => _atkRange = value;
    }

    public float AtkSpeed
    {
        get => _atkSpeed;
        set => _atkSpeed = value;
    }

    public float MaxAtkCount
    {
        get => _maxAtkCount;
        set
        {
            _maxAtkCount = value;
            DownGaugeToHit = (100 / _maxAtkCount) + 1;
        }
    }

    public float Def
    {
        get => _def;
        set => _def = value;
    }

    public float DownGaugeFromHit
    {
        get => _downGaugeFromHit;
        set => _downGaugeFromHit = value;
    }
    
    public float DownGaugeToHit
    {
        get => _downGaugeToHit;
        private set => _downGaugeToHit = value;
    }
    
    public float MoveSpeed
    {
        get => _moveSpeed;
        set => _moveSpeed = value;
    }
}
