using System;
using UnityEngine;

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

    public float Timer { get; set; } = 0;

    public virtual void CastSkill()
    {
        var player = Managers.Game.Player.GetComponent<PlayerController>();
        
        if (player.CurrentSkill != null) return;

        if (player.Status.Stamina < 7) return;

        player.Status.Stamina -= 7;
        if (SkillCastAnimClip != null)
        {
            player.GetComponent<Animator>().Play(SkillCastAnimClip.name);
        }
        
        Managers.Graphics.Visual.DrawSkillFloatingIcon(this);
        player.CurrentSkill = this;
        
        switch (MovingType)
        {
            case MoveType.Stop:
                player.IdleState.CanMove = false;
                player.IdleState.MoveHorizontal = 0;
                player.IdleState.MoveVertical = 0;
                
                player.IdleState.ChangeToInputTypeNothing();
                
                break;
            case MoveType.Walk:
                
                player.IdleState.ChangeToWalk();
                player.IdleState.FixedToWalk = true;
                Managers.Input.RemoveAction(Managers.Input.KeyButtonPressed, Managers.Input.WalkKey, player.IdleState.ChangeToWalk);
                Managers.Input.RemoveAction(Managers.Input.KeyButtonUp, Managers.Input.WalkKey, player.IdleState.ChangeToRun);
                
                break;
            case MoveType.Run:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        Managers.Input.AddAction(Managers.Input.KeyButtonDown, Managers.Input.SkillQuitKey, StopSkill);

    }
    
    public virtual void UseSkill()
    {
        Managers.Game.Player.GetComponentInChildren<Animator>().Play(SkillUseAnimClip.name);
    }
    
    public virtual void StopSkill()
    {
        Debug.Log("MoveType = " + MovingType);
        
        var player = Managers.Game.Player.GetComponent<PlayerController>();
        
        switch (MovingType)
        {
            case MoveType.Stop:
                player.IdleState.CanMove = true;
                player.IdleState.DestPos = player.transform.position;
                break;
            case MoveType.Walk:
                
                Managers.Input.RemoveAction(Managers.Input.KeyButtonPressed, Managers.Input.WalkKey, Managers.Game.Player.GetComponent<PlayerController>().IdleState.ChangeToWalk);
                Managers.Input.RemoveAction(Managers.Input.KeyButtonUp, Managers.Input.WalkKey, Managers.Game.Player.GetComponent<PlayerController>().IdleState.ChangeToRun);

                Managers.Input.AddAction(Managers.Input.KeyButtonPressed, Managers.Input.WalkKey, Managers.Game.Player.GetComponent<PlayerController>().IdleState.ChangeToWalk);
                Managers.Input.AddAction(Managers.Input.KeyButtonUp, Managers.Input.WalkKey, Managers.Game.Player.GetComponent<PlayerController>().IdleState.ChangeToRun);

                player.IdleState.FixedToWalk = false;
                player.IdleState.ChangeToRun();
                
                break;
            case MoveType.Run:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        Managers.Input.RemoveAction(Managers.Input.KeyButtonDown, Managers.Input.SkillQuitKey, StopSkill);

        Managers.Graphics.Visual.ClearSkillFloatingIcon();
        Managers.Game.Player.GetComponent<PlayerController>().CurrentSkill = null;
    }

    public void CoolTimeUpdate()
    {
        if (Timer <= CoolTime) Timer += Time.deltaTime;
    }
}
