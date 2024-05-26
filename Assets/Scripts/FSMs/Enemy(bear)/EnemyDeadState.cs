using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyDeadState : IStateBase
{
    private EnemyController _controller;
    private Animator _animator;
    public float Timer { get; set; }
    public EnemyDeadState(GameObject go) => _controller = go.GetComponent<EnemyController>();
    
    public void Init()
    {
        Timer = 5;
        _animator = _controller.GetComponentInChildren<Animator>();
    }

    public void StartState()
    {
        _animator.SetBool("Death", true);
        Timer += _animator.GetCurrentAnimatorClipInfo(0).Length;

        var targetSystem = Managers.Game.TargetingSystem;
        if(_controller.gameObject == targetSystem.Target) targetSystem.ClearTarget();
    }

    public void UpdateState()
    {
        Timer -= Time.deltaTime;
    }

    public void EndState()
    {
        _controller.DieAction();
        _controller.MakeItem();
        _controller.gameObject.SetActive(false);
    }
}
