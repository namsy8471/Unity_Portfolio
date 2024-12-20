using System.Collections;
using System.Collections.Generic;
using Contents.Status;
using UnityEngine;

public class Smash : AttackSkill
{
    private int _maxAtkCountOrigin;
    
    public Smash()
    {
        Damage = 0;
        DamageRatio = 4.5f;
        CoolTime = 5.0f;
        Range = 2.5f;
        DownGauge = 101f;
        
        BonusStatus = new Status
        {
            Str = 3
        };

        SkillIcon = Resources.Load<Sprite>("Images/SkillIcons/Smash");
        
        SkillRank = Rank.F;
        MovingType = MoveType.Run;
        SkillType = Type.Battle;
        
        SkillCastAnimClip = null;
        SkillUseAnimClip = Resources.Load<AnimationClip>("Animation/Player/Skill/Smash/Smash");
        
        Managers.Game.Player.GetComponent<PlayerController>().Status += BonusStatus;
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
