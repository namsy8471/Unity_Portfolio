using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillDescription : MonoBehaviour, IPointerDownHandler
{
    public Skill SkillComponent { get; private set; }

    private Image _skillImage;
    private string _skillName;
    private string _skillRank;
    
    public void Init<T>(T skill) where T : Skill
    {
        SkillComponent = skill;
        
        _skillImage = transform.Find("SkillIcon").gameObject.GetComponent<Image>();
        _skillImage.sprite = SkillComponent.SkillIcon;

        _skillName = typeof(T).Name; 
        transform.Find("SkillName/SkillName").GetComponent<TextMeshProUGUI>().text = _skillName;

        transform.Find("RankText").GetComponent<TextMeshProUGUI>().text
            = "랭크 " + Enum.GetName(typeof(Skill.Rank), SkillComponent.SkillRank);

        GetComponentInChildren<Button>().interactable = (skill is ActiveSkill);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerEnter.name);

        switch (eventData.pointerEnter.name)
        {
            case "SkillIcon":
            {
                Managers.Game.SkillSystem.StartDragSkill(SkillComponent);
                break;
            }
            
            case "SkillName":
            {
                Debug.Log("Description Window Appear");
                break;
            }
            default:
                break;
        }
    }

    public void SkillUseButtonClick() => (SkillComponent as ActiveSkill)?.CastSkill();
}
