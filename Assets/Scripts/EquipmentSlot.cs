using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Image image;
    private RectTransform rectTransform;
    private InventoryController inventoryController;
    private Dictionary<string, GameObject> equipmentInstanceDictionary;
    
    private ItemDataEquipment equippedItemData;
    private InventoryItem equippedItem;
    private ItemDataEquipment equipmentItemData;
    private InventoryItem equipmentItem;

    
    [SerializeField] protected ItemDataEquipment.Type equipmentSlotType;
    [SerializeField] protected GameObject defaultEquipmentGameObject;
    
    void Start()
    {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        inventoryController = FindObjectOfType(typeof (InventoryController)) as InventoryController;

        equipmentInstanceDictionary = new Dictionary<string, GameObject>();
        
        equippedItem = null;
        equipmentItemData = null;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (inventoryController.SelectedItem != null)
        {
            equipmentItem = inventoryController.SelectedItem;
            equipmentItemData = (ItemDataEquipment) equipmentItem.ItemData;
            
            if (equipmentItemData.EquipmentType == ItemDataEquipment.Type.None) return;
            ItemDataEquipment.Type equipmentType = equipmentItemData.EquipmentType;
            
            Debug.Log("장비 타입 : " + equipmentType + "슬롯 타입 : " + equipmentSlotType);
            if (equipmentItemData.EquipmentType == equipmentSlotType) image.color = Color.gray;
        }
        else if (equippedItem != null)
        {
            image.color = Color.gray;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = Color.white;
        equipmentItem = null;
        equipmentItemData = null;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        // 장비
        if (equippedItem == null && equipmentItemData.EquipmentType == equipmentSlotType)
        {
            equippedItem = equipmentItem;
            equippedItemData = (ItemDataEquipment) equippedItem.ItemData;
            inventoryController.SendMessage("MakeSelectedItemToNull", SendMessageOptions.DontRequireReceiver);
            
            // 장비 이미지 중앙
            SetEquipmentImageCenter();
            
            // 무기 모델 변경
            defaultEquipmentGameObject.SetActive(false);
            if (equipmentInstanceDictionary.ContainsKey(equippedItemData.ItemName) == false)
            {
                var equipmentInstance = Instantiate(equippedItemData.ItemModelingForEquipment,
                    defaultEquipmentGameObject.gameObject.transform.parent);
                equipmentInstanceDictionary.Add(equippedItemData.ItemName, equipmentInstance);
            }
            else
            {
                equipmentInstanceDictionary[equippedItemData.ItemName].SetActive(true);
            }

            Debug.Log("장비 됨!");
        }
        
        // 장비 해제
        else if (equippedItem != null)
        {
            // 장비 해제
            if (inventoryController.SelectedItem == null)
            {
                inventoryController.SendMessage("GetItemFromEquipmentSlot", equippedItem,
                    SendMessageOptions.DontRequireReceiver);

                equipmentItem = equippedItem;
                equipmentItemData = equippedItemData;
                
                equippedItem = null;
                equippedItemData = null;
                
                // // 장비 모델 교체
                equipmentInstanceDictionary[equipmentItemData.ItemName].SetActive(false);
                
                
                defaultEquipmentGameObject.SetActive(true);
                Debug.Log("장비 해제됨!");
            }
            // 장비 교체
            else if (inventoryController.SelectedItem != null && equipmentItemData.EquipmentType == equipmentSlotType)
            {
                // 장비 정보 교체
                var temp = equipmentItem;

                equipmentItem = equippedItem;
                equipmentItemData = equippedItemData;
                
                equippedItem = temp;
                equippedItemData = (ItemDataEquipment) equippedItem.ItemData;
                
                inventoryController.SendMessage("GetItemFromEquipmentSlot", equipmentItem,
                    SendMessageOptions.DontRequireReceiver);
                
                // 무기 이미지 중앙 맞추기
                SetEquipmentImageCenter();
                
                // 장비 모델 교체
                if (equipmentInstanceDictionary.ContainsKey(equippedItemData.ItemName) == false)
                {
                    var equipmentInstance = Instantiate(equippedItemData.ItemModelingForEquipment,
                        defaultEquipmentGameObject.gameObject.transform.parent);
                    equipmentInstanceDictionary.Add(equippedItemData.ItemName, equipmentInstance);
                    
                    equipmentInstanceDictionary[equipmentItemData.ItemName].SetActive(false);
                }
                else
                {
                    equipmentInstanceDictionary[equipmentItemData.ItemName].SetActive(false);
                    equipmentInstanceDictionary[equippedItemData.ItemName].SetActive(true);
                }
                Debug.Log("장비 교체");
            }
        }
    }

    private void SetEquipmentImageCenter()
    {
        var weaponRectTransform = equippedItem.GetComponent<RectTransform>();

        weaponRectTransform.SetParent(rectTransform);

        // 무기 이미지 중앙 맞추기
        var size = rectTransform.sizeDelta;
        var width = size.x / 2;
        var height = size.y / 2;

        weaponRectTransform.localPosition = new Vector2(width, -height);
    }
}
