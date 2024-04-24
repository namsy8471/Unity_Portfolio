using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSkill : ActiveSkill
{
    protected float Damage;
    protected float DamageRatio;
    protected float Range;
    
    public float SkillDamage => Damage;
    public float SkillDamageRatio => DamageRatio;
    public float SkillRange => Range;
}
