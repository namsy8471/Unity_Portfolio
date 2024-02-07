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

    // Start is called before the first frame update
    void Start()
    {
        Hp = 100;
        Mp = 50;
        Stamina = 50;

        Str = 20;
        Dex = 20;
        Int = 20;
        Will = 20;
        Luck = 20;

        Dmg = 20;
        AtkRange = 2.5f;
        Def = 10;
        
        
        
        MoveSpeed = 200.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
