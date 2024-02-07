using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR;

public class EnemyAttackState : MonoBehaviour, IStateBase
{
    enum AttackType
    {
        Atk1,
        Atk2,
        Atk3,
        Atk4,
        AtkFinish
    }

    private AttackType attackType;

    private Animator animator;
    private GameObject player;
    
    private float attackRange;
    [SerializeField] private int maxAttackCount;
    private float attackDownGauge;
    private int attackCount;
    private float attackTimer;
    private float attackDelay;
    private float lastAttackDelay;

    private bool isAtkFinished;
    
    void Start()
    {
        attackRange = 2;
        maxAttackCount = 3;
        attackDownGauge = 100 / (float) maxAttackCount + 1;
        
        attackCount = 0;
        attackTimer = 0;
        
        attackDelay = 0.8f;
        lastAttackDelay = 3.0f;

        isAtkFinished = false;

        
        animator = GetComponentInChildren<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Init()
    {
        throw new NotImplementedException();
    }

    public void StartState()
    {
        // Debug.Log("Enemy Attack State Start!");
        isAtkFinished = false;
        ChangeState(AttackType.Atk1);
    }

    public void UpdateState()
    {
        // Debug.Log("Enemy Attack State Update!");

        switch (attackType)
        {
            case AttackType.Atk1:
                
                if (attackCount < maxAttackCount && attackTimer >= attackDelay)
                {
                    ChangeState(AttackType.Atk2);
                    break;
                }
                
                else if (attackCount == maxAttackCount && attackTimer >= lastAttackDelay)
                {
                    ChangeState(AttackType.AtkFinish);
                    break;
                }
                
                attackTimer += Time.deltaTime;
                break;
            case AttackType.Atk2:
                
                if (attackCount < maxAttackCount && attackTimer >= attackDelay)
                {
                    ChangeState(AttackType.Atk3);
                    break;
                }
                
                else if (attackCount == maxAttackCount && attackTimer >= lastAttackDelay)
                {
                    ChangeState(AttackType.AtkFinish);
                    break;
                }
                attackTimer += Time.deltaTime;
                break;
            case AttackType.Atk3:
                
                if (attackCount < maxAttackCount && attackTimer >= attackDelay)
                {
                    ChangeState(AttackType.Atk4);
                    break;
                }
                
                else if (attackCount == maxAttackCount && attackTimer >= lastAttackDelay)
                {
                    ChangeState(AttackType.AtkFinish);
                    break;
                }
                attackTimer += Time.deltaTime;
                
                break;
            case AttackType.Atk4:
                
                if (attackCount == maxAttackCount && attackTimer >= lastAttackDelay)
                {
                    ChangeState(AttackType.AtkFinish);
                    break;
                }
                
                attackTimer += Time.deltaTime;
                break;
            
            case AttackType.AtkFinish:

                
                if (attackTimer >= lastAttackDelay)
                {
                    isAtkFinished = true;
                }
                
                attackTimer += Time.deltaTime;
                
                break;

            default:
                break;
        }
    }

    public void EndState()
    {
        // Debug.Log("Enemy Attack State End!");
    }

    private void Attack()
    {
        player.SendMessage("GetDamage", attackDownGauge, SendMessageOptions.DontRequireReceiver);
    }

    private void AttackFinish()
    {
        ChangeState(AttackType.AtkFinish);
    }

    public bool GetAtkDone()
    {
        return isAtkFinished;
    }
    
    private void ChangeState(AttackType type)
    {
        switch (attackType)
        {
            case AttackType.Atk1:
                break;
            case AttackType.Atk2:
                break;
            case AttackType.Atk3:
                break;
            case AttackType.Atk4:
                break;
            case AttackType.AtkFinish:
                break;
            default:
                break;
        }

        attackType = type;
        attackTimer = 0;

        switch (attackType)
        {
            case AttackType.Atk1:
                attackCount = 1;
                animator.SetTrigger("Attack1");
                Attack();
                break;
            
            case AttackType.Atk2:
                attackCount = 2;
                animator.SetTrigger("Attack2");
                Attack();

                break;
            
            case AttackType.Atk3:
                attackCount = 3;
                animator.SetTrigger("Attack3");
                Attack();

                break;
            
            case AttackType.Atk4:
                attackCount = 4;
                animator.SetTrigger("Attack5");
                Attack();

                break;
            
            case AttackType.AtkFinish:
                attackCount = 0;
                animator.SetTrigger("Buff");
                
                break;
            default:
                break;
        }
    }
    
    public float GetAttackRange()
    {
        return attackRange;
    }
}
