using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSystem
{
    
    // TODO
    // 스킬을 받아온다.
    // 스킬을 사용한다.
    // 스킬 사용 시 플레이어에게 알린다.
    // 현재 스킬이 사용 중인지 알린다.
    // 스킬을 드래그 할 수 있다.

    public List<Skill> SkillsList;
    public Smash SkillSmash;
    
    public GameObject DragSkillIcon { get; private set; }
    public Skill CurrentSelectSkill { get; set; }
    
    public void Init()
    {
        var parentGo = new GameObject() {name = "SkillSystem"};
        parentGo.transform.SetParent(Managers.ManagersGo.transform);
        
        DragSkillIconInit();
        
        SkillSmash = new Smash();
        SkillsList = new List<Skill>
        {
            SkillSmash
        };
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
    }

    private void DragSkill()
    {
        Debug.Log("DragSkill 작동 중");
        DragSkillIcon.transform.position = Input.mousePosition;
    }
    
    public void AddSkill<T>(T newSkill) where T : Skill
    {
        var skillToAdd = GameObject.Instantiate(Resources.Load<GameObject>
            ("Prefabs/UI/UI_SkillDescription/SkillDescription"), 
            Managers.Graphics.UI.SkillUI.transform.Find("SkillWindow/WindowHandle/UI_Background/SkillDescriptions"));

        skillToAdd.GetComponent<UI_SkillDescription>().Init(newSkill);
    }
}
