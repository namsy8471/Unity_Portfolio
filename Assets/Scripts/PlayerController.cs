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

    private enum BattleMode
    {
        Idle,
        Battle
    }

    private IdleState idleState;
    private MoveState moveState;
    private AttackState attackState;
    private GetDamageState getDamageState;
    private TargetingSystem targetingSystem;
    
    private bool isBattle;

    private Animator animator;

    
    private SoundManager soundManager;
    
    [SerializeField] private PlayerState currentState;
    private BattleMode battleMode;
    
    // Start is called before the first frame update
    private void Start()
    {
        isBattle = false;
        
        idleState = GetComponent<IdleState>();
        moveState = GetComponent<MoveState>();
        attackState = GetComponent<AttackState>();
        getDamageState = GetComponent<GetDamageState>();
        targetingSystem = GetComponent<TargetingSystem>();
        
        animator = GetComponentInChildren<Animator>();
        
        soundManager = FindObjectOfType(typeof(SoundManager)).GetComponent<SoundManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        Debug.Log("플레이어 FSM = " + currentState);
        
        switch (battleMode)
        {
            case BattleMode.Battle:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    ChangeState(BattleMode.Idle);
                    break;
                }
                
                break;
            
            case BattleMode.Idle:
                if (Input.GetKeyDown(KeyCode.Space) || targetingSystem.IsCurrentTargetExist())
                {
                    ChangeState(BattleMode.Battle);
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

                if (targetingSystem.IsCurrentTargetExist() &&
                    Vector3.Distance(targetingSystem.GetCurrentTargetPos(), transform.position) < attackState.GetAtkRange())
                {
                    ChangeState(PlayerState.Attack);
                    break;
                }
                
                idleState.UpdateState();

                break;
            
            case PlayerState.Attack:

                if (!targetingSystem.IsCurrentTargetExist())
                {
                    ChangeState(PlayerState.Idle);    
                }
                
                attackState.UpdateState();

                break;
            
            case PlayerState.GetDamage:
                
                getDamageState.UpdateState();
                
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

                if (moveState.GetIsMoveDone())
                {
                    ChangeState(PlayerState.Idle);
                    break;
                }

                if (targetingSystem.IsCurrentTargetExist() &&
                    Vector3.Distance(targetingSystem.GetCurrentTargetPos(), transform.position) < attackState.GetAtkRange())
                {
                    ChangeState(PlayerState.Attack);
                    break;
                }
                
                moveState.UpdateState();

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
                idleState.EndState();
                break;
            
            case PlayerState.Move:
                moveState.EndState();
                break;
            
            case PlayerState.Attack:
                attackState.EndState();
                break;
            
            case PlayerState.GetDamage:
                getDamageState.EndState();
                break;
            
            default:
                break;
        }
        
        currentState = newState;

        switch (currentState)
        {
            case PlayerState.Idle:
                idleState.StartState();
                break;
            
            case PlayerState.Move:
                moveState.StartState();
                break;
            
            case PlayerState.Attack:
                attackState.StartState();
                break;
            
            case PlayerState.GetDamage:
                getDamageState.StartState();
                break;
            
            default:
                break;
        }
    }

    private void ChangeState(BattleMode mode)
    {
        switch (battleMode)
        {
            case BattleMode.Idle:
                break;
            case BattleMode.Battle:
                break;
            default:
                break;
        }

        battleMode = mode;
        
        switch (battleMode)
        {
            case BattleMode.Idle:
                isBattle = false;
                animator.SetBool("isBattle", isBattle);
                break;
            case BattleMode.Battle:
                isBattle = true;
                animator.SetBool("isBattle", isBattle);
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
    
    // 사운드 재생
    private void PlaySound(string keyWord)
    {
        soundManager.SendMessage("PlaySound", keyWord, SendMessageOptions.DontRequireReceiver);
    }
}
