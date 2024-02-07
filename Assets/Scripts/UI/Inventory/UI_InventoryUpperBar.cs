using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_InventoryUpperBar : UI_DraggableWindow
{
    private void Start()
    {
        base.Init();
    }
    
    public override void CloseButtonClick()
    {
        transform.parent.root.gameObject.SetActive(false);
    }
    
}
