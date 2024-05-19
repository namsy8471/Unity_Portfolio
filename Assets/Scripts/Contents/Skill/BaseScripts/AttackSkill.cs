using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSkill : ActiveSkill
{
    protected float Damage;
    protected float DamageRatio;
    protected float Range;
    protected float DownGauge;
    
    public float SkillDamage => Damage;
    public float SkillDamageRatio => DamageRatio;
    public float SkillRange => Range;
    public float SkillDownGauge => DownGauge;

    public override void StopSkill()
    {
        base.StopSkill();
        Managers.Game.Player.GetComponent<PlayerController>().AttackState.Skill = null;
    }
}
