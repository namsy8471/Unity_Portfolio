using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_UpperBar : UI_DraggableWindow
{
    private void Start()
    {
        base.Init();
    }
    
    public override void CloseButtonClick()
    {
        transform.parent.gameObject.SetActive(false);
        Managers.Input.RemovePlayerMouseActions();
        Managers.Input.RollbackPlayerMouseActions();
    }
}
