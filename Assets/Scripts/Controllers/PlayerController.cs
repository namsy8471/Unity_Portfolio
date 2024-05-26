using System;
using System.Collections;
using System.Collections.Generic;
using Contents.Status;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : Controller
{
    public enum PlayerState
    {
        Idle,
        Attack,
        GetDamage,
        Down,
        Dead
    }
    
    private bool _isBattle;
    
    [SerializeField] private float _healRegenTimer;
    [SerializeField] private float _healRegenTimeLimit;
    [SerializeField] private PlayerState currentState;
    
    private List<Action> _playerMouseDownActions = new List<Action>();
    private List<Action> _playerMousePressedActions = new List<Action>();
    
    public ActiveSkill CurrentSkill { get; set; }
    public IdleState IdleState { get; } = new IdleState();
    public AttackState AttackState { get; } = new AttackState();
    public GetDamageState GetDamageState { get; } = new GetDamageState();
    public DownState DownState { get; } = new DownState();
    public DeadState DeadState { get; } = new DeadState();
    public PlayerStatus Status { get; set; }

    public List<Action> PlayerMouseDownActions => _playerMouseDownActions;
    public List<Action> PlayerMousePressedActions => _playerMousePressedActions;

    public bool IsAttackReserved { get; set;}

    public float DownGauge { get; set; } = 0;

    public EnemyController CurrentEnemy { get; set; }

    private void Start()
    {
        _isBattle = false;
        Animator = GetComponent<Animator>();
        
        _healRegenTimeLimit = 10;
        _healRegenTimer = _healRegenTimeLimit;
        
        gameObject.GetOrAddComponent<AudioSource>();

        CurrentSkill = new ActiveSkill();
        
        #region KeyBinding in InputManager
        
        Managers.Input.AddAction(Managers.Input.KeyButtonDown, Managers.Input.PostureChangeKey, PostureChangeFunction);
        
        Managers.Input.LMBDown += ReserveAttack;
        PlayerMouseDownActions.Add(ReserveAttack);
        
        #endregion
        
        HpBarInit();
        
        Status = new PlayerStatus(gameObject);
        
        IdleState.Init();
        AttackState.Init();
        GetDamageState.Init();
        DownState.Init();
        DeadState.Init();

        Managers.Graphics.UI.BindingSliderWithPlayerStatus();
        
        Managers.Game.SkillSystem.AddSkill(new Smash());
        Managers.Game.SkillSystem.AddSkill(new Defence());
        Managers.Game.SkillSystem.AddSkill(new CounterAttack());

        
        CurrentSkill = null;
    }

    protected override void Update()
    {
        base.Update();
        
        switch (currentState)
        {
            case PlayerState.Idle:
                if (_healRegenTimer <= 0)
                {
                    Status.Hp += Time.deltaTime;
                    Status.Mp += Time.deltaTime;
                    Status.Stamina += Time.deltaTime;
                    DownGauge -= Time.deltaTime;
                }
                else _healRegenTimer -= Time.deltaTime;
                break;
            
            case PlayerState.Attack:
            case PlayerState.GetDamage:
            case PlayerState.Down:
            case PlayerState.Dead:
                _healRegenTimer = _healRegenTimeLimit;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        switch (currentState)
        {
            case PlayerState.Attack:

                if (AttackState.AttackTimer <= 0)
                {
                    ChangeState(PlayerState.Idle);
                    break;
                }
                
                AttackState.UpdateState();

                break;
            
            case PlayerState.GetDamage:

                if (GetDamageState.Timer <= 0)
                {
                    ChangeState(PlayerState.Idle);
                    break;
                }
                
                GetDamageState.UpdateState();
                
                break;
            
            case PlayerState.Down:

                if (DownState.Timer <= 0)
                {
                    ChangeState(PlayerState.Idle);
                    break;
                }
                
                DownState.UpdateState();

                break;
            case PlayerState.Dead:
                
                DeadState.UpdateState();
                break;
            
            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case PlayerState.Idle:
                
                IdleState.UpdateState();
                
                if (CurrentSkill is not DefendSkill && IsAttackReserved)
                {
                    Attack();
                }
                
                break;
            
            default:
                break;
        }
    }
    
    private void PostureChangeFunction()
    {
        if (Managers.Game.TargetingSystem.IsCurrentTargetExist())
        {
            _isBattle = true;
            Animator.SetBool("isBattle", _isBattle);
            return;
        }
        
        _isBattle = !_isBattle;
        Animator.SetBool("isBattle", _isBattle);
    }
    
    public void ChangeState(PlayerState newState)
    {
        if (currentState == newState) return;
        
        switch (currentState)
        {
            case PlayerState.Idle:
                IdleState.EndState();
                break;
            
            case PlayerState.Attack:
                AttackState.EndState();
                break;
            
            case PlayerState.GetDamage:
                GetDamageState.EndState();
                break;
            
            case PlayerState.Down:
                DownState.EndState();
                break;
            
            case PlayerState.Dead:
                DeadState.EndState();
                break;
            default:
                break;
        }
        
        currentState = newState;

        switch (currentState)
        {
            case PlayerState.Idle:
                IdleState.StartState();
                break;
            
            case PlayerState.Attack:
                AttackState.StartState();
                break;
            
            case PlayerState.GetDamage:
                GetDamageState.StartState();
                break;
            
            case PlayerState.Down:
                DownState.StartState();
                break;
            case PlayerState.Dead:
                DeadState.StartState();
                break;
            default:
                break;
        }
    }

    public void ChangeState(PlayerState newState, EnemyController enemyController)
    {
        switch (currentState)
        {
            case PlayerState.Idle:
                IdleState.EndState();
                break;
            
            case PlayerState.Attack:
                AttackState.EndState();
                break;
            
            case PlayerState.GetDamage:
                GetDamageState.EndState();
                break;
            
            case PlayerState.Down:
                DownState.EndState();
                break;
            
            default:
                break;
        }
        
        currentState = newState;

        switch (currentState)
        {
            case PlayerState.Idle:
                IdleState.StartState();
                break;
            
            case PlayerState.Attack:
                AttackState.StartState();
                break;
            
            case PlayerState.GetDamage:
                GetDamageState.StartState(enemyController);
                break;
            
            case PlayerState.Down:
                DownState.StartState();
                break;
            
            default:
                break;
        }
    }
    
    private void ReserveAttack()
    {
        if (Managers.Game.TargetingSystem.IsCurrentTargetExist())
            IsAttackReserved = true;
        else
            IsAttackReserved = Managers.Ray.RayHitCollider.gameObject.layer == LayerMask.NameToLayer("Enemy");

        _isBattle = IsAttackReserved || _isBattle;
        Animator.SetBool("isBattle", _isBattle);
    }
    
    private void Attack()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            var targetingSystem = Managers.Game.TargetingSystem;
            
            if (targetingSystem.IsCurrentTargetExist()
                && targetingSystem.Target.layer == LayerMask.NameToLayer("Enemy")
                && Vector3.Distance(targetingSystem.Target.transform.position, transform.position)
                <= ((CurrentSkill as AttackSkill)?.SkillRange ?? Status.AtkRange)
                && targetingSystem.Target.GetComponent<EnemyController>().InvincibleTimer <= 0)
            {
                ChangeState(PlayerState.Attack);
            }
        }
        
        else
        {
            var ray = Managers.Ray;
            
            if (Vector3.Distance(ray.RayHitPointByMouseClicked, transform.position)
                <= ((CurrentSkill as AttackSkill)?.SkillRange ?? Status.AtkRange) 
                && ray.RayHitColliderByMouseClicked.gameObject.layer == LayerMask.NameToLayer("Enemy")
                && ray.RayHitColliderByMouseClicked.gameObject.GetComponent<EnemyController>().InvincibleTimer <= 0)
            {
                if (ray.RayHitColliderByMouseClicked.gameObject.GetComponent<EnemyController>().State
                    == EnemyController.EnemyState.Dead)
                    return;
                
                ChangeState(PlayerState.Attack);
            }
        }
    }
    
    public void GetDamage(EnemyController enemyController)
    {
        CurrentEnemy = enemyController;
        ChangeState(PlayerState.GetDamage, enemyController);
    }

    #region Animation Event
    private void AttackToEnemy() => AttackState.Attack(CurrentEnemy);

    private void GetInvincibleTime()
    {
        InvincibleTimer = Animator.GetCurrentAnimatorStateInfo(0).length;
        if(CurrentEnemy == null)
            CurrentEnemy = Managers.Game.TargetingSystem.Target.GetComponent<EnemyController>();
    }
    //private void PlaySound(string keyWord) => Managers.Sound.PlaySound(gameObject, keyWord);
    
    #endregion
}
