using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendSkill : ActiveSkill
{
    [SerializeField] protected float Def;
    [SerializeField] protected float DefRatio;

    public float SkillDef => Def;
    public float SkillDefRatio => DefRatio;
}
