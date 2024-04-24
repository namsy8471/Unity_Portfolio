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

        var isValid = skill as ActiveSkill;
        GetComponentInChildren<Button>().interactable = (isValid != null);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerEnter.name);

        switch (eventData.pointerEnter.name)
        {
            case "SkillIcon":
            {
                Managers.Game.SkillSystem.CurrentSelectSkill = SkillComponent;
                Managers.Game.SkillSystem.DragSkillIcon.SetActive(true);
                Managers.Game.SkillSystem.DragSkillIcon.GetComponent<Image>().sprite = SkillComponent.SkillIcon;
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

    public void SkillUseButtonClick()
    {
        Debug.Log("스킬 사용버튼 클릭됨!");
        (SkillComponent as ActiveSkill).CastSkill();
    }
}
