using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUpperBar : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    private RectTransform rectTransform;
    private InventoryController inventoryController;
    private MoveState moveState;
    
    private static Action changeMousePointToGrabbing;
    private static Action changeMousePointToNormal;

    private Vector2 firstClickedPos;
    private Vector2 distanceWithInventory;
    
    private void Start()
    {
        rectTransform = transform.parent.GetComponent<RectTransform>();
        inventoryController = FindObjectOfType(typeof(InventoryController)) as InventoryController;
        moveState = FindObjectOfType(typeof(MoveState)) as MoveState;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if (inventoryController.SelectedItem != null) return;
        
        var newPos = new Vector2(eventData.position.x - distanceWithInventory.x, eventData.position.y - distanceWithInventory.y);
        
        rectTransform.position = newPos;
        changeMousePointToGrabbing();
        moveState.SendMessage("LockMakingWaypoint", SendMessageOptions.DontRequireReceiver);
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        changeMousePointToNormal();
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            firstClickedPos = eventData.position;
            distanceWithInventory = new Vector2(firstClickedPos.x - rectTransform.position.x,
                firstClickedPos.y - rectTransform.position.y);
        }
    }
    
    // 인벤토리 버튼 이벤트 함수
    public void InventoryCloseButtonClick()
    {
        transform.parent.gameObject.transform.parent.gameObject.SetActive(false);
    }
    
    public void RegisterHandler(Action grabbing, Action normal)
    {
        changeMousePointToGrabbing = grabbing;
        changeMousePointToNormal = normal;
    }
    
}
