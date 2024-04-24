using System;
using System.Collections;
using Contents.Status;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        GetDamage
    }

    enum BattleState
    {
        Idle,
        Battle
    }

    [SerializeField] private EnemyState _enemyState;
    [SerializeField] private BattleState _battleState;
    
    private EnemyIdleState _idleState;
    private EnemyMoveState _moveState;
    private EnemyAttackState _attackState;
    private EnemyGetDamageState _getDamageState;
    
    private Animator _animator;
    private SphereCollider _detectCol;
    
    private GameObject _player;

    private float _petrolTimer;
    private bool _playerInRange;

    public Status Status { get; } = new Status();

    void Start()
    {
        _animator = GetComponentInChildren<Animator>();

        _detectCol = GetComponentInChildren<SphereCollider>();
        GetComponentInChildren<EnemyDetectingBoundary>().TriggerEnter = OnTriggerEnterInDetectingCollider;
        GetComponentInChildren<EnemyDetectingBoundary>().TriggerExit = OnTriggerExitFromDetectingCollider;
        
        _player = Managers.Game.Player;
        
        _enemyState = EnemyState.Idle;
        _battleState = BattleState.Idle;
        
        _animator.SetBool("Idle",true);

        _petrolTimer = 0;
        _playerInRange = false;
        
        _idleState = new EnemyIdleState(gameObject);
        _moveState = new EnemyMoveState(gameObject);
        _attackState = new EnemyAttackState(gameObject);
        _getDamageState = new EnemyGetDamageState(gameObject);
        
        _idleState.Init();
        _moveState.Init();
        _attackState.Init();
        _getDamageState.Init();
    }

    void Update()
    {
        switch (_enemyState)
        {
            case EnemyState.Idle:
                if (_petrolTimer > 3)
                {
                    _petrolTimer = 0;
                    ChangeState(EnemyState.Move);
                    break;
                }
                
                if (_playerInRange)
                {
                    ChangeState(EnemyState.Attack);
                    break;
                }

                _petrolTimer += Time.deltaTime;
                _idleState.UpdateState();
                break;
            
            case EnemyState.Attack:

                if (!_playerInRange && _attackState.GetAtkDone())
                {
                    ChangeState(EnemyState.Move);
                    break;
                }
                
                _attackState.UpdateState();
                break;
            
            case EnemyState.GetDamage:
                
                _getDamageState.UpdateState();
                break;
            
            default:
                break;
        }
        
    }

    private void FixedUpdate()
    {
        _playerInRange = Vector3.Distance(_player.transform.position, transform.position) <= _attackState.GetAttackRange();
        
        switch (_enemyState)
        {
            case EnemyState.Move:
                if (_moveState.GetPatrolDone())
                {
                    ChangeState(EnemyState.Idle);
                    break;
                }

                if (_playerInRange)
                {
                    ChangeState(EnemyState.Attack);
                    break;
                } 
                
                _moveState.UpdateState();
                break;
        }
    }

    void ChangeState(EnemyState state)
    {
        switch (_enemyState)
        {
            case EnemyState.Idle:
                _idleState.EndState();
                break;
            case EnemyState.Move:
                _moveState.EndState();
                break;
            case EnemyState.Attack:
                _attackState.EndState();
                break;
            case EnemyState.GetDamage:
                _getDamageState.EndState();
                break;
            default:
                break;
        }

        _enemyState = state;

        switch (_enemyState)
        {
            case EnemyState.Idle:
                _idleState.StartState();
                break;
            case EnemyState.Move:
                _moveState.StartState();
                break;
            case EnemyState.Attack:
                _attackState.StartState();
                break;
            case EnemyState.GetDamage:
                _getDamageState.StartState();
                break;
            default:
                break;
        }
    }

    void ChangeState(BattleState state)
    {
        switch (_battleState)
        {
            case BattleState.Idle:
                _animator.SetBool("Idle", false);
                break;
            case BattleState.Battle:
                break;
            default:
                break;
        }

        _battleState = state;

        switch (_battleState)
        {
            case BattleState.Idle:
                _animator.SetBool("Idle", true);
                break;
            case BattleState.Battle:
                _animator.SetTrigger("Buff");
                break;
            default:
                break;
        }
    }

    private void ChangeStateToMove()
    {
        _animator.SetTrigger("Buff End");
        ChangeState(EnemyState.Move);
    }
    
    public void GetDamage(float value)
    {
        ChangeState(EnemyState.GetDamage);
        gameObject.SendMessage("AddDownGauge", value, SendMessageOptions.DontRequireReceiver);
    }
    
    private void BackToIdle()
    {
        ChangeState(EnemyState.Idle);
    }
    
    private void OnTriggerEnterInDetectingCollider(Collider other)
    {
        if (other.CompareTag("Player") && _battleState == BattleState.Idle)
        {
            var playerPos = _player.transform.position;
            var position = transform.position;
            playerPos.y = position.y;
            
            transform.LookAt(playerPos);
            
            _moveState.SetFindPlayer(true);
            
            ChangeState(EnemyState.Idle);
            ChangeState(BattleState.Battle);
            Invoke(nameof(ChangeStateToMove), Random.Range(1, 5));

            _detectCol.radius = 15f;
        }
    }

    private void OnTriggerExitFromDetectingCollider(Collider other)
    {
        if (other.CompareTag("Player") && _battleState == BattleState.Battle)
        {
            _moveState.SetFindPlayer(false);
            ChangeState(BattleState.Idle);
            ChangeState(EnemyState.Idle);

            _detectCol.radius = 7f;
        }
    }
}
