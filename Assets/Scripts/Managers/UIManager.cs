using System.Collections;
using System.Collections.Generic;
using Contents.Status;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class UIManager
{
    // This Manager manage UI like Inventory, Button etc.
    // 이 매니저는 인벤토리, 버튼 등의 UI를 담당합니다.

    private Canvas _inventoryUI;
    private Canvas _uiBackground;
    private Canvas _statusUI;
    private Canvas _skillUI;
    private Canvas _dieUI;
    private List<Canvas> _canvasList = new List<Canvas>();
    
    private Slider _hpBar;
    private Slider _mpBar;
    private Slider _staminaBar;
    private Slider _hpBarInStatusWindow;
    private Slider _mpBarInStatusWindow;
    private Slider _staminaBarInStatusWindow;
    
    private Inventory_Main _mainInventory;
    private readonly List<Inventory_Base> _inventories = new List<Inventory_Base>();


    public Canvas InventoryUICanvas => _inventoryUI;
    public Canvas UIBackgroundCanvas => _uiBackground;
    public Canvas SkillUI => _skillUI;

    public Slider HpBar => _hpBar;
    public Slider MpBar => _mpBar;
    public Slider StaminaBar => _staminaBar;
    public Slider HpBarInStatusWindow => _hpBarInStatusWindow;
    public Slider MpBarInStatusWindow => _mpBarInStatusWindow;
    public Slider StaminaBarInStatusWindow => _staminaBarInStatusWindow;

    public TextMeshProUGUI Str { get; set; }
    public TextMeshProUGUI Int { get; set; }
    public TextMeshProUGUI Dex { get; set; }
    public TextMeshProUGUI Will { get; set; }
    public TextMeshProUGUI Luck { get; set; }
    public TextMeshProUGUI Damage { get; set; }
    public TextMeshProUGUI Def { get; set; }

    public Inventory_Main MainInventory => _mainInventory;
    public List<Inventory_Base> InventoryList => _inventories;

    public void Init()
    {
        CanvasInit();

        _mainInventory = _inventoryUI.GetComponentInChildren<Inventory_Main>();
        _mainInventory.Init();
        
        _hpBar = _uiBackground.transform.Find("ParentBackground/Bars/HPBar").GetComponent<Slider>();
        _mpBar = _uiBackground.transform.Find("ParentBackground/Bars/MPBar").GetComponent<Slider>();
        _staminaBar = _uiBackground.transform.Find("ParentBackground/Bars/StaminaBar").GetComponent<Slider>();

        _hpBarInStatusWindow = _statusUI.transform.Find("StatusWindow/WindowHandle/UI_Background/HPBar").GetComponent<Slider>();
        _mpBarInStatusWindow = _statusUI.transform.Find("StatusWindow/WindowHandle/UI_Background/MPBar").GetComponent<Slider>();
        _staminaBarInStatusWindow = _statusUI.transform.Find("StatusWindow/WindowHandle/UI_Background/StaminaBar").GetComponent<Slider>();
        
        InitializeText();
        
        _inventories.Add(_mainInventory);
        
        #region Key and Action Binding

        _uiBackground.GetComponentInChildren<UI_InventoryButton>().CloseOrOpen = CloseOrOpenInventory;
        _inventoryUI.GetComponentInChildren<UI_InventoryButton>().CloseOrOpen = CloseOrOpenInventory;
        Managers.Input.AddAction(Managers.Input.KeyButtonDown, Managers.Input.InventoryKey, CloseOrOpenInventory);

        _uiBackground.GetComponentInChildren<UI_StatusWindowButton>().CloseOrOpen = CloseOrOpenStatusWindow;
        _statusUI.GetComponentInChildren<UI_StatusWindowButton>().CloseOrOpen = CloseOrOpenStatusWindow;
        Managers.Input.AddAction(Managers.Input.KeyButtonDown, Managers.Input.CharacterStatusKey, CloseOrOpenStatusWindow);

        _uiBackground.GetComponentInChildren<UI_SkillWindowButton>().CloseOrOpen = CloseOrOpenSkillWindow;
        _skillUI.GetComponentInChildren<UI_SkillWindowButton>().CloseOrOpen = CloseOrOpenSkillWindow;
        Managers.Input.AddAction(Managers.Input.KeyButtonDown, Managers.Input.SkillWindowKey, CloseOrOpenSkillWindow);
        
        _uiBackground.GetComponentInChildren<UI_MainMenuButton>().CloseOrOpen = CloseOrOpenMenu;
        _uiBackground.GetComponentInChildren<UI_MainMenuButton>().transform.Find("MenuWindow/Background/GameQuitButton").
            GetComponent<Button>().onClick.AddListener(Application.Quit);
        
        #endregion
        
        _inventoryUI.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        _statusUI.gameObject.SetActive(false);
        _skillUI.gameObject.SetActive(false);
    }

    private void CanvasInit()
    {
        _inventoryUI = GameObject.Instantiate(Resources.Load<GameObject>
            ("Prefabs/UI/Inventory/UI_MainInventory")).GetComponent<Canvas>();
        _canvasList.Add(_inventoryUI);
        
        _statusUI = GameObject.Instantiate(Resources.Load<GameObject>
            ("Prefabs/UI/UI_StatusWindow/StatusWindowCanvas").GetComponent<Canvas>());
        _canvasList.Add(_statusUI);

        _skillUI = GameObject.Instantiate(Resources.Load<GameObject>
            ("Prefabs/UI/UI_SkillWindow/SkillWindowCanvas").GetComponent<Canvas>());
        _canvasList.Add(_skillUI);

        _uiBackground = GameObject.Instantiate(Resources.Load<GameObject>
            ("Prefabs/UI/UI_Background/UI_Background")).GetComponent<Canvas>();
        _canvasList.Add(_uiBackground);

        _dieUI = GameObject.Instantiate(Resources.Load<GameObject>
            ("Prefabs/UI/UI_Dead/DieUI").GetComponent<Canvas>());
        _dieUI.gameObject.SetActive(false);
        
        _dieUI.gameObject.GetComponentInChildren<Button>().onClick.AddListener(ResetGame);
    }

    public void DieCanvasActive()
    {
        foreach (var var in _canvasList)
        {
            var.gameObject.SetActive(false);
        }
        
        _dieUI.gameObject.SetActive(true);
    }

    public void WinCanvasActive()
    {
        DieCanvasActive();

        _dieUI.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Game Clear";
        _dieUI.gameObject.GetComponentInChildren<Button>().onClick.RemoveListener(ResetGame);
        _dieUI.gameObject.GetComponentInChildren<Button>().onClick.AddListener(BackToTitle);
    }

    private void ResetGame()
    {
        SceneManager.LoadScene("GameScene");
    }
    
    private void BackToTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }

    private void CloseOrOpenMenu()
    {
        _uiBackground.GetComponentInChildren<UI_MainMenuButton>().transform.Find("MenuWindow").gameObject.SetActive(
            !_uiBackground.GetComponentInChildren<UI_MainMenuButton>().transform.Find("MenuWindow").gameObject.activeSelf);
    }
    
    private void CloseOrOpenInventory()
    {
        var isMainInventoryOpenOrClose = _mainInventory.transform.parent.gameObject.activeSelf;
        
        foreach (var inven in _inventories)
        {
            inven.gameObject.transform.parent.gameObject.SetActive(!isMainInventoryOpenOrClose);
        }
        
        Managers.Input.RemovePlayerMouseActions();
        Managers.Input.RollbackPlayerMouseActions();
    }

    private void CloseOrOpenStatusWindow()
    {
        _statusUI.gameObject.SetActive(!_statusUI.gameObject.activeSelf);
        
        Managers.Input.RemovePlayerMouseActions();
        Managers.Input.RollbackPlayerMouseActions();
    }

    private void CloseOrOpenSkillWindow()
    {
        _skillUI.gameObject.SetActive(!_skillUI.gameObject.activeSelf);
        
        Managers.Input.RemovePlayerMouseActions();
        Managers.Input.RollbackPlayerMouseActions();
    }

    private void InitializeSlider(Slider slider, float maxValue)
    {
        slider.minValue = 0;
        slider.maxValue = maxValue;
        slider.value = maxValue;

        var text = slider.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        text.text = slider.value + "/" + slider.maxValue;
    }

    private void InitializeText()
    {
        Str = _statusUI.transform.Find("StatusWindow/WindowHandle/UI_Background/StrText/Str").GetComponent<TextMeshProUGUI>();
        Int = _statusUI.transform.Find("StatusWindow/WindowHandle/UI_Background/IntText/Int").GetComponent<TextMeshProUGUI>();
        Dex = _statusUI.transform.Find("StatusWindow/WindowHandle/UI_Background/DexText/Dex").GetComponent<TextMeshProUGUI>();
        Will = _statusUI.transform.Find("StatusWindow/WindowHandle/UI_Background/WillText/Will").GetComponent<TextMeshProUGUI>();
        Luck = _statusUI.transform.Find("StatusWindow/WindowHandle/UI_Background/LuckText/Luck").GetComponent<TextMeshProUGUI>();

        Damage = _statusUI.transform.Find("StatusWindow/WindowHandle/UI_Background/DamageText/Damage").GetComponent<TextMeshProUGUI>();
        Def = _statusUI.transform.Find("StatusWindow/WindowHandle/UI_Background/DefText/Def").GetComponent<TextMeshProUGUI>();
    }
    
    public void BindingSliderWithPlayerStatus()
    {
        InitializeSlider(_hpBar, Managers.Game.Player.GetComponent<PlayerController>().Status.MaxHp);
        InitializeSlider(_mpBar, Managers.Game.Player.GetComponent<PlayerController>().Status.MaxMp);
        InitializeSlider(_staminaBar, Managers.Game.Player.GetComponent<PlayerController>().Status.MaxStamina);
        
        InitializeSlider(_hpBarInStatusWindow, Managers.Game.Player.GetComponent<PlayerController>().Status.MaxHp);
        InitializeSlider(_mpBarInStatusWindow, Managers.Game.Player.GetComponent<PlayerController>().Status.MaxMp);
        InitializeSlider(_staminaBarInStatusWindow, Managers.Game.Player.GetComponent<PlayerController>().Status.MaxStamina);
    }
    
    public void ChangeSliderValue(Slider slider, float value)
    {
        slider.value = value;

        var text = slider.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        text.text = Mathf.Floor(slider.value) + "/" + slider.maxValue;
    }
    
    public void ChangeSliderMaxValue(Slider slider, float maxValue)
    {
        slider.maxValue = maxValue;
        slider.value = maxValue;

        var text = slider.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        text.text = Mathf.Floor(slider.value) + "/" + slider.maxValue;
    }

    public void ChangeText(TextMeshProUGUI text, int value)
    {
        text.text = value.ToString();
    }
    
    public void ChangeTextForDamage(TextMeshProUGUI text, int minValue, int maxvalue)
    {
        text.text = minValue + " ~ " + maxvalue;
    }
}
