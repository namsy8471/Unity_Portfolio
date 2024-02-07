using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IStateBase
{
    private enum AtkState
    {
        Idle,
        Atk1,
        Atk2,
        Atk3,
        AtkFinish
    }

    private AtkState _atkState;
    
    private float attackRange;
    private float attackCooldown;
    private float lastAttackCooldown;
    private float attackTimer;

    private float downGauge;
    
    private Animator animator;

    public void Init()
    {
        _atkState = AtkState.Idle;
        animator = Managers.Game.Player.GetComponentInChildren<Animator>();
        
        downGauge = 34;
        
        attackRange = 3.0f;
        attackCooldown = 0.3f;
        lastAttackCooldown = 2.0f;
        attackTimer = 0;
    }

    public void StartState()
    {
        // Debug.Log("Attack State Start!");

        attackTimer = 0;

    }

    public void UpdateState()
    {
        // Debug.Log("Attack State Update!");
        // Debug.Log("Attack State : " + atkState);
        attackTimer += Time.deltaTime;
        
        if(attackTimer >= 2)
            ChangeState(AtkState.AtkFinish);
        
        switch (_atkState)
        {
            case AtkState.Idle:
                
                if (Input.GetMouseButtonDown(0) && !Managers.Game.TargetingSystem.GetCurrentTarget().GetComponent<EnemyGetDamageState>().IsInvincible())
                {
                    ChangeState(AtkState.Atk1);
                    break;
                }
                
                break;
            
            case AtkState.Atk1:
                if (Input.GetMouseButtonDown(0) && attackTimer >= attackCooldown)
                {
                    ChangeState(AtkState.Atk2);
                    break;
                }
                
                break;
            
            case AtkState.Atk2:
                if (Input.GetMouseButtonDown(0) && attackTimer >= attackCooldown)
                {
                    ChangeState(AtkState.Atk3);
                    break;
                }
                break;
            
            case AtkState.Atk3:
                if (attackTimer >= lastAttackCooldown)
                {
                    ChangeState(AtkState.AtkFinish);
                    break;
                }
                break;
            case AtkState.AtkFinish:
                
                break;
        }
    }

    public void EndState()
    {
        // Debug.Log("Attack State End!");
        _atkState = AtkState.Idle;
    }
    
    private void Attack()
    {
        Managers.Game.TargetingSystem.GetCurrentTarget().SendMessage("GetDamage", downGauge, SendMessageOptions.DontRequireReceiver);
    }

    private void ChangeState(AtkState state)
    {
        switch (_atkState)
        {
            case AtkState.Idle:
                break;
            case AtkState.Atk1:
                break;
            case AtkState.Atk2:
                break;
            case AtkState.Atk3:
                break;
            default:
                break;
        }

        _atkState = state;
        attackTimer = 0;
        
        switch (_atkState)
        {
            case AtkState.Idle:
                break;
            case AtkState.Atk1:
                animator.SetTrigger("Attack");
                Attack();
                break;
            case AtkState.Atk2:
                animator.SetTrigger("NextAttack");
                Attack();
                break;
            case AtkState.Atk3:
                animator.SetTrigger("LastAttack");
                Attack();
                break;
            case AtkState.AtkFinish:
                Managers.Game.Player.gameObject.SendMessage("BackToIdle", SendMessageOptions.DontRequireReceiver);
                break;
            default:
                break;
        }
        
    }

    public float GetAtkRange()
    {
        return attackRange;
    }
    
}
