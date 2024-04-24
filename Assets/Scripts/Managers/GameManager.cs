using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    private PoolingManager _poolingManager = new PoolingManager();
    private InventoryController _inventoryController = new InventoryController();
    private TargetingSystem _targetingSystem = new TargetingSystem();
    private SkillSystem _sKillSystem = new SkillSystem();

    public GameObject Player { get; private set; }
    public PoolingManager PoolingManager => _poolingManager;
    public InventoryController InventoryController => _inventoryController;
    public TargetingSystem TargetingSystem => _targetingSystem;
    public SkillSystem SkillSystem => _sKillSystem;

    public void Init()
    {
        Player = GameObject.FindWithTag("Player");

        _poolingManager.Init();
        _targetingSystem.Init();
        _inventoryController.Init();
        _sKillSystem.Init();
    }

    public void Update()
    {
        _targetingSystem.Update();
        _inventoryController.Update();
        _sKillSystem.Update();
    }
}
