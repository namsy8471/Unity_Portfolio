using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class InputManager
{
    private Dictionary<KeyCode, Action> _keyButtonDown = new Dictionary<KeyCode, Action>();
    private Dictionary<KeyCode, Action> _keyButtonPressed = new Dictionary<KeyCode, Action>();
    private Dictionary<KeyCode, Action> _keyButtonUp = new Dictionary<KeyCode, Action>();
    public Dictionary<KeyCode, Action> KeyButtonDown => _keyButtonDown;
    public Dictionary<KeyCode, Action> KeyButtonPressed => _keyButtonPressed;
    public Dictionary<KeyCode, Action> KeyButtonUp => _keyButtonUp;

    public Action LMBDown;
    public Action LMBPressed;
    public Action RMBDown;
    public Action RMBPressed;

    public Action WinDown;
    
    #region KeyCodes and properties

    private KeyCode _inventoryKeyCode;
    private KeyCode _characterStatusKeyCode;
    private KeyCode _skillWindowKeyCode;
    
    private KeyCode _inventoryItemRotateKeyCode;
    private KeyCode _postureChangeKeyCode;

    private KeyCode _moveForwardKeyCode;
    private KeyCode _moveBackwardKeyCode;
    private KeyCode _moveLeftKeyCode;
    private KeyCode _moveRightKeyCode;
    private KeyCode _walkKeyCode;

    private KeyCode _targetingKeyCode;
    private KeyCode _skillQuitKeyCode;
    
    public KeyCode InventoryKey => _inventoryKeyCode;
    public KeyCode CharacterStatusKey => _characterStatusKeyCode;
    public KeyCode SkillWindowKey => _skillWindowKeyCode;
    
    public KeyCode InventoryItemRotateKey => _inventoryItemRotateKeyCode;
    public KeyCode PostureChangeKey => _postureChangeKeyCode;
    
    public KeyCode MoveForwardKey => _moveForwardKeyCode;
    public KeyCode MoveBackwardKey => _moveBackwardKeyCode;
    public KeyCode MoveLeftKey => _moveLeftKeyCode;
    public KeyCode MoveRightKey => _moveRightKeyCode;
    public KeyCode WalkKey => _walkKeyCode;

    public KeyCode TargetingKey => _targetingKeyCode;
    public KeyCode SkillQuitKey => _skillQuitKeyCode;
    
    
    // Windows API constants for key codes
    private const int VK_LWIN = 0x5B; // Left Windows key
    private const int VK_RWIN = 0x5C; // Right Windows key
    
    // Import the GetAsyncKeyState function from user32.dll
    [DllImport("user32.dll")]
    private static extern short GetAsyncKeyState(int vKey);
    
    #endregion

    private List<KeyCode> _wholeKeyList = new List<KeyCode>();

    public void Init()
    {
        _inventoryKeyCode = KeyCode.I;
        _characterStatusKeyCode = KeyCode.C;
        _skillWindowKeyCode = KeyCode.Z;
        
        _inventoryItemRotateKeyCode = KeyCode.R;
        _postureChangeKeyCode = KeyCode.Space;

        _moveForwardKeyCode = KeyCode.W;
        _moveBackwardKeyCode = KeyCode.S;
        _moveLeftKeyCode = KeyCode.A;
        _moveRightKeyCode = KeyCode.D;

        _walkKeyCode = KeyCode.LeftShift;

        _targetingKeyCode = KeyCode.LeftControl;
        _skillQuitKeyCode = KeyCode.Escape;
    }

    public void Update()
    {
        foreach (var keyCode in _keyButtonDown.Keys)
        {
            if (Input.GetKeyDown(keyCode))
            {
                _keyButtonDown[keyCode]?.Invoke();
            }
        }

        foreach (var keyCode in _keyButtonPressed.Keys)
        {
            if (Input.GetKey(keyCode))
            {
                _keyButtonPressed[keyCode]?.Invoke();
            }
        }
        
        foreach (var keyCode in _keyButtonUp.Keys)
        {
            if (Input.GetKeyUp(keyCode))
            {
                _keyButtonUp[keyCode]?.Invoke();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            LMBDown?.Invoke();
        }
        
        if (Input.GetMouseButton(0))
        {
            LMBPressed?.Invoke();
        }

        if (Input.GetMouseButtonDown(1))
        {
            RMBDown?.Invoke();
        }

        if (Input.GetMouseButton(1))
        {
            RMBPressed?.Invoke();
        }

        if (IsWindowsKeyPressed())
        {
            WinDown?.Invoke();
        }
    }
    
    private bool IsWindowsKeyPressed()
    {
        // Check the state of both the left and right Windows keys
        return (GetAsyncKeyState(VK_LWIN) & 0x8000) != 0 || (GetAsyncKeyState(VK_RWIN) & 0x8000) != 0;
    }
    
    public void AddAction(Dictionary<KeyCode, Action> dictionary, KeyCode key, Action action)
    {
        if (dictionary.ContainsKey(key))
        {
            dictionary[key] += action;
        }
        else
        {
            dictionary.Add(key, action);
        }
        
        _wholeKeyList.Add(key);
    }
    
    public void RemoveAction(Dictionary<KeyCode, Action> dictionary, KeyCode key, Action action)
    {
        if (!dictionary.ContainsKey(key)) return;
        
        dictionary[key] -= action;
        _wholeKeyList.Remove(key);
    }
    
    public void RemovePlayerMouseActions()
    {
        foreach (var action in Managers.Game.Player.GetComponent<PlayerController>().PlayerMouseDownActions)
        {
            Managers.Input.LMBDown -= action;
        }

        foreach (var action in Managers.Game.Player.GetComponent<PlayerController>().PlayerMousePressedActions)
        {
            Managers.Input.LMBPressed -= action;
        }
    }
    
    public void RollbackPlayerMouseActions()
    {
        foreach (var action in Managers.Game.Player.GetComponent<PlayerController>().PlayerMouseDownActions)
        {
            Managers.Input.LMBDown += action;
        }

        foreach (var action in Managers.Game.Player.GetComponent<PlayerController>().PlayerMousePressedActions)
        {
            Managers.Input.LMBPressed += action;
        }
    }
    
    public void MapNewKey(KeyCode ordinaryKey, KeyCode newKeyCode)
    {
        foreach (var key in _keyButtonDown.Keys)
        {
            if (key == ordinaryKey)
            {
                _keyButtonDown.Add(newKeyCode, _keyButtonDown[ordinaryKey]);
                _keyButtonDown.Remove(ordinaryKey);
            }
        }

        foreach (var key in _keyButtonPressed.Keys)
        {
            if (key == ordinaryKey)
            {
                _keyButtonPressed.Add(newKeyCode, _keyButtonPressed[ordinaryKey]);
                _keyButtonPressed.Remove(ordinaryKey);
            }
        }
        
        foreach (var key in _keyButtonUp.Keys)
        {
            if (key == ordinaryKey)
            {
                _keyButtonUp.Add(newKeyCode, _keyButtonUp[ordinaryKey]);
                _keyButtonUp.Remove(ordinaryKey);
            }
        }
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
