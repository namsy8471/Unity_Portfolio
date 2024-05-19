using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class VisualManager
{
    // This Manager will be manager visual effect like Image, Line Renderer, particle etc.
    // 이 매니저는 이미지나 라인 렌더러, 파티클 등의 시각 효과를 담당합니다.

    private GameObject _imageCanvas;
    private GameObject _skillBubbleCanvas;
    
    private LineRenderer _lineRenderer;
    
    private GameObject _circleImage;
    private readonly float _circleSize = 50f;
    private float _rotationOffset = 0;

    private GameObject _skillBubble;
    
    public GameObject ImageCanvas => _imageCanvas;
    private Image SkillIconInBubble { get; set; }

    public void Init()
    {
        _imageCanvas = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/ImageCanvas"));
        _imageCanvas.GetComponent<Canvas>().sortingOrder = 1;
        
        _skillBubbleCanvas = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/UI_SkillFloatingIcon/UI_SkillBubbleCanvas"),
            GameObject.FindWithTag("Player").transform);
        _skillBubbleCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
        _skillBubbleCanvas.transform.position = 
            _skillBubbleCanvas.transform.parent.transform.position
            + Vector3.up * (GameObject.FindWithTag("Player").GetComponent<Collider>().bounds.size.y + 0.4f);
        
        TargetingCircleInit();
        LineRendererInit();
        SkillFloatingIconInit();
        
        #region Action Binding
        
        // These functions use in Targeting System.
        Managers.Game.TargetingSystem.SetDrawCircleAction(DrawCircleOnEnemy);
        Managers.Game.TargetingSystem.SetDrawLineAction(DrawLineToEnemy);
        Managers.Game.TargetingSystem.SetClearTargetingCircle(ClearTargetingCircle);
        Managers.Game.TargetingSystem.SetClearTargetingLineRenderer(ClearTargetingLineRenderer);
        
        #endregion
    }

    public void Update()
    {
        if(_skillBubble.activeSelf)
            UpdateSkillFloatingIcon();
    }
    
    private void TargetingCircleInit()
    {
        _circleImage = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/TargetingCircle/TargetingCircle"),
            _imageCanvas.transform);
        
        _circleImage.SetActive(false);
    }

    private void LineRendererInit()
    {
        _lineRenderer = Managers.ManagersGo.AddComponent<LineRenderer>();
        _lineRenderer.material = Resources.Load<Material>("Materials/LineRenderer");
        _lineRenderer.startColor = Color.cyan;
        _lineRenderer.endColor = Color.cyan;
        _lineRenderer.startWidth = 0.05f;
        _lineRenderer.endWidth = 0.05f;
        _lineRenderer.positionCount = 0;

        _lineRenderer.shadowCastingMode = ShadowCastingMode.Off;
    }

    private void SkillFloatingIconInit()
    {
        _skillBubble = GameObject.Instantiate(Resources.Load<GameObject>
                ("Prefabs/UI/UI_SkillFloatingIcon/UI_SkillFloatingIcon"),
            _skillBubbleCanvas.transform);
        
        SkillIconInBubble = _skillBubble.transform.GetChild(0).GetComponent<Image>();
        _skillBubble.SetActive(false);
    }

    private void DrawCircleOnEnemy(Vector3 currentTargetPos)
    {
        if (Managers.Game.TargetingSystem.Target == null) return;
        
        Managers.Game.TargetingSystem.Target.GetComponent<EnemyController>()?.HpBar.SetActive(true);
        
        _circleImage.gameObject.SetActive(true);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(currentTargetPos);

        _circleImage.transform.position = screenPos;

        _rotationOffset += 1;
        _circleImage.gameObject.transform.rotation = Quaternion.Euler(0, 0, _rotationOffset);
        
        float dist = (Managers.Game.TargetingSystem.Target.transform.position - Camera.main.transform.position).magnitude;
        float scaleFactor = _circleSize / dist;
        _circleImage.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1.0f);
    }
    
    private void DrawLineToEnemy()
    {
        Vector3 mousePos = Managers.Ray.RayHitPoint;
        Vector3 targetPosition = Managers.Game.TargetingSystem.Target.transform.position;

        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0, mousePos);
        _lineRenderer.SetPosition(1, targetPosition);
    }

    public void DrawSkillFloatingIcon<T>(T skill) where T : ActiveSkill
    {
        _skillBubble.SetActive(true);
        _skillBubble.GetComponent<UI_SkillFloatingIcon>().Skill = skill;
        SkillIconInBubble.sprite = skill.SkillIcon;
    }

    private void UpdateSkillFloatingIcon()
    {
        _skillBubble.transform.LookAt(Camera.main.transform);
    }
    
    private void ClearTargetingCircle()
    {
        Managers.Game.TargetingSystem.Target.GetComponent<EnemyController>()?.HpBar.SetActive(false);
        _circleImage.SetActive(false);
    }

    private void ClearTargetingLineRenderer()
    {
        _lineRenderer.positionCount = 0;
    }
    
    public void ClearSkillFloatingIcon()
    {
        SkillIconInBubble.sprite = null;
        _skillBubble.GetComponent<UI_SkillFloatingIcon>().Skill = null;
        _skillBubble.SetActive(false);
    }
}
