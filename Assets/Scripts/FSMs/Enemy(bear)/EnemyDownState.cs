using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDownState : IStateBase
{
    private EnemyController _controller;
    private Animator _animator;
    private Rigidbody _rb;

    public float Timer { get; private set; }
    public EnemyDownState(GameObject go) => _controller = go.GetComponent<EnemyController>();
    
    public void Init()
    {
        _animator = _controller.GetComponentInChildren<Animator>();
        _rb = _controller.GetComponent<Rigidbody>();
    }

    public void StartState()
    {
        _rb.AddForce(-_controller.transform.forward * 600, ForceMode.Impulse);
        _controller.DownGauge = 0;
        Timer = _animator.GetCurrentAnimatorStateInfo(0).length + 1f;
        _controller.InvincibleTimer = 3.5f;
        
        if (_controller.Status.Hp <= 0)
        {
            _controller.ChangeState(EnemyController.EnemyState.Dead);
        }
    }

    public void UpdateState()
    {
        Timer -= Time.deltaTime;
    }

    public void EndState()
    {
        
    }
}
