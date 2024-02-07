using System;
using UnityEngine;
using UnityEngine.UI;


public class TargetingSystem
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

    // 타게팅 시스템 관련
    private Camera cam;
    
    private LayerMask targetLayer;
    private float targetRadius = 50.0f; // 마우스 히트 포인트에서 타게팅이 가능한 범위
    
    private LineRenderer _lineRenderer;          // 마우스와 적이 이어지는 라인 그리기
    private Vector3 _mouseTargetPos;          // 히트 포인트 저장 백터
    private GameObject _target;
    
    /// 타게팅 원 이미지 관련
    private Image _circleImage;
    private float _circleSize = 50f;
    private RectTransform _circleRectTransform;

    private float _rotationOffset = 0;           // 원 회전 오프셋
    ////////////////

    private TargetState _targetState;
    
    public GameObject Target
    {
        get => _target;
        set => _target = value;
    }
    
    public void Init()
    {
        _circleImage = Resources.Load<Image>("Images/TargetingCircle");
        if(_circleImage != null) _circleRectTransform = _circleImage.rectTransform;

        _changeCursorForBattle += Managers.Cursor.ChangeCursorForBattle;
        _changeCursorForNormal += Managers.Cursor.BackNormalCursor;
        
        _lineRenderer = Managers.Game.Player.GetComponent<LineRenderer>();
        
        _lineRenderer.startColor = Color.cyan;
        _lineRenderer.endColor = Color.cyan;
        _lineRenderer.startWidth = 0.05f;
        _lineRenderer.endWidth = 0.05f;
        
        cam = Camera.main;
    }

    public void Update()
    {
        switch (_targetState)
        {
            case TargetState.Idle:
                
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    _targetState = TargetState.TargetingStart;
                    break;
                }
                break;
            
            case TargetState.TargetingStart:
                
                FindEnemy();
                _targetState = TargetState.Targeting;
                
                break;
            
            case TargetState.Targeting:
                if (Input.GetKeyUp(KeyCode.LeftControl))
                {
                    _targetState = TargetState.Idle;
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
            _mouseTargetPos = hit.point;
        
        // 적 타겟 찾기
        Collider[] colliders = Physics.OverlapSphere(_mouseTargetPos, targetRadius, targetLayer);
        
        if (colliders.Length > 0)
        {
            // 가장 가까운 적을 선택
            var closestDistance = Mathf.Infinity;

            foreach (Collider collider in colliders)
            {
                var distance = Vector3.Distance(_mouseTargetPos, collider.transform.position);

                if (distance < closestDistance)
                {
                    _target = collider.gameObject;
                    closestDistance = distance;
                }
            }
        }
    }

    private void ClearTarget()
    {
        _target = null;
        _lineRenderer.positionCount = 0;
        _circleImage.gameObject.SetActive(false);
        
        _changeCursorForNormal?.Invoke();
    }

    private void TargetEnemy()
    {
        DrawCircleToEnemy(_target.transform.position);
        UpdateTargetIndicator();
        
        _changeCursorForBattle?.Invoke();
    }

    private void UpdateTargetIndicator()
    {
        // 적이 있는 경우 마우스와 적 사이에 선을 그림
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, 1000f, 1 << LayerMask.NameToLayer("Ground")))
            _mouseTargetPos = hit.point;
        
        Vector3 targetPosition = _target.transform.position;

        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0, _mouseTargetPos);
        _lineRenderer.SetPosition(1, targetPosition);
    }

    private void DrawCircleToEnemy(Vector3 currentTargetPos)
    {
        _circleImage.gameObject.SetActive(true);
        Vector3 screenPos = cam.WorldToScreenPoint(currentTargetPos);

        // 타겟팅 원의 위치 업데이트
        _circleRectTransform.position = screenPos;

        // 타겟팅 원 회전
        _rotationOffset += 1;
        _circleImage.gameObject.transform.rotation = Quaternion.Euler(0, 0, _rotationOffset);
        
        // 타겟팅 카메라 거리에 따른 원 크기 조절
        float dist = (_target.transform.position - cam.transform.position).magnitude;
        float scaleFactor = _circleSize / dist;
        _circleRectTransform.localScale = new Vector3(scaleFactor, scaleFactor, 1.0f);
    }

    public bool IsCurrentTargetExist()
    {
        return _target;
    }
    
    
    public GameObject GetCurrentTarget()
    {
        return _target;
    }

}
