using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_PopUpCloseButton : UI_Button
{
    private void Start()
    {
        base.Init();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        var currentTr = transform.parent;
        while (currentTr.parent != null)
        {
            currentTr = currentTr.parent;
            if (currentTr.GetComponent<UI_TopParentPopUp>() != null) break;
        }
        
        currentTr.gameObject.SetActive(false);
        
        Managers.Input.RemovePlayerMouseActions();
        Managers.Input.RollbackPlayerMouseActions();
    }

}
