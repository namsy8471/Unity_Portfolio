using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendSkill : ActiveSkill
{
    protected float Def;
    protected float DefRatio;

    public float SkillDef => Def;
    public float SkillDefRatio => DefRatio;

    public override void StopSkill()
    {
        base.StopSkill();
        Managers.Game.Player.GetComponent<PlayerController>().GetDamageState.DefenceSkill = null;
    }
}
