using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_SkillFloatingIcon : UI_Base, IPointerClickHandler
{
    public ActiveSkill Skill { get; set; }

    public void OnPointerClick(PointerEventData eventData)
    {
        Skill.StopSkill();
    }
}
