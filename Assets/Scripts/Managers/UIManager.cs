using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager
{
    // This Manager manage UI like Inventory, Button etc.
    // 이 매니저는 인벤토리, 버튼 등의 UI를 담당합니다.

    private Canvas _inventoryUI;
    private Canvas _UIBackground;
    private UI_InventoryButton _uiInventoryButton;

    private Slider _hpBar;
    private Slider _mpBar;
    private Slider _staminaBar;
    
    private Inventory_Main _mainInventory;
    private List<Inventory_Base> _inventories = new List<Inventory_Base>();


    public Canvas InventoryUICanvas => _inventoryUI;
    public Canvas UIBackgroundCanvas => _UIBackground;

    public Slider HpBar => _hpBar;
    public Slider MpBar => _mpBar;
    public Slider StaminaBar => _staminaBar;

    public Inventory_Main MainInventory => _mainInventory;
    public List<Inventory_Base> InventoryList => _inventories;

    private bool _isInventoryClosed = false;


    public void Init()
    {
        _inventoryUI = GameObject.Instantiate(Resources.Load<GameObject>
                ("Prefabs/UI/Inventory/UI_MainInventory")).GetComponent<Canvas>();

        _UIBackground = GameObject.Instantiate(Resources.Load<GameObject>
            ("Prefabs/UI/UI_Background/UI_Background")).GetComponent<Canvas>();
        
        _mainInventory = _inventoryUI.GetComponentInChildren<Inventory_Main>();
        _mainInventory.Init();

        _hpBar = GameObject.Find("HPBar").GetComponent<Slider>();
        _mpBar = GameObject.Find("MPBar").GetComponent<Slider>();
        _staminaBar = GameObject.Find("StaminaBar").GetComponent<Slider>();
        
        _inventories.Add(_mainInventory);

        _uiInventoryButton = _UIBackground.GetComponentInChildren<UI_InventoryButton>();
        _uiInventoryButton.CloseOrOpenInventory = CloseOrOpenInventory;
        
        Managers.Input.AddAction(Managers.Input.KeyButtonDown, Managers.Input.InventoryKey, CloseOrOpenInventory);
    }

    
    
    private void CloseOrOpenInventory()
    {
        _isInventoryClosed = !_isInventoryClosed;
        foreach (var inven in _inventories)
        {
            inven.gameObject.transform.parent.gameObject.SetActive(_isInventoryClosed);
            Managers.Input.RemovePlayerMouseActions();
            Managers.Input.RollbackPlayerMouseActions();
        }
    }

    private void InitializeSlider(Slider slider, float maxValue)
    {
        slider.minValue = 0;
        slider.maxValue = maxValue;
        slider.value = maxValue;

        var text = slider.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        text.text = slider.value + "/" + slider.maxValue;
    }
    
    public void BindingSliderWithPlayerStatus()
    {
        InitializeSlider(_hpBar, Managers.Game.Player.GetComponent<Stat>().MaxHp);
        InitializeSlider(_mpBar, Managers.Game.Player.GetComponent<Stat>().MaxMp);
        InitializeSlider(_staminaBar, Managers.Game.Player.GetComponent<Stat>().MaxStamina);
    }

    public void ChangeSliderValue(Slider slider, float value)
    {
        slider.value = value;

        var text = slider.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        text.text = Mathf.Floor(slider.value) + "/" + slider.maxValue;
    }
}
