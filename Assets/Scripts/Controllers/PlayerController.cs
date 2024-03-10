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

    private readonly IdleState _idleState = new IdleState();
    private readonly MoveState _moveState = new MoveState();
    private readonly AttackState _attackState = new AttackState();
    private readonly GetDamageState _getDamageState = new GetDamageState();

    public MoveState MoveState => _moveState;

    private bool isBattle;

    private Animator _animator;
    
    private List<Action> _playerMouseDownActions = new List<Action>();
    private List<Action> _playerMousePressedActions = new List<Action>();
    public List<Action> PlayerMouseDownActions => _playerMouseDownActions;
    public List<Action> PlayerMousePressedActions => _playerMousePressedActions;
    
    [SerializeField] private PlayerState currentState;
    
    private void Start()
    {
        isBattle = false;
        
        _animator = GetComponentInChildren<Animator>();

        #region KeyBinding in InputManager
        Managers.Input.KeyButtonDown.Add(Managers.Input.PostureChangeKey, PostureChangeFunction);
        
        Managers.Input.AddAction(Managers.Input.KeyButtonPressed, Managers.Input.MoveForwardKey, ChangeToMoveState);
        Managers.Input.AddAction(Managers.Input.KeyButtonPressed, Managers.Input.MoveBackwardKey, ChangeToMoveState);
        Managers.Input.AddAction(Managers.Input.KeyButtonPressed, Managers.Input.MoveLeftKey, ChangeToMoveState);
        Managers.Input.AddAction(Managers.Input.KeyButtonPressed, Managers.Input.MoveRightKey, ChangeToMoveState);
        
        Managers.Input.LMBPressed += ChangeToMoveState;
        
        _playerMousePressedActions.Add(ChangeToMoveState);
        #endregion
        
        _idleState.Init();
        _moveState.Init();
        _attackState.Init();
        _getDamageState.Init();
    }

    private void Update()
    {
        Debug.Log("플레이어 FSM = " + currentState);

        switch (currentState)
        {
            case PlayerState.Idle:
                
                if (Managers.Game.TargetingSystem.IsCurrentTargetExist() &&
                    Vector3.Distance(Managers.Game.TargetingSystem.Target.transform.position, transform.position) < _attackState.GetAtkRange())
                {
                    ChangeState(PlayerState.Attack);
                    break;
                }
                
                _idleState.UpdateState();

                break;
            
            case PlayerState.Attack:

                if (!Managers.Game.TargetingSystem.IsCurrentTargetExist())
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

    private void PostureChangeFunction()
    {
        if (Managers.Game.TargetingSystem.IsCurrentTargetExist())
        {
            isBattle = true;
            _animator.SetBool("isBattle", isBattle);
            return;
        }
        
        isBattle = !isBattle;
        _animator.SetBool("isBattle", isBattle);
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

                if (Managers.Game.TargetingSystem.IsCurrentTargetExist() &&
                    Vector3.Distance(Managers.Game.TargetingSystem.Target.transform.position, transform.position) < _attackState.GetAtkRange())
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
        if (currentState == newState) return;
        
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

    private void ChangeToMoveState()
    {
        ChangeState(PlayerState.Move);
    }
}
