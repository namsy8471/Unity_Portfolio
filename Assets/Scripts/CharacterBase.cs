using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    // 기초 스테이터스
    protected float hp;
    protected float mp;
    protected float stamina;

    protected float str;
    protected float inteli;
    protected float dex;
    protected float will;
    protected float luck;

    protected float wholeDmg;
    protected float def;

    public float Hp
    {
        get => hp;
        set => hp = value;
    }

    public float Mp
    {
        get => mp;
        set => mp = value;
    }

    public float Stamina
    {
        get => stamina;
        set => stamina = value;
    }
}
