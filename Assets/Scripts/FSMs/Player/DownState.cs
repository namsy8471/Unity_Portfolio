using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DownState : IStateBase
{
    private PlayerController _controller;
    private Animator _animator;
    private Rigidbody _rb;

    public float Timer { get; private set; }

    public void Init()
    {
        _controller = Managers.Game.Player.GetComponent<PlayerController>();
        _animator = _controller.GetComponent<Animator>();
        _rb = _controller.GetComponent<Rigidbody>();
    }

    public void StartState()
    {
        _controller.DownGauge = 0;
        
        _animator.Play("Down");
        _rb.AddForce(-_controller.transform.forward * 500.0f, ForceMode.Impulse);

        Timer = _animator.GetCurrentAnimatorStateInfo(0).length;

        if (_controller.Status.Hp <= 0)
        {
            _controller.ChangeState(PlayerController.PlayerState.Dead);
            return;
        }
        
        _controller.InvincibleTimer = Timer + 3.0f;
    }

    public void UpdateState()
    {
        Timer -= Time.deltaTime;
    }

    public void EndState()
    {
        
    }
}
