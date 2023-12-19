using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemDataBackpack : ItemData
{
    [SerializeField] private GameObject inventoryPrefab;
    [SerializeField] private GameObject inventory;
    
    private ItemGrid itemGrid;
    
    private bool isOpen;
    
    public override void Init()
    {
        isOpen = false;
        
        //가방 만들기
        if (inventory == null)
        {
            inventory = Instantiate(inventoryPrefab, new Vector2(1280, 720), Quaternion.identity,
                GameObject.Find("MainInventory").transform);
            inventory.transform.SetAsFirstSibling();
            inventory.GetComponentInChildren<ItemGrid>().SetGrid(6, 6);
            inventory.gameObject.SetActive(isOpen);
        }
        
        Debug.Log("가방 만들어짐!");

    }
    public override void UseItem()
    {
        // 가방 열기 및 닫기
        isOpen = !isOpen;
        inventory.gameObject.SetActive(isOpen);
        Debug.Log("가방 사용됨!");
    }

    public override void DropItem()
    {
        Debug.Log("가방 버려짐!");
        Destroy(inventory.gameObject);
    }

    public override void PickUpItem()
    {
        isOpen = false;
        inventory.gameObject.SetActive(isOpen);
    }
}
