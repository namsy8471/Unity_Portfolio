using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemDataShoes : ItemDataArmor
{
    public override void Init()
    {
        type = Type.Shoes;
    }
}
