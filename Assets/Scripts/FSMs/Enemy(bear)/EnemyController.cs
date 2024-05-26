using System;
using System.Collections;
using Contents.Status;
using Scenes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemyController : Controller
{
    public enum EnemyState
    {
        Idle,
        Move,
        Attack,
        GetDamage,
        Down,
        Dead
    }

    enum BattleState
    {
        Idle,
        Battle
    }

    [SerializeField] public EnemyState State;
    [SerializeField] private BattleState _battleState;
    
    private EnemyIdleState _idleState;
    private EnemyMoveState _moveState;
    private EnemyAttackState _attackState;
    private EnemyGetDamageState _getDamageState;
    private EnemyDownState _downState;
    private EnemyDeadState _deadState;
    
    private SphereCollider _detectCol;
    
    private PlayerController _player;
    private float _petrolTimer;
    
    public float DownGauge { get; set; }
    public Status Status { get; set; }
    
    public Action DieAction;
    void Start()
    {
        Animator = GetComponentInChildren<Animator>();

        _detectCol = GetComponentInChildren<SphereCollider>();
        GetComponentInChildren<EnemyDetectingBoundary>().TriggerEnter = OnTriggerEnterInDetectingCollider;
        GetComponentInChildren<EnemyDetectingBoundary>().TriggerExit = OnTriggerExitFromDetectingCollider;
        
        _player = Managers.Game.Player.GetComponent<PlayerController>();
        
        State = EnemyState.Idle;
        _battleState = BattleState.Idle;
        
        Animator.SetBool("Idle",true);

        _petrolTimer = 0;
        DownGauge = 0;
        
        var go = gameObject;

        HpBarInit();
        
        Status = new Status(go);
        
        _idleState = new EnemyIdleState(go);
        _moveState = new EnemyMoveState(go);
        _attackState = new EnemyAttackState(go);
        _getDamageState = new EnemyGetDamageState(go);
        _downState = new EnemyDownState(go);
        _deadState = new EnemyDeadState(go);
        
        _idleState.Init();
        _moveState.Init();
        _attackState.Init();
        _getDamageState.Init();
        _downState.Init();
        _deadState.Init();

        DieAction = GameObject.Find("@Scene").GetComponent<GameScene>().EnemyKilled;
    }

    protected override void Update()
    {
        base.Update();
        
        switch (State)
        {
            case EnemyState.Idle:
                if (_petrolTimer > 3)
                {
                    _petrolTimer = 0;
                    ChangeState(EnemyState.Move);
                    break;
                }
                
                if (Vector3.Distance(_player.transform.position, transform.position) <= Status.AtkRange
                    && _player.InvincibleTimer <= 0)
                {
                    ChangeState(EnemyState.Attack);
                    break;
                }

                _petrolTimer += Time.deltaTime;
                _idleState.UpdateState();
                break;
            
            case EnemyState.Attack:

                if (_attackState.Timer <= 0)
                {
                    ChangeState(EnemyState.Move);
                    break;
                }
                
                _attackState.UpdateState();
                break;
            
            case EnemyState.GetDamage:
                
                if (_getDamageState.Timer <= 0)
                {
                    ChangeState(EnemyState.Idle);
                    break;
                }
                
                _getDamageState.UpdateState();
                
                break;
            case EnemyState.Down:
                
                if (_downState.Timer <= 0)
                {
                    ChangeState(EnemyState.Idle);
                }
                
                _downState.UpdateState();
                break;
            case EnemyState.Dead:

                if (_deadState.Timer <= 0)
                {
                    ChangeState(EnemyState.Idle);
                }
                
                _deadState.UpdateState();
                
                break;
            default:
                break;
        }
        
    }

    private void FixedUpdate()
    {
        switch (State)
        {
            case EnemyState.Move:

                var isPlayerInAtkRange =
                    Vector3.Distance(_player.transform.position, transform.position) <= Status.AtkRange;
                
                if (isPlayerInAtkRange && _player.InvincibleTimer <= 0)
                {
                    ChangeState(EnemyState.Attack);
                    break;
                } 
                
                if (isPlayerInAtkRange || _moveState.GetPatrolDone())
                {
                    ChangeState(EnemyState.Idle);
                    break;
                }
                
                _moveState.UpdateState();
                break;
        }
    }

    private void OnDisable()
    {
        if (Managers.Game.TargetingSystem.Target == gameObject)
        {
            Managers.Game.TargetingSystem.Target = null;
        }

        if (Managers.Ray.RayHitCollider == gameObject.GetComponent<Collider>())
        {
            Managers.Ray.RayHitCollider = null;
        }

        if (Managers.Ray.RayHitColliderByMouseClicked == gameObject.GetComponent<Collider>())
        {
            Managers.Ray.RayHitColliderByMouseClicked = null;
        }
        
        Destroy(gameObject, 1.0f);
    }

    public void MakeItem()
    {
        // 아이템 생성
        string path = "";
        
        int rand = Random.Range(0, 3);
        switch (rand)
        {
            case 0:
                path = "Prefabs/Equipment/Weapon/LongSword";
                break;
            case 1:
                path = "Prefabs/Equipment/Shield/Shield1/HighLevelShield";
                break;
            case 2:
                path = "Prefabs/Equipment/Shield/Shield3/LowLevelShield";
                break;
        }

        GameObject obj = Instantiate(Resources.Load<GameObject>(path), transform.position + Vector3.up * 0.2f, transform.rotation);
        int startIndex = path.LastIndexOf('/') + 1;
        string result = path.Substring(startIndex);

        obj.name = result;
    }

    public void ChangeState(EnemyState state)
    {
        switch (State)
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
            case EnemyState.Down:
                _downState.EndState();
                break;
            case EnemyState.Dead:
                _deadState.EndState();
                break;
            
            default:
                break;
        }

        State = state;

        switch (State)
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
            case EnemyState.Down:
                _downState.StartState();
                break;
            case EnemyState.Dead:
                _deadState.StartState();
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
                Animator.SetBool("Idle", false);
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
                Animator.SetBool("Idle", true);
                break;
            case BattleState.Battle:
                Animator.SetTrigger("Buff");
                break;
            default:
                break;
        }
    }

    private void ChangeStateToMove()
    {
        Animator.SetTrigger("Buff End");
        ChangeState(EnemyState.Move);
    }
    
    public void GetDamage()
    {
        ChangeState(EnemyState.GetDamage);
    }
    
    private void OnTriggerEnterInDetectingCollider(Collider other)
    {
        if (other.CompareTag("Player") && _battleState == BattleState.Idle)
        {
            var playerPos = _player.transform.position;
            var position = transform.position;
            playerPos.y = position.y;
            
            transform.LookAt(playerPos);
            
            _moveState.IsPlayerFound = true;
            
            ChangeState(EnemyState.Idle);
            ChangeState(BattleState.Battle);
            Invoke(nameof(ChangeStateToMove), Animator.GetCurrentAnimatorStateInfo(0).length);

            _detectCol.radius = 15f;
        }
    }

    private void OnTriggerExitFromDetectingCollider(Collider other)
    {
        if (other.CompareTag("Player") && _battleState == BattleState.Battle)
        {
            _moveState.IsPlayerFound = false;
            ChangeState(BattleState.Idle);
            ChangeState(EnemyState.Idle);

            _detectCol.radius = 7f;
        }
    }

    #region Animation Event

    public void Attack() => _attackState.Attack();
    //private void PlaySound(string keyWord) => Managers.Sound.PlaySound(gameObject, keyWord);

    #endregion
}
