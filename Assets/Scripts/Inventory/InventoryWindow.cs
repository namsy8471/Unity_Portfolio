using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryWindow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private MoveState moveState;

    private void Start()
    {
        moveState = FindObjectOfType(typeof(MoveState)) as MoveState;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        moveState.SendMessage("LockMakingWaypoint", SendMessageOptions.DontRequireReceiver);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        moveState.SendMessage("UnlockMakingWaypoint", SendMessageOptions.DontRequireReceiver);
    }

    private void OnDisable()
    {
        moveState.SendMessage("UnlockMakingWaypoint", SendMessageOptions.DontRequireReceiver);
    }
}
