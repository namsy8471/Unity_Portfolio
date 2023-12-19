using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DroppedItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private static Action _changeCursorForGrab;
    private static Action _changeCursorForGrabbing;
    private static Action _changeCursorForNormal;
    
    [SerializeField] private ItemData itemData;
    public ItemData ItemData
    {
        get => itemData;
        set => itemData = value;
    }

    private InventoryController inventoryController;

    private TextMesh itemNameText;
    private bool isSelected;
    
    private void Start()
    {
        inventoryController = FindObjectOfType(typeof(InventoryController)) as InventoryController;
        itemNameText = GetComponentInChildren<TextMesh>();
        
        itemNameText.text = itemData.ItemName;
        isSelected = false;
    }

    private void Update()
    {
        Transform tr = itemNameText.transform;
        tr.rotation = Quaternion.LookRotation( tr.position - Camera.main.transform.position );
    }
    
    void RootingThisItem()
    {
        if(inventoryController.SelectedItem != null) return;
        inventoryController.SendMessage("RootingItem", itemData.ItemName, SendMessageOptions.DontRequireReceiver);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(inventoryController.SelectedItem != null) return;
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            _changeCursorForGrabbing();
            
            if (Vector3.Distance(GameObject.Find("Player").transform.position, transform.position) > 1.0f)
            {
                isSelected = true;
            }
            else
            {
                RootingThisItem();
                Destroy(gameObject);
                _changeCursorForNormal();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(inventoryController.SelectedItem != null) return;
        if (isSelected && other.CompareTag("Player"))
        {
            RootingThisItem();
            Destroy(gameObject);
            _changeCursorForNormal();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(inventoryController.SelectedItem != null) return;
        _changeCursorForGrab();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(inventoryController.SelectedItem != null) return;
        _changeCursorForNormal();
    }

    public void RegisterHandler(Action grab, Action grabbing, Action backToNormal)
    {
        _changeCursorForGrab = grab;
        _changeCursorForGrabbing = grabbing;
        _changeCursorForNormal = backToNormal;
    }
}
