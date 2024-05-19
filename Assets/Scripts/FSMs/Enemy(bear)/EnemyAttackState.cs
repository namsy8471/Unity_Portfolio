using System;
using System.Collections;
using System.Collections.Generic;
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

    private GameObject _controller;
    public float Timer { get; private set; }

    public EnemyAttackState(GameObject go) => _controller = go;
    public void Init()
    {
        _animator = _controller.GetComponent<Animator>();
        _player = Managers.Game.Player.GetComponent<PlayerController>();
    }

    public void StartState()
    {
        _animator.Play("Attack" + Random.Range(1,5));
        Timer = _animator.GetCurrentAnimatorStateInfo(0).length - 0.2f;
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
        
        _player.GetDamage(_controller.GetComponent<EnemyController>());
        
        if (_player.CurrentSkill is DefendSkill)
        {
            Timer += 4;
        }
    }
}
