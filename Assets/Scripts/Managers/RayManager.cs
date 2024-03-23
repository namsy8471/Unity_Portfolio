using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class RayManager
{
    // This Manager manage Ray. All function needed to use ray need to access this Manager.
    // 이 매니저는 레이를 관리합니다. 레이가 필요한 모든 기능은 이 매니저에 접근해야만 합니다.

    private Collider _hitCollider;
    private Vector3 _hitPoint;
    
    private const float SphereRadius = 50.0f; // 마우스 히트 포인트에서 타게팅이 가능한 범위
    
    public Collider RayHitCollider => _hitCollider;
    public Vector3 RayHitPoint => _hitPoint;
    public void Init()
    {
        Managers.Game.TargetingSystem.SetSetClosestEnemyCollider(SetClosestEnemyCollider);
    }

    public void Update()
    {
        CastRay();
    }

    private void CastRay()
    {
        if (Util.IsMousePointerOutOfScreen()) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, 100f))
        {
            if (hit.collider.isTrigger)
            {
                if (Physics.Raycast(hit.point, ray.direction, out hit, 100f))
                {
                    _hitCollider = hit.collider;
                    _hitPoint = hit.point;
                    
                    Debug.Log(_hitCollider.name);
                }
            }
            
            else
            {
                _hitCollider = hit.collider;
                _hitPoint = hit.point;
            }
        }

        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.green);
    }

    private void SetClosestEnemyCollider()
    {
        Collider[] colliders = Physics.OverlapSphere(_hitPoint, SphereRadius, 1 << LayerMask.NameToLayer("Enemy"));
        
        if (colliders.Length > 0)
        {
            var closestDistance = Mathf.Infinity;

            foreach (Collider collider in colliders)
            {
                var distance = Vector3.Distance(_hitPoint, collider.transform.position);

                if (distance < closestDistance)
                {
                    Managers.Game.TargetingSystem.Target = collider.gameObject;
                    closestDistance = distance;
                }
            }
        }
    }
}
