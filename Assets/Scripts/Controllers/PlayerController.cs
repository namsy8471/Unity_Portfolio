using System;
using System.Collections;
using System.Collections.Generic;
using Contents.Status;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    private enum PlayerState
    {
        Idle,
        Attack,
        GetDamage,
        Down
    }
    
    private bool _isBattle;
    private Animator _animator;
    
    [SerializeField] private float _healRegenTimer;
    [SerializeField] private float _healRegenTimeLimit;
    [SerializeField] private PlayerState currentState;

    public ActiveSkill CurrentSkill { get; set; }
    
    public IdleState IdleState { get; } = new IdleState();
    public AttackState AttackState { get; } = new AttackState();
    public GetDamageState GetDamageState { get; } = new GetDamageState();
    public PlayerStatus Status { get; private set; }

    private List<Action> _playerMouseDownActions = new List<Action>();
    private List<Action> _playerMousePressedActions = new List<Action>();
    public List<Action> PlayerMouseDownActions => _playerMouseDownActions;
    public List<Action> PlayerMousePressedActions => _playerMousePressedActions;

    public bool IsAttackReserved { get; set;}

    private void Start()
    {
        _isBattle = false;
        _animator = GetComponentInChildren<Animator>();
        
        _healRegenTimeLimit = 5;
        _healRegenTimer = _healRegenTimeLimit;

        CurrentSkill = new ActiveSkill();
        
        #region KeyBinding in InputManager
        
        Managers.Input.AddAction(Managers.Input.KeyButtonDown, Managers.Input.PostureChangeKey, PostureChangeFunction);
        Managers.Input.AddAction(Managers.Input.KeyButtonDown, Managers.Input.SkillQuitKey, CurrentSkill.StopSkill);
        
        Managers.Input.LMBDown += ReserveAttack;
        PlayerMouseDownActions.Add(ReserveAttack);
        
        #endregion
        
        Status = new PlayerStatus();
        
        IdleState.Init();
        AttackState.Init();
        GetDamageState.Init();

        Managers.Graphics.UI.BindingSliderWithPlayerStatus();
        
        Managers.Game.SkillSystem.AddSkill(new Smash());
        Managers.Game.SkillSystem.AddSkill(new Defence());

        CurrentSkill = null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            //Managers.Game.Player.GetComponentInChildren<Animator>().Play("Smash");
            Managers.Game.SkillSystem.AddSkill(Managers.Game.SkillSystem.SkillSmash);
            // smash.UseSkill();
        }
        
        switch (currentState)
        {
            case PlayerState.Idle:
                if (_healRegenTimer <= 0)
                {
                    Status.Hp += 0.01f;
                    Status.Mp += 0.01f;
                    Status.Stamina += 0.01f;
                }
                else _healRegenTimer -= Time.deltaTime;
                break;
            
            case PlayerState.Attack:
            case PlayerState.GetDamage:
            case PlayerState.Down:
                _healRegenTimer = _healRegenTimeLimit;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        switch (currentState)
        {
            case PlayerState.Attack:

                if (AttackState.AttackTimer >= Status.AtkSpeed && !_animator.applyRootMotion)
                {
                    ChangeState(PlayerState.Idle);
                    break;
                }
                
                AttackState.UpdateState();

                break;
            
            case PlayerState.GetDamage:
                
                GetDamageState.UpdateState();
                
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
                
                if (IsAttackReserved)
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
            _animator.SetBool("isBattle", _isBattle);
            return;
        }
        
        _isBattle = !_isBattle;
        _animator.SetBool("isBattle", _isBattle);
    }
    
    private void ChangeState(PlayerState newState)
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
            
            default:
                break;
        }
    }
    
    private void GetDamage(float value)
    {
        ChangeState(PlayerState.GetDamage);
        gameObject.SendMessage("AddDownGauge", value, SendMessageOptions.DontRequireReceiver);
    }

    private void BackToIdle()
    {
        ChangeState(PlayerState.Idle);
    }

    private void SetRootAnimationFalse()
    {
        _animator.applyRootMotion = false;
    }
    
    public void SetRootAnimFalseInvoke()
    {
        Invoke(nameof(SetRootAnimationFalse), CurrentSkill.SkillUseAnimClip.length + 0.1f);
    }

    private void ReserveAttack()
    {
        if (Managers.Game.TargetingSystem.IsCurrentTargetExist())
            IsAttackReserved = true;
        else
            IsAttackReserved = Managers.Ray.RayHitCollider.gameObject.layer == LayerMask.NameToLayer("Enemy");
    }
    
    private void Attack()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Managers.Game.TargetingSystem.IsCurrentTargetExist()
                && Managers.Game.TargetingSystem.GetCurrentTarget().layer == LayerMask.NameToLayer("Enemy")
                && Vector3.Distance(Managers.Game.TargetingSystem.Target.transform.position, transform.position)
                <= ((CurrentSkill as AttackSkill)?.SkillRange ?? Status.AtkRange))
            {
                ChangeState(PlayerState.Attack);
            }
        }
        
        else
        {
            if (Vector3.Distance(Managers.Ray.RayHitPoint, transform.position)
                <= ((CurrentSkill as AttackSkill)?.SkillRange ?? Status.AtkRange) 
                && Managers.Ray.RayHitCollider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                ChangeState(PlayerState.Attack);
            }
        }
    }

}
