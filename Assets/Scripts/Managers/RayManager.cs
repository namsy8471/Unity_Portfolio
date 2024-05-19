using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RayManager
{
    // This Manager manages Ray. All functions needed to use ray needs to access this Manager.
    // 이 매니저는 레이를 관리합니다. 레이가 필요한 모든 기능은 이 매니저에 접근해야만 합니다.

    private Collider _hitCollider;
    private Vector3 _hitPoint;

    private GameObject _currentGo;
    
    private LayerMask _layerMask;
    private const float SphereRadius = 50.0f; // 마우스 히트 포인트에서 타게팅이 가능한 범위
    
    public Collider RayHitCollider => _hitCollider;
    public Vector3 RayHitPoint => _hitPoint;

    public Collider RayHitColliderByMouseClicked { get; set; }
    public Vector3 RayHitPointByMouseClicked { get; set; }

    public void Init()
    {
        Managers.Game.TargetingSystem.SetSetClosestEnemyCollider(SetClosestEnemyCollider);
        _layerMask = ~LayerMask.GetMask("Player", "DetectingBoundary", "Building");
    }

    public void Update()
    {
        CastRay();
    }

    private void CastRay()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (Util.IsMousePointerOutOfScreen()) return;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hit, 100f, _layerMask))
        {
            SaveHitInfo(hit);
            if (Input.GetMouseButton(0))
            {
                SaveHitInfoByMouse(hit);
            }
            
            if (_hitCollider.gameObject.layer == LayerMask.NameToLayer("Enemy") ||
                _hitCollider.gameObject.layer == LayerMask.NameToLayer("Player") ||
                _hitCollider.gameObject == Managers.Game.TargetingSystem.Target)
            {
                if (_currentGo != _hitCollider.gameObject)
                {
                    _currentGo = _hitCollider.gameObject;
                    _currentGo.GetComponent<Controller>().HpBar.SetActive(true);
                }
            }
            else
            {
                _currentGo?.GetComponent<Controller>().HpBar.SetActive(false);
                _currentGo = null;
            }
        }

        if (Physics.Raycast(ray, out var hit2, 100f, LayerMask.GetMask("Player")))
        {
            Managers.Game.Player.GetComponent<Controller>().HpBar.SetActive(true);
        }
        else
        {
            Managers.Game.Player.GetComponent<Controller>().HpBar.SetActive(false);
        }

    }

    private void SaveHitInfo(RaycastHit hit)
    {
        _hitCollider = hit.collider;
        _hitPoint = hit.point;
    }

    private void SaveHitInfoByMouse(RaycastHit hit)
    {
        RayHitColliderByMouseClicked = hit.collider;
        RayHitPointByMouseClicked = hit.point;
    }

    private void SetClosestEnemyCollider()
    {
        Collider[] colliders = Physics.OverlapSphere(_hitPoint, SphereRadius, 1 << LayerMask.NameToLayer("Enemy"));

        if (colliders.Length < 1) return;
        
        var closestDistance = Mathf.Infinity;
        foreach (Collider collider in colliders)
        {
            var distance = Vector3.Distance(_hitPoint, collider.transform.position);

            var enemyState = collider.GetComponent<EnemyController>().State;
            if (enemyState == EnemyController.EnemyState.Dead) continue;
            
            if (distance < closestDistance)
            {
                Managers.Game.TargetingSystem.Target = collider.gameObject;
                closestDistance = distance;
            }
        }
    
    }
}
