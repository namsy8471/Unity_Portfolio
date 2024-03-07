using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemNameTag : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    public ItemData ItemData
    {
        get => itemData;
        set => itemData = value;
    }

    private TextMesh _itemNameText;
    
    private void Start()
    {
        _itemNameText = GetComponentInChildren<TextMesh>();
        
        _itemNameText.text = itemData.ItemName;
    }

    private void Update()
    {
        _itemNameText.transform.rotation = Camera.main.transform.rotation;
    }
}
