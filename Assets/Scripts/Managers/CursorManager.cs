using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;

public class CursorManager
{
    // 마우스 커서 파일
    private Texture2D battleCursor;
    private Texture2D grabCursor;
    private Texture2D grabbingCursor;

    private bool isDrag = false;
    public bool IsDragging => isDrag;
    
    public void Init()
    {
        battleCursor = Resources.Load<Texture2D>("Images/cursor_battle");
        grabCursor = Resources.Load<Texture2D>("Images/cursor_grab");
        grabbingCursor = Resources.Load<Texture2D>("Images/cursor_grabbing");
    }

    // 커서 관련 콜백 함수
    public void ChangeCursorForBattle()
    {
        Cursor.SetCursor(battleCursor, new Vector2(battleCursor.width / 2, battleCursor.height / 2), CursorMode.Auto);
    }

    public void ChangeCursorForGrab()
    {
        Cursor.SetCursor(grabCursor, new Vector2(grabCursor.width / 2, grabCursor.height / 2), CursorMode.Auto);
        isDrag = false;
        Debug.Log("IsDrag = " + isDrag);
    }
    
    public void ChangeCursorForGrabbing()
    {
        Cursor.SetCursor(grabbingCursor, new Vector2(grabbingCursor.width / 2, grabbingCursor.height / 2), CursorMode.Auto);
        if (EventSystem.current.IsPointerOverGameObject())
        {
            isDrag = true;
        }

        Debug.Log("IsDrag = " + isDrag);
    }

    public void BackNormalCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        isDrag = false;
        Debug.Log("IsDrag = " + isDrag);
    }
}
