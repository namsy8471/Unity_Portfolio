using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class TargetingSystem : MonoBehaviour
{

    private enum TargetState
    {
        Idle,
        TargetingStart,
        Targeting
    }
    
    // 마우스 커서 제어
    private static Action _changeCursorForBattle;
    private static Action _changeCursorForNormal;

    public delegate void ChangeCursorForBattle();

    // 타게팅 시스템 관련
    private Camera cam;
    [SerializeField]private  LayerMask targetLayer;
    [SerializeField]private float targetRadius = 50.0f; // 마우스 히트 포인트에서 타게팅이 가능한 범위
    private LineRenderer lineRenderer;          // 마우스와 적이 이어지는 라인 그리기
    private Vector3 mouseTargetPos;          // 히트 포인트 저장 백터
    [SerializeField]private GameObject currentTarget;
    
    /// 타게팅 원 이미지 관련
    [SerializeField]private Image circleImage;
    private float circleSize = 50f;
    private RectTransform circleRectTransform;

    private  float rotationOffset = 0;           // 원 회전 오프셋
    ////////////////

    private TargetState targetState;
    
    private void Start()
    {
        circleRectTransform = circleImage.rectTransform;
        
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        
        lineRenderer.startColor = Color.cyan;
        lineRenderer.endColor = Color.cyan;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        
        cam = Camera.main;
        
    }

    // Update is called once per frame
    private void Update()
    {
        switch (targetState)
        {
            case TargetState.Idle:
                
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    targetState = TargetState.TargetingStart;
                    break;
                }
                break;
            
            case TargetState.TargetingStart:
                
                FindEnemy();
                targetState = TargetState.Targeting;
                
                break;
            
            case TargetState.Targeting:
                if (Input.GetKeyUp(KeyCode.LeftControl))
                {
                    ClearTarget();
                    targetState = TargetState.Idle;
                    break;
                }
                
                TargetEnemy();
                break;
            
            default:
                break;
            
        }
    }
    
    private void FindEnemy()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, 1000f, 1 << LayerMask.NameToLayer("Ground")))
            mouseTargetPos = hit.point;
        
        // 적 타겟 찾기
        Collider[] colliders = Physics.OverlapSphere(mouseTargetPos, targetRadius, targetLayer);
        
        if (colliders.Length > 0)
        {
            // 가장 가까운 적을 선택
            var closestDistance = Mathf.Infinity;

            foreach (Collider collider in colliders)
            {
                var distance = Vector3.Distance(mouseTargetPos, collider.transform.position);

                if (distance < closestDistance)
                {
                    currentTarget = collider.gameObject;
                    closestDistance = distance;
                }
            }
        }
    }

    private void ClearTarget()
    {
        currentTarget = null;
        lineRenderer.positionCount = 0;
        circleImage.gameObject.SetActive(false);
        _changeCursorForNormal();
    }

    private void TargetEnemy()
    {
        DrawCircleToEnemy(currentTarget.transform.position);
        UpdateTargetIndicator();
        _changeCursorForBattle();
    }

    private void UpdateTargetIndicator()
    {
        // 적이 있는 경우 마우스와 적 사이에 선을 그림
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, 1000f, 1 << LayerMask.NameToLayer("Ground")))
            mouseTargetPos = hit.point;
        
        Vector3 targetPosition = currentTarget.transform.position;

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, mouseTargetPos);
        lineRenderer.SetPosition(1, targetPosition);
    }

    private void DrawCircleToEnemy(Vector3 currentTargetPos)
    {
        circleImage.gameObject.SetActive(true);
        Vector3 screenPos = cam.WorldToScreenPoint(currentTargetPos);

        // 타겟팅 원의 위치 업데이트
        circleRectTransform.position = screenPos;

        // 타겟팅 원 회전
        rotationOffset += 1;
        circleImage.gameObject.transform.rotation = Quaternion.Euler(0, 0, rotationOffset);
        
        // 타겟팅 카메라 거리에 따른 원 크기 조절
        float dist = (currentTarget.transform.position - cam.transform.position).magnitude;
        float scaleFactor = circleSize / dist;
        circleRectTransform.localScale = new Vector3(scaleFactor, scaleFactor, 1.0f);
    }

    public bool IsCurrentTargetExist()
    {
        return currentTarget;
    }
    public Vector3 GetCurrentTargetPos()
    {
        return currentTarget.transform.position;
    }
    
    public GameObject GetCurrentTarget()
    {
        return currentTarget;
    }

    public void RegisterHandler(Action battleCursor, Action backToNormalCursor)
    {
        _changeCursorForBattle = battleCursor;
        _changeCursorForNormal = backToNormalCursor;
    }
}
