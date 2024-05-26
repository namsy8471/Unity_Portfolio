using System;
using System.Collections;
using System.Collections.Generic;
using Contents.Status;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR;
using Random = UnityEngine.Random;

public class EnemyAttackState : IStateBase
{
    enum AttackType
    {
        Atk1,
        Atk2,
        Atk3,
        Atk4,
        AtkFinish
    }

    private AttackType _attackType;

    private Animator _animator;
    private PlayerController _player;
    
    private float _attackRange;
    private int _maxAttackCount;
    private float _attackDownGauge;

    private EnemyController _controller;
    public float Timer { get; private set; }

    public EnemyAttackState(GameObject go) => _controller = go.GetComponent<EnemyController>();
    public void Init()
    {
        _animator = _controller.GetComponent<Animator>();
        _player = Managers.Game.Player.GetComponent<PlayerController>();
    }

    public void StartState()
    {
        if (_controller.Status.Hp <= 0)
        {
            _controller.ChangeState(EnemyController.EnemyState.Dead);
        }
        
        _animator.Play("Attack" + Random.Range(1,5));
        Timer = _animator.GetCurrentAnimatorStateInfo(0).length;
    }

    public void UpdateState()
    {
        Timer -= Time.deltaTime;
    }

    public void EndState()
    {
        
    }
    
    public void Attack()
    {
        var pos = _player.transform.position;
        pos.y = _controller.transform.position.y;
        
        _controller.transform.LookAt(pos);
        
        _player.GetDamage(_controller);
        
        if (_player.CurrentSkill is DefendSkill)
        {
            Timer += 4;
        }
    }
}
