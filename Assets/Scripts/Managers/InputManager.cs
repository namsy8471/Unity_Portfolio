using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputManager
{
    // 버튼 할당
    public delegate void MouseButtonPressed();

    private Dictionary<KeyCode, Action> _keyButtonPressed = new Dictionary<KeyCode, Action>();
    public Dictionary<KeyCode, Action> KeyButtonPressed => _keyButtonPressed;
    
    public event MouseButtonPressed LMBPressed;
    public event MouseButtonPressed RMBPressed;
    
    // KeyCodes and properties
    private KeyCode inventoryKeyCode;
    public KeyCode InventoryKey => inventoryKeyCode;
    public void Init()
    {
        inventoryKeyCode = KeyCode.I;
    }

    public void Update()
    {
        // 여기서 delegate로 지정한 키들을 작동 시키자!

        foreach (var keyCode in _keyButtonPressed.Keys)
        {
            if (Input.GetKeyDown(keyCode))
            {
                _keyButtonPressed[keyCode]?.Invoke();
            }
        }
    }

    public void MapKey(KeyCode keyCode)
    {
        
    }

    public void MapMouse(int i)
    {
        switch (i)
        {
            case 0:
            {
                break;
            }
            case 1:
            {
                break;
            }
            case 2:
            {
                break;
            }
            
            default:
                break;
        }
    }
}
