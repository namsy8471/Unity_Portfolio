using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyMoveState : IStateBase
{
    enum WalkType
    {
        Walk,
        Run
    }

    enum MovingState
    {
        Patrol,
        Player
    }

    enum PatrolState
    {
        SetRandPos,
        Move,
        Finish
    }
    
    private WalkType _walkType;
    private MovingState _moveState;
    private PatrolState _patrolState;
    
    private GameObject _player;
    private Animator _animator;
    private EnemyController _controller;
    
    private Transform _tr;
    private Rigidbody _rb;
    
    // 속도 관련 변수들
    private float _walkSpeedOffset = 100f;
    private float _runSpeedOffset = 500f;

    private float _speed = 100f;

    
    // 주변 배회 관련 변수들
    private float _patrolTimer;
    private float _randomPatrolTime;
    
    private bool _patrolDone;
    public bool IsPlayerFound { get; set;}

    private Vector3 _randDirection;

    public EnemyMoveState(GameObject go) => _controller = go.GetComponent<EnemyController>();
    
    public void Init()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _animator = _controller.GetComponentInChildren<Animator>();
        _tr = _controller.GetComponent<Transform>();
        _rb = _controller.GetComponent<Rigidbody>();

        _patrolDone = false;
        IsPlayerFound = false;
    }

    public void StartState()
    {
        // Debug.Log("Enemy Move State Update");

        ChangeState(IsPlayerFound ? MovingState.Player : MovingState.Patrol);
        ChangeState(WalkType.Walk);
        
        _patrolTimer = 0;
        _patrolDone = false;
        _randomPatrolTime = Random.Range(1, 4);
        
        if (_controller.Status.Hp <= 0)
        {
            _controller.ChangeState(EnemyController.EnemyState.Dead);
            return;
        }
    }

    public void UpdateState()
    {
        // Debug.Log("Enemy Move State Update");

        if (_controller.DownGauge > 0)
            _controller.DownGauge -= Time.deltaTime;
        else _controller.DownGauge = 0;
        
        switch (_moveState)
        {
            case MovingState.Patrol:
                switch (_patrolState)
                {
                    case PatrolState.SetRandPos:
                        _randDirection = new Vector3(Random.value * 2 - 1, 0 , Random.value * 2 - 1).normalized;
                        
                        _patrolState = PatrolState.Move;
                        break;
                    
                    case PatrolState.Move:
                        if (_patrolTimer > _randomPatrolTime)
                        {
                            _patrolTimer = 0;
                            _patrolState = PatrolState.Finish;
                            break;
                        }

                        _patrolTimer += Time.deltaTime;
                        Patrol();
                        break;
                    
                    case PatrolState.Finish:
                        PatrolDone();
                        break;
                    
                    default:
                        break;
                }
                break;
            case MovingState.Player:
                Chase();
                break;
            default:
                break;
        }
        switch (_walkType)
        {
            case WalkType.Walk:
            {
                float percent = Random.Range(0, 301) / 300f;
                if (_moveState == MovingState.Player && percent < 0.002f)
                {
                    ChangeState(WalkType.Run);
                    break;
                }

                break;
            }
            case WalkType.Run:
                break;
            default:
                break;
        }
    }
    
    public void EndState()
    {
        // Debug.Log("Enemy Move State End!");

        _rb.velocity = Vector3.zero;
        _animator.SetBool("WalkForward", false);
        _animator.SetBool("Run Forward", false);
    }

    void Patrol()
    {
        var lookDir = _randDirection + _tr.position;
        _tr.LookAt(lookDir);
        _rb.velocity = _randDirection * (_speed * Time.deltaTime);
    }
    
    void Chase()
    {
        var playerPos = _player.transform.position;
        var pos = _tr.position;
        playerPos.y = pos.y;
        
        var dir = playerPos - pos;
        
        _tr.LookAt(playerPos);
        _rb.velocity = dir.normalized * (_speed * Time.deltaTime);
    }
    
    void ChangeState(WalkType state)
    {
        switch (_walkType)
        {
            case WalkType.Walk:
                _animator.SetBool("WalkForward", false);
                break;
            case WalkType.Run:
                _animator.SetBool("Run Forward", false);

                break;
            default:
                break;
        }

        _walkType = state;

        switch (_walkType)
        {
            case WalkType.Walk:
                _animator.SetBool("WalkForward", true);
                _speed = _walkSpeedOffset;
                break;
            case WalkType.Run:
                _animator.SetBool("Run Forward", true);
                _speed = _runSpeedOffset;
                break;
            default:
                break;
        }
    }

    void ChangeState(MovingState state)
    {
        switch (_moveState)
        {
            case MovingState.Patrol:
                break;
            case MovingState.Player:
                break;
            default:
                break;
        }

        _moveState = state;

        switch (_moveState)
        {
            case MovingState.Patrol:
                break;
            case MovingState.Player:
                break;
            default:
                break;
        }
    }

    void PatrolDone()
    {
        _patrolState = PatrolState.SetRandPos;
        _patrolDone = true;
    }

    public bool GetPatrolDone()
    {
        return _patrolDone;
    }
}
