using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyIdleState : IStateBase
{
    private Animator _animator;
    private EnemyController _controller;
    public EnemyIdleState(GameObject go) => _controller = go.GetComponent<EnemyController>();
    
    public void Init()
    {
        _animator = _controller.GetComponentInChildren<Animator>();
    }

    public void StartState()
    {
        _animator.SetBool("Idle", true);
        
        if (_controller.Status.Hp <= 0)
        {
            _controller.ChangeState(EnemyController.EnemyState.Dead);
            return;
        }
    }

    public void UpdateState()
    {
        if (_controller.DownGauge > 0)
            _controller.DownGauge -= Time.deltaTime;
        else _controller.DownGauge = 0;
    }

    public void EndState()
    {
        _animator.SetBool("Idle", false);
    }
}
