using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualManager
{
    // This Manager will be manager visual effect like Image, Line Renderer, particle etc.
    // 이 매니저는 이미지나 라인 렌더러, 파티클 등의 시각 효과를 담당합니다.

    private GameObject _imageCanvas;
    
    private GameObject mouseClickParticlePrefab;
    private GameObject _mouseClickParticle;
    
    private LineRenderer _lineRenderer;          // 마우스와 적이 이어지는 라인 그리기
    
    private GameObject _circleImage;
    private float _circleSize = 50f;
    
    private float _rotationOffset = 0;           // 원 회전 오프셋
    ////////////////
    public void Init()
    {
        _imageCanvas = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/ImageCanvas"));
        _circleImage = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/TargetingCircle/TargetingCircle"), _imageCanvas.transform);
        
        _circleImage.SetActive(false);
        
        _lineRenderer = Managers.ManagersGO.AddComponent<LineRenderer>();
        
        _lineRenderer.startColor = Color.cyan;
        _lineRenderer.endColor = Color.cyan;
        _lineRenderer.startWidth = 0.05f;
        _lineRenderer.endWidth = 0.05f;
        
        mouseClickParticlePrefab = Resources.Load<GameObject>("Particles/MouseClickParticle");

        #region Action Binding
        
        // This func use in Targeting System.
        Managers.Game.TargetingSystem.SetDrawCircleAction(DrawCircleOnEnemy);
        Managers.Game.TargetingSystem.SetDrawLineAction(DrawLineToEnemy);
        Managers.Game.TargetingSystem.SetClearTargetingCircle(ClearTargetingCircle);
        Managers.Game.TargetingSystem.SetClearTargetingLineRenderer(ClearTargetingLineRenderer);
        
        #endregion
    }
    
    private void DrawCircleOnEnemy(Vector3 currentTargetPos)
    {
        if (Managers.Game.TargetingSystem.Target == null) return;
        _circleImage.gameObject.SetActive(true);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(currentTargetPos);

        // 타겟팅 원의 위치 업데이트
        _circleImage.transform.position = screenPos;

        // 타겟팅 원 회전
        _rotationOffset += 1;
        _circleImage.gameObject.transform.rotation = Quaternion.Euler(0, 0, _rotationOffset);
        
        // 타겟팅 카메라 거리에 따른 원 크기 조절
        float dist = (Managers.Game.TargetingSystem.Target.transform.position - Camera.main.transform.position).magnitude;
        float scaleFactor = _circleSize / dist;
        _circleImage.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1.0f);
    }
    
    private void DrawLineToEnemy()
    {
        // 적이 있는 경우 마우스와 적 사이에 선을 그림
        // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // RaycastHit hit;
        //
        // if (Physics.Raycast(ray, out hit, 1000f, 1 << LayerMask.NameToLayer("Ground")))
        //     mousePos = hit.point;

        Vector3 mousePos = Managers.Ray.RayHitPoint;
        Vector3 targetPosition = Managers.Game.TargetingSystem.Target.transform.position;

        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0, mousePos);
        _lineRenderer.SetPosition(1, targetPosition);
    }

    private void ClearTargetingCircle()
    {
        _circleImage.SetActive(false);
    }

    private void ClearTargetingLineRenderer()
    {
        _lineRenderer.positionCount = 0;
    }
    
    public void CreateMouseClickParticle(Vector3 pos)
    {
        // if (_mouseClickParticle) Destroy(_mouseClickParticle);
        //
        // _mouseClickParticle = Instantiate(mouseClickParticlePrefab, pos, mouseClickParticlePrefab.transform.rotation);
    }
}
