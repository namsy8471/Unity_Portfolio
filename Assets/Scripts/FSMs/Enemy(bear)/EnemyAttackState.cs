using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR;

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
    private GameObject _player;
    
    private float _attackRange;
    private int _maxAttackCount;
    private float _attackDownGauge;
    private int _attackCount;
    private float _attackTimer;
    private float _attackDelay;
    private float _lastAttackDelay;

    private bool _isAtkFinished;

    private GameObject _controller;

    public EnemyAttackState(GameObject go) => _controller = go;
    public void Init()
    {
        _attackRange = 2;
        _maxAttackCount = 3;
        _attackDownGauge = 100 / (float) _maxAttackCount + 1;
        
        _attackCount = 0;
        _attackTimer = 0;
        
        _attackDelay = 0.8f;
        _lastAttackDelay = 3.0f;

        _isAtkFinished = false;
        
        _animator = _controller.GetComponentInChildren<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    public void StartState()
    {
        _isAtkFinished = false;
        ChangeState(AttackType.Atk1);
    }

    public void UpdateState()
    {
        switch (_attackType)
        {
            case AttackType.Atk1:
                
                if (_attackCount < _maxAttackCount && _attackTimer >= _attackDelay)
                {
                    ChangeState(AttackType.Atk2);
                    break;
                }
                
                else if (_attackCount == _maxAttackCount && _attackTimer >= _lastAttackDelay)
                {
                    ChangeState(AttackType.AtkFinish);
                    break;
                }
                
                _attackTimer += Time.deltaTime;
                break;
            case AttackType.Atk2:
                
                if (_attackCount < _maxAttackCount && _attackTimer >= _attackDelay)
                {
                    ChangeState(AttackType.Atk3);
                    break;
                }
                
                else if (_attackCount == _maxAttackCount && _attackTimer >= _lastAttackDelay)
                {
                    ChangeState(AttackType.AtkFinish);
                    break;
                }
                _attackTimer += Time.deltaTime;
                break;
            case AttackType.Atk3:
                
                if (_attackCount < _maxAttackCount && _attackTimer >= _attackDelay)
                {
                    ChangeState(AttackType.Atk4);
                    break;
                }
                
                else if (_attackCount == _maxAttackCount && _attackTimer >= _lastAttackDelay)
                {
                    ChangeState(AttackType.AtkFinish);
                    break;
                }
                _attackTimer += Time.deltaTime;
                
                break;
            case AttackType.Atk4:
                
                if (_attackCount == _maxAttackCount && _attackTimer >= _lastAttackDelay)
                {
                    ChangeState(AttackType.AtkFinish);
                    break;
                }
                
                _attackTimer += Time.deltaTime;
                break;
            
            case AttackType.AtkFinish:

                
                if (_attackTimer >= _lastAttackDelay)
                {
                    _isAtkFinished = true;
                }
                
                _attackTimer += Time.deltaTime;
                
                break;

            default:
                break;
        }
    }

    public void EndState()
    {
        // Debug.Log("Enemy Attack State End!");
    }

    private void Attack()
    {
        _player.SendMessage("GetDamage", _attackDownGauge, SendMessageOptions.DontRequireReceiver);
    }

    private void AttackFinish()
    {
        ChangeState(AttackType.AtkFinish);
    }

    public bool GetAtkDone()
    {
        return _isAtkFinished;
    }
    
    private void ChangeState(AttackType type)
    {
        switch (_attackType)
        {
            case AttackType.Atk1:
                break;
            case AttackType.Atk2:
                break;
            case AttackType.Atk3:
                break;
            case AttackType.Atk4:
                break;
            case AttackType.AtkFinish:
                break;
            default:
                break;
        }

        _attackType = type;
        _attackTimer = 0;

        switch (_attackType)
        {
            case AttackType.Atk1:
                _attackCount = 1;
                _animator.SetTrigger("Attack1");
                Attack();
                break;
            
            case AttackType.Atk2:
                _attackCount = 2;
                _animator.SetTrigger("Attack2");
                Attack();

                break;
            
            case AttackType.Atk3:
                _attackCount = 3;
                _animator.SetTrigger("Attack3");
                Attack();

                break;
            
            case AttackType.Atk4:
                _attackCount = 4;
                _animator.SetTrigger("Attack5");
                Attack();

                break;
            
            case AttackType.AtkFinish:
                _attackCount = 0;
                _animator.SetTrigger("Buff");
                
                break;
            default:
                break;
        }
    }
    
    public float GetAttackRange()
    {
        return _attackRange;
    }
}
