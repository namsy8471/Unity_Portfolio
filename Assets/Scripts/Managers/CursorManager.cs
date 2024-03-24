using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;

public class CursorManager
{
    // This Manager manages Cursor. Whole functions about Cursor will be used in this manager.
    // 이 매니저는 커서를 관리합니다. 커서와 관련된 모든 함수는 여기서 사용됩니다.
    
    // 마우스 커서 파일
    private Texture2D battleCursor;
    private Texture2D grabCursor;
    private Texture2D grabbingCursor;

    private bool _isDragging;

    public void Init()
    {
        battleCursor = Resources.Load<Texture2D>("Images/cursor_battle");
        grabCursor = Resources.Load<Texture2D>("Images/cursor_grab");
        grabbingCursor = Resources.Load<Texture2D>("Images/cursor_grabbing");
    }

    public void Update()
    {
        if (Managers.Game.TargetingSystem.isTargetingWorkNow
            || Managers.Ray.RayHitCollider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
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
        Cursor.SetCursor(battleCursor, new Vector2(battleCursor.width / 2, battleCursor.height / 2), CursorMode.Auto);
    }

    private void ChangeCursorForGrab()
    {
        Cursor.SetCursor(grabCursor, new Vector2(grabCursor.width / 2, grabCursor.height / 2), CursorMode.Auto);
        _isDragging = false;
    }
    
    private void ChangeCursorForGrabbing()
    {
        Cursor.SetCursor(grabbingCursor, new Vector2(grabbingCursor.width / 2, grabbingCursor.height / 2), CursorMode.Auto);
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
