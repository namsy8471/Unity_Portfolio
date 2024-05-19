using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSystem
{
    public List<Skill> SkillsList;
    
    public GameObject DragSkillIcon { get; private set; }
    public Skill CurrentSelectSkill { get; set; }
    
    public void Init()
    {
        var parentGo = new GameObject() {name = "SkillSystem"};
        parentGo.transform.SetParent(Managers.ManagersGo.transform);
        
        DragSkillIconInit();
        
        SkillsList = new List<Skill>();
    }

    private void DragSkillIconInit()
    {
        DragSkillIcon = new GameObject() { name = "DragSkillIcon"};
        DragSkillIcon.transform.SetParent(Managers.Graphics.Visual.ImageCanvas.transform);
        DragSkillIcon.AddComponent<Image>();
        DragSkillIcon.GetComponent<Image>().raycastTarget = false;
        DragSkillIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 60);
        
        DragSkillIcon.SetActive(false);
    }

    public void Update()
    {
        if(CurrentSelectSkill != null)
            DragSkill();

        foreach (var skill in SkillsList)
        {
            // 쿨타임 감지
            var activeSkill = skill as ActiveSkill;
            activeSkill?.CoolTimeUpdate();
        }
    }

    private void DragSkill()
    {
        Debug.Log("DragSkill 작동 중");
        DragSkillIcon.transform.position = Input.mousePosition;

        if (Input.GetMouseButton(1))
        {
            StopDragSkill();
        }
    }

    public void StartDragSkill(Skill skill)
    {
        CurrentSelectSkill = skill;
        DragSkillIcon.SetActive(true);
        DragSkillIcon.GetComponent<Image>().sprite = skill.SkillIcon;
    }

    public void StopDragSkill()
    {
        CurrentSelectSkill = null;
        DragSkillIcon.SetActive(false);
    }
    
    public void AddSkill<T>(T newSkill) where T : Skill
    {
        var skillToAdd = GameObject.Instantiate(Resources.Load<GameObject>
            ("Prefabs/UI/UI_SkillDescription/SkillDescription"), 
            Managers.Graphics.UI.SkillUI.transform.Find("SkillWindow/WindowHandle/UI_Background/SkillDescriptions"));

        skillToAdd.GetComponent<UI_SkillDescription>().Init(newSkill);
        SkillsList.Add(newSkill);
    }
}
