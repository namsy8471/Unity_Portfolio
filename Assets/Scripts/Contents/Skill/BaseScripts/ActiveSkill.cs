using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ActiveSkill : Skill
{
    public enum MoveType
    {
        Stop,
        Walk,
        Run
    }

    public AnimationClip SkillCastAnimClip { get; protected set; }
    public AnimationClip SkillUseAnimClip { get; protected set; }
    public float CoolTime { get; protected set; }
    public MoveType MovingType { get; protected set; }
    

    public virtual void CastSkill()
    {
        if (SkillCastAnimClip != null)
        {
            Managers.Game.Player.GetComponentInChildren<Animator>().Play(SkillCastAnimClip.name);
        }
        
        Managers.Graphics.Visual.DrawSkillFloatingIcon(this);
        Managers.Game.Player.GetComponent<PlayerController>().CurrentSkill = this;
    }
    
    public virtual void UseSkill()
    {
        Managers.Game.Player.GetComponentInChildren<Animator>().applyRootMotion = true;
        Managers.Game.Player.GetComponentInChildren<Animator>().Play(SkillUseAnimClip.name);
        //Invoke("SetRootAnimationFalse", SkillUseAnimClip.length);
        StopSkill();
    }
    
    public void SetRootAnimationFalse()
    {
        Managers.Game.Player.GetComponentInChildren<Animator>().applyRootMotion = false;
    }
    
    public virtual void StopSkill()
    {
        Managers.Game.Player.GetComponent<PlayerController>().CurrentSkill = null;
        Managers.Graphics.Visual.ClearSkillFloatingIcon();
    }
}
