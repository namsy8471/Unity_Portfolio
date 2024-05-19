using System.Collections;
using System.Collections.Generic;
using Contents.Status;
using UnityEngine;
using UnityEngine.EventSystems;

public class Skill
{
    protected Skill() { }

    public enum Rank
    {
        F = 1,
        E,
        D,
        C,
        B,
        A
    }

    public enum Type
    {
        Magic,
        Battle
    }

    private Rank _skillRank;
    private Type _skillType;
    private Status _bonusStatus;
    private Sprite _skillIcon;

    public Rank SkillRank { get => _skillRank; protected set => _skillRank = value; }
    public Type SkillType { get => _skillType; protected set => _skillType = value; }
    public Status BonusStatus { get => _bonusStatus; protected set => _bonusStatus = value; }
    public Sprite SkillIcon { get => _skillIcon; protected set => _skillIcon = value; }
    public virtual void RankUp() {}
}
