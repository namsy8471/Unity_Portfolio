using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : MonoBehaviour, IStateBase
{
    private enum AtkState
    {
        Idle,
        Atk1,
        Atk2,
        Atk3,
        AtkFinish
    }

    private AtkState atkState;
    
    [SerializeField]private float attackRange;
    [SerializeField]private float attackCooldown;
    [SerializeField]private float lastAttackCooldown;
    private float attackTimer;

    private float downGauge;
    
    private TargetingSystem targetingSystem;
    private Animator animator;
    
    private void Start()
    {
        atkState = AtkState.Idle;
        animator = GetComponentInChildren<Animator>();
        targetingSystem = GetComponent<TargetingSystem>();

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
        
        switch (atkState)
        {
            case AtkState.Idle:
                
                if (Input.GetMouseButtonDown(0) && !targetingSystem.GetCurrentTarget().GetComponent<EnemyGetDamageState>().IsInvincible())
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
        atkState = AtkState.Idle;
    }
    
    private void Attack()
    {
        targetingSystem.GetCurrentTarget().SendMessage("GetDamage", downGauge, SendMessageOptions.DontRequireReceiver);
    }

    private void ChangeState(AtkState state)
    {
        switch (atkState)
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

        atkState = state;
        attackTimer = 0;
        
        switch (atkState)
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
                gameObject.SendMessage("BackToIdle", SendMessageOptions.DontRequireReceiver);
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
