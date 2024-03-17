using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : IStateBase
{
    private Animator _animator;
    private GameObject _controller;
    public EnemyIdleState(GameObject go) => _controller = go;
    
    public void Init()
    {
        _animator = _controller.GetComponentInChildren<Animator>();
    }

    public void StartState()
    {
        _animator.SetBool("Idle", true);
    }

    public void UpdateState()
    {
        
    }

    public void EndState()
    {
        _animator.SetBool("Idle", false);
    }
}
