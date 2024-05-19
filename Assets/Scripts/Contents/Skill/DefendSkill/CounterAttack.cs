using System.Collections;
using System.Collections.Generic;
using Contents.Status;
using UnityEngine;

public class CounterAttack : DefendSkill
{
    public CounterAttack()
    {
        Def = 0;
        DefRatio = 100;
        
        CoolTime = 5.0f;
        
        BonusStatus = new Status
        {
            Str = 1,
            Dex = 2
        };

        SkillIcon = Resources.Load<Sprite>("Images/SkillIcons/CounterAttack");
        
        SkillRank = Rank.F;
        MovingType = MoveType.Stop;
        SkillType = Type.Battle;
        
        SkillCastAnimClip = null;
        SkillUseAnimClip = Resources.Load<AnimationClip>("Animation/Player/Skill/CounterAttack/CounterAttack");

        Managers.Game.Player.GetComponent<PlayerController>().Status += BonusStatus;
    }
}
