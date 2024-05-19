using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillBox : UI_Base
{
    private Skill _skill;
    private Image _skillIcon;

    private void Start()
    {
        _skill = null;
        _skillIcon = transform.GetChild(0).GetComponentInChildren<Image>();

        var str = name;
        Debug.Log(str);
        
        var num = int.Parse(str.Replace("SkillBox", ""));
        Managers.Input.AddAction(Managers.Input.KeyButtonDown, (KeyCode.F1 + num - 1), CastSkill);
    }

    private void SetSkillBox(Skill skill)
    {
        _skill = skill;
        _skillIcon.sprite = _skill.SkillIcon;
        _skillIcon.color = new Color(255f, 255, 255, 1);
    }

    private void DeleteSkill()
    {
        _skill = null;
        _skillIcon.sprite = null;
        _skillIcon.color = new Color(255, 255, 255, 0.03f);
    }
    
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Right:
                DeleteSkill();
                break;
            case PointerEventData.InputButton.Left:
            {
                _skillIcon.gameObject.transform.localScale = new Vector3(0.9f, 0.9f);

                if (_skill == null)
                {
                    var newSkill = Managers.Game.SkillSystem.CurrentSelectSkill;
                    SetSkillBox(newSkill);
                    Managers.Game.SkillSystem.StopDragSkill();
                }
                else
                {
                    CastSkill();
                }

                break;
            }
        }
    }

    private void CastSkill()
    {
        if (_skill is not ActiveSkill activeSkill) return;

        activeSkill.CastSkill();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        _skillIcon.gameObject.transform.localScale = new Vector3(1.0f, 1.0f);
    }
}
