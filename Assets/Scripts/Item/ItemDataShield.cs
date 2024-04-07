using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemDataShield : ItemDataArmor
{
    
    public override void Init()
    {
        type = Type.Shield;
    }
}
