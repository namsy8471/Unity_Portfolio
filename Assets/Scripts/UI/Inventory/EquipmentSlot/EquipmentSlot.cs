using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Image _image;
    private RectTransform _rectTransform;
    private InventoryController _inventoryController;
    
    protected InventoryItem CurrentEquipmentItem;
    private InventoryItem _equipmentItemOnMouse;
    private InventoryItem _tempForSwitching;
    
    protected ItemDataEquipment.Type EquipmentSlotType;
    protected GameObject DefaultEquipmentGameObject;
    
    void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        _image = GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();
        _inventoryController = Managers.Game.InventoryController;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        MousePointerEnterInEquipmentSlot();
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        MousePointerExitFromEquipmentSlot();
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (CurrentEquipmentItem == null &&
            (_equipmentItemOnMouse.ItemData as ItemDataEquipment).EquipmentType == EquipmentSlotType)
        {
            EquipItem(isSwitching: false);
        }
        
        else if (CurrentEquipmentItem != null)
        {
            if (_inventoryController.SelectedItem == null)
            {
                UnequipItem(isSwitching: false);
            }
            else if (_inventoryController.SelectedItem != null
                     && (_equipmentItemOnMouse.ItemData as ItemDataEquipment).EquipmentType == EquipmentSlotType)
            {
                SwapEquipment();
            }
        }
    }
    
    private void MousePointerEnterInEquipmentSlot()
    {
        if (_inventoryController.SelectedItem != null)
        {
            _equipmentItemOnMouse = _inventoryController.SelectedItem;
            
            if ((_equipmentItemOnMouse.ItemData as ItemDataEquipment) == null) return;
            ItemDataEquipment.Type equipmentType = (_equipmentItemOnMouse.ItemData as ItemDataEquipment).EquipmentType;
            
            Debug.Log("장비 타입 : " + equipmentType + "슬롯 타입 : " + EquipmentSlotType);
            if ((_equipmentItemOnMouse.ItemData as ItemDataEquipment).EquipmentType == EquipmentSlotType)
                _image.color = Color.gray;
        }
        else if (CurrentEquipmentItem != null)
        {
            _image.color = Color.gray;
        }
    }
    
    private void MousePointerExitFromEquipmentSlot()
    {
        _image.color = Color.white;
        _equipmentItemOnMouse = null;
    }
    
    protected virtual void EquipItem(bool isSwitching)
    {
        Managers.Game.PoolingManager.SwitchEquipmentModeling(EquipmentSlotType,
            null,
            _tempForSwitching ?
                _tempForSwitching.ItemData as ItemDataEquipment :
                _equipmentItemOnMouse.ItemData as ItemDataEquipment);
        
        CurrentEquipmentItem = _tempForSwitching ? _tempForSwitching : _equipmentItemOnMouse;

        if (isSwitching == false)
        {
            _inventoryController.MakeSelectedItemToNull();
            _equipmentItemOnMouse = null;
        }
        
        else
        {
            _inventoryController.GetItemFromEquipmentSlot(_equipmentItemOnMouse);
            _tempForSwitching = null;
        }

        SetEquipmentImageCenter();
        
        Debug.Log("장비 됨!");
    }
    
    protected virtual void UnequipItem(bool isSwitching)
    {
        Managers.Game.PoolingManager.SwitchEquipmentModeling(EquipmentSlotType,
            CurrentEquipmentItem.ItemData as ItemDataEquipment, 
            null);

        _tempForSwitching = _equipmentItemOnMouse;
        _equipmentItemOnMouse = CurrentEquipmentItem;

        if (isSwitching == false)
        {
            _inventoryController.GetItemFromEquipmentSlot(CurrentEquipmentItem);
            _tempForSwitching = null;
        }
        
        CurrentEquipmentItem = null;
        
        Debug.Log("장비 해제됨!");
    }
    
    private void SwapEquipment()
    {
        UnequipItem(isSwitching: true);
        EquipItem(isSwitching: true);

        Debug.Log("장비 교체");
    }
    
    private void SetEquipmentImageCenter()
    {
        var weaponRectTransform = CurrentEquipmentItem.GetComponent<RectTransform>();

        weaponRectTransform.SetParent(_rectTransform);

        // 무기 이미지 중앙 맞추기
        var size = _rectTransform.sizeDelta;
        var width = size.x / 2;
        var height = size.y / 2;

        weaponRectTransform.localPosition = new Vector2(width, -height);
    }
}
