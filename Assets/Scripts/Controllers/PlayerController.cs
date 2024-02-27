using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    private enum PlayerState
    {
        Idle,
        Move,
        Attack,
        GetDamage
    }

    private enum Posture
    {
        Normal,
        Battle
    }

    private readonly IdleState _idleState = new IdleState();
    private readonly MoveState _moveState = new MoveState();
    private readonly AttackState _attackState = new AttackState();
    private readonly GetDamageState _getDamageState = new GetDamageState();
    private TargetingSystem _targetingSystem;
    
    private bool isBattle;

    private Animator _animator;

    [SerializeField] private PlayerState currentState;
    private Posture _posture;
    
    private void Start()
    {
        isBattle = false;

        _targetingSystem = Managers.Game.TargetingSystem;
        
        _animator = GetComponentInChildren<Animator>();
        
        _idleState.Init();
        _moveState.Init();
        _attackState.Init();
        _getDamageState.Init();
    }

    private void Update()
    {
        Debug.Log("플레이어 FSM = " + currentState);
        
        switch (_posture)
        {
            case Posture.Battle:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    ChangeState(Posture.Normal);
                    break;
                }
                
                break;
            
            case Posture.Normal:
                if (Input.GetKeyDown(KeyCode.Space) || _targetingSystem.IsCurrentTargetExist())
                {
                    ChangeState(Posture.Battle);
                    break;
                }
                
                break;
        }

        switch (currentState)
        {
            case PlayerState.Idle:
                
                if (Input.GetMouseButton(0) || Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
                {
                    ChangeState(PlayerState.Move);
                    break;
                }

                if (_targetingSystem.IsCurrentTargetExist() &&
                    Vector3.Distance(_targetingSystem.Target.transform.position, transform.position) < _attackState.GetAtkRange())
                {
                    ChangeState(PlayerState.Attack);
                    break;
                }
                
                _idleState.UpdateState();

                break;
            
            case PlayerState.Attack:

                if (!_targetingSystem.IsCurrentTargetExist())
                {
                    ChangeState(PlayerState.Idle);    
                }
                
                _attackState.UpdateState();

                break;
            
            case PlayerState.GetDamage:
                
                _getDamageState.UpdateState();
                
                break;
            
            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case PlayerState.Move:

                if (_moveState.IsMoveDone)
                {
                    ChangeState(PlayerState.Idle);
                    break;
                }

                if (_targetingSystem.IsCurrentTargetExist() &&
                    Vector3.Distance(_targetingSystem.Target.transform.position, transform.position) < _attackState.GetAtkRange())
                {
                    ChangeState(PlayerState.Attack);
                    break;
                }
                
                _moveState.UpdateState();

                break;
            
            default:
                break;
        }
    }

    
    private void ChangeState(PlayerState newState)
    {
        switch (currentState)
        {
            case PlayerState.Idle:
                _idleState.EndState();
                break;
            
            case PlayerState.Move:
                _moveState.EndState();
                break;
            
            case PlayerState.Attack:
                _attackState.EndState();
                break;
            
            case PlayerState.GetDamage:
                _getDamageState.EndState();
                break;
            
            default:
                break;
        }
        
        currentState = newState;

        switch (currentState)
        {
            case PlayerState.Idle:
                _idleState.StartState();
                break;
            
            case PlayerState.Move:
                _moveState.StartState();
                break;
            
            case PlayerState.Attack:
                _attackState.StartState();
                break;
            
            case PlayerState.GetDamage:
                _getDamageState.StartState();
                break;
            
            default:
                break;
        }
    }

    private void ChangeState(Posture mode)
    {
        switch (_posture)
        {
            case Posture.Normal:
                break;
            case Posture.Battle:
                break;
            default:
                break;
        }

        _posture = mode;
        
        switch (_posture)
        {
            case Posture.Normal:
                isBattle = false;
                _animator.SetBool("isBattle", isBattle);
                break;
            case Posture.Battle:
                isBattle = true;
                _animator.SetBool("isBattle", isBattle);
                break;
            default:
                break;
        }    
    }
    
    private void GetDamage(float value)
    {
        // Debug.Log("GetDamage 매소드 작동");
        ChangeState(PlayerState.GetDamage);
        gameObject.SendMessage("AddDownGauge", value, SendMessageOptions.DontRequireReceiver);
    }

    private void BackToIdle()
    {
        ChangeState(PlayerState.Idle);
    }
    
    
}
