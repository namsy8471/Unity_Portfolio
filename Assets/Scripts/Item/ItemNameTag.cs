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
        if (GetComponentInChildren<TextMesh>() != null)
        {
            _itemNameText = GetComponentInChildren<TextMesh>();
        }

        else
        {
            GameObject go = Instantiate(new GameObject(name: "ItemName"), gameObject.transform);
            go.name = "ItemName";
            _itemNameText = go.AddComponent<TextMesh>();
        }
        
        _itemNameText.text = itemData.ItemName;
        _itemNameText.characterSize = 0.2f;
        _itemNameText.anchor = TextAnchor.LowerCenter;
        _itemNameText.alignment = TextAlignment.Center;
    }

    private void Update()
    {
        _itemNameText.transform.rotation = Camera.main.transform.rotation;
    }
}
