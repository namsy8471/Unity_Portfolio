using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    // 아이템 크기 (길이 및 높이)
    [SerializeField] private int width;
    [SerializeField] private int height;

    public int Width => width;
    public int Height => height;

    [SerializeField] private string _itemName;
    [SerializeField] private Sprite _itemIcon;
    [SerializeField] private GameObject _itemModelingForDropping;
    [SerializeField] private GameObject _itemModelingForEquipment;
    [SerializeField] private float _groundYOffset;

    public Sprite ItemIcon => _itemIcon;
    public string ItemName => _itemName;
    public GameObject ItemModelingForDropping => _itemModelingForDropping;
    public GameObject ItemModelingForEquipment => _itemModelingForEquipment;
    public float GroundYOffset => _groundYOffset;
    protected InventoryController InventoryController => Managers.Game.InventoryController;

    // 이미지 크기
    public void GetImageSize(out int x, out int y)
    {
        x = (int)(width * ItemGrid.TileSize.Width);
        y = (int)(height * ItemGrid.TileSize.Height);
    }
    
    // 그리드 사이즈 구하기
    public void SetGridSize()
    {
        var size = _itemIcon.rect.size;
        
        width = (int)(size.x / ItemGrid.TileSize.Width);
        height = (int)(size.y / ItemGrid.TileSize.Height);
    }

    public virtual void Init() {}       // 최초 아이템 습득 시 사용되는 함수
    public virtual void UseItem() {}    // 아이템 사용 시 사용되는 함수
    public virtual void DropItem(){}    // 아이템 드랍 시 사용되는 함수
    public virtual void PickUpItem(){}    // 아이템을 인벤토리에서 집을 시 사용되는 함수
}
