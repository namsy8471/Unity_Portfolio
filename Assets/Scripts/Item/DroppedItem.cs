using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DroppedItem : MonoBehaviour, IPointerClickHandler
{
    
    
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
        inventoryController = Managers.Game.InventoryController;
        itemNameText = GetComponentInChildren<TextMesh>();
        
        itemNameText.text = itemData.ItemName;
        isSelected = false;
    }

    private void Update()
    {
        itemNameText.transform.rotation = Camera.main.transform.rotation;
    }
    
    void LootingThisItem()
    {
        if(inventoryController.SelectedItem != null) return;
        inventoryController.LootingItem(itemData.ItemName);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(inventoryController.SelectedItem != null) return;
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            //_changeCursorForGrabbing?.Invoke();
            
            if (Vector3.Distance(Managers.Game.Player.transform.position, transform.position) > 1.0f)
            {
                isSelected = true;
            }
            else
            {
                LootingThisItem();
                Destroy(gameObject);
                //_changeCursorForNormal();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("트리거 들어옴");
        if(inventoryController.SelectedItem != null) return;
        Debug.Log("인벤토리 컨트롤러 selectedItem != null");

        if (isSelected && other.CompareTag("Player"))
        {
            LootingThisItem();
            Destroy(gameObject);
           // _changeCursorForNormal?.Invoke();
        }
    }


}
