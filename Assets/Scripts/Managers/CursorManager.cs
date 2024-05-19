using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;

public class CursorManager
{
    // This Manager manages Cursor. Whole functions about Cursor will be used in this manager.
    // 이 매니저는 커서를 관리합니다. 커서와 관련된 모든 함수는 여기서 사용됩니다.
    
    // 마우스 커서 파일
    private Texture2D _battleCursor;
    private Texture2D _grabCursor;
    private Texture2D _grabbingCursor;

    private bool _isDragging;

    public void Init()
    {
        _battleCursor = Resources.Load<Texture2D>("Images/cursor_battle");
        _grabCursor = Resources.Load<Texture2D>("Images/cursor_grab");
        _grabbingCursor = Resources.Load<Texture2D>("Images/cursor_grabbing");
    }

    public void Update()
    {
        if (Util.IsMousePointerOutOfScreen())
        {
            BackNormalCursor();
            return;
        }
        
        if (Managers.Game.TargetingSystem.isTargetingWorkNow
            || (Managers.Ray.RayHitCollider.gameObject.layer == LayerMask.NameToLayer("Enemy")
            && Managers.Ray.RayHitCollider.gameObject.GetComponent<EnemyController>().State != EnemyController.EnemyState.Dead))
        {
            ChangeCursorForBattle();
        }

        else if (Managers.Ray.RayHitCollider.gameObject.layer == LayerMask.NameToLayer("Item")
                 || _isDragging)
        {
            if (Input.GetMouseButton(0))
            {
                ChangeCursorForGrabbing();
            }

            else
            {
                ChangeCursorForGrab();
            }
        }

        else
        {
            BackNormalCursor();
        }
    }

    private void ChangeCursorForBattle()
    {
        Cursor.SetCursor(_battleCursor, new Vector2(_battleCursor.width / 2, _battleCursor.height / 2), CursorMode.Auto);
    }

    private void ChangeCursorForGrab()
    {
        Cursor.SetCursor(_grabCursor, new Vector2(_grabCursor.width / 2, _grabCursor.height / 2), CursorMode.Auto);
        _isDragging = false;
    }
    
    private void ChangeCursorForGrabbing()
    {
        Cursor.SetCursor(_grabbingCursor, new Vector2(_grabbingCursor.width / 2, _grabbingCursor.height / 2), CursorMode.Auto);
        if (EventSystem.current.IsPointerOverGameObject())
        {
            _isDragging = true;
        }
    }

    private void BackNormalCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        _isDragging = false;
    }

    public void SetIsDraggingNow(bool isDrag)
    {
        _isDragging = isDrag;
    }
}
