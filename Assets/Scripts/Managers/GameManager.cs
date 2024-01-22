using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    
    [SerializeField] private ItemGrid mainInventory;
    private bool isInventoryClosed = false;

    [SerializeField]private TargetingSystem targetingSystem;
    [SerializeField]private DroppedItem droppedItem;
    [FormerlySerializedAs("inventoryHandle")] [SerializeField] private InventoryUpperBar inventoryUpperBar;
    
    
    // 마우스 커서 파일
    private Texture2D battleCursor;
    private Texture2D grabCursor;
    private Texture2D grabbingCursor;
    
    private void Awake()
    {
        battleCursor = Resources.Load<Texture2D>("Images/cursor_battle");
        grabCursor = Resources.Load<Texture2D>("Images/cursor_grab");
        grabbingCursor = Resources.Load<Texture2D>("Images/cursor_grabbing");

        if (instance == null)
        {
            instance = this;
            
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                return null;
            
            return instance;
        }
    }

    private void Start()
    {
        mainInventory.SetGrid(8, 10);
        
        // 이벤트 핸들러 등록
        targetingSystem.RegisterHandler(ChangeCursorForBattle, BackNormalCursor);
        droppedItem.RegisterHandler(ChangeCursorForGrab, ChangeCursorForGrabbing, BackNormalCursor);
        inventoryUpperBar.RegisterHandler(ChangeCursorForGrabbing, BackNormalCursor);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isInventoryClosed = !isInventoryClosed;
            mainInventory.transform.parent.parent.gameObject.SetActive(isInventoryClosed);
        }
    }

    // 커서 관련 콜백 함수
    private void ChangeCursorForBattle()
    {
        Cursor.SetCursor(battleCursor, new Vector2(battleCursor.width / 2, battleCursor.height / 2), CursorMode.Auto);
    }

    private void ChangeCursorForGrab()
    {
        Cursor.SetCursor(grabCursor, new Vector2(grabCursor.width / 2, grabCursor.height / 2), CursorMode.Auto);
    }
    
    private void ChangeCursorForGrabbing()
    {
        Cursor.SetCursor(grabbingCursor, new Vector2(grabbingCursor.width / 2, grabbingCursor.height / 2), CursorMode.Auto);
    }

    private void BackNormalCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
