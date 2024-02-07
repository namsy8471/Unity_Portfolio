using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stat : MonoBehaviour
{
    // 기초 스테이터스
    [SerializeField]protected float _hp;
    [SerializeField]protected float _mp;
    [SerializeField]protected float _stamina;

    [SerializeField]protected float _str;
    [SerializeField]protected float _int;
    [SerializeField]protected float _dex;
    [SerializeField]protected float _will;
    [SerializeField]protected float _luck;

    [SerializeField]protected float _wholeDmg;
    [SerializeField]protected float _atkRange;
    [SerializeField]protected float _def;

    [SerializeField]protected float _moveSpeed;

    public float Hp
    {
        get => _hp;
        set => _hp = value;
    }

    public float Mp
    {
        get => _mp;
        set => _mp = value;
    }

    public float Stamina
    {
        get => _stamina;
        set => _stamina = value;
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
    
    public float Dmg
    {
        get => _wholeDmg;
        set => _wholeDmg = value;
    }

    public float AtkRange
    {
        get => _atkRange;
        set => _atkRange = value;
    }
    
    public float Def
    {
        get => _def;
        set => _def = value;
    }

    public float MoveSpeed
    {
        get => _moveSpeed;
        set => _moveSpeed = value;
    }
}
