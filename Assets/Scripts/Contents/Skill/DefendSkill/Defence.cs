using System.Collections;
using System.Collections.Generic;
using Contents.Status;
using UnityEngine;

public class Defence : DefendSkill
{
    public Defence()
    {
        Def = 20;
        DefRatio = 30f;
        CoolTime = 5.0f;

        BonusStatus = new Status
        {
            Def = 2
        };

        SkillIcon = Resources.Load<Sprite>("Images/SkillIcons/Defence");

        SkillRank = Rank.F;
        MovingType = MoveType.Walk;
        SkillType = Type.Battle;

        SkillUseAnimClip =
            Resources.Load<AnimationClip>("Animation/Player/Skill/Defence/Defence");
    }

    public override void CastSkill()
    {
        base.CastSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();
    }

    public override void StopSkill()
    {
        base.StopSkill();
    }

    public override void RankUp()
    {
        base.RankUp();
    }
}
