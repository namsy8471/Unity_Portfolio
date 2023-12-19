using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillData : ScriptableObject
{
    [SerializeField] private Sprite skillIcon;
    [SerializeField] private float dmg;
    [SerializeField] private float dmgRatio;
    [SerializeField] private float coolTime;
    
    enum MoveType
    {
        Stop,
        Walk,
        Run,
        Rush
    }

    [SerializeField] private MoveType moveType;
    
    public virtual void UseSkill() {}
    public virtual void QuitSkill() {}
}
