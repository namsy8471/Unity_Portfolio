using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Random = UnityEngine.Random;

public class EnemyFSM : MonoBehaviour
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

    [SerializeField] private EnemyState enemyState;
    [SerializeField] private BattleState battleState;
    
    private EnemyIdleState enemyIdleState;
    private EnemyMoveState enemyMoveState;
    private EnemyAttackState enemyAttackState;
    private EnemyGetDamageState enemyGetDamageState;

    private Animator animator;
    private SphereCollider detectCol;
    
    private GameObject player;
    private Transform tr;

    private float petrolTimer;
    private bool playerInRange;
    void Start()
    {
        enemyIdleState = GetComponent<EnemyIdleState>();
        enemyMoveState = GetComponent<EnemyMoveState>();
        enemyAttackState = GetComponent<EnemyAttackState>();
        enemyGetDamageState = GetComponent<EnemyGetDamageState>();
        
        animator = GetComponentInChildren<Animator>();
        detectCol = GetComponent<SphereCollider>();
        
        player = GameObject.FindGameObjectWithTag("Player");
        tr = GetComponent<Transform>();
        
        enemyState = EnemyState.Idle;
        battleState = BattleState.Idle;
        animator.SetBool("Idle",true);

        petrolTimer = 0;
        playerInRange = false;
    }

    void Update()
    {
        Debug.Log("적 FSM = " + enemyState);
        
        switch (enemyState)
        {
            case EnemyState.Idle:
                if (petrolTimer > 3)
                {
                    petrolTimer = 0;
                    ChangeState(EnemyState.Move);
                    break;
                }
                
                StartCoroutine("CheckAttackRange");
                if (playerInRange)
                {
                    StopCoroutine("CheckAttackRange");
                    ChangeState(EnemyState.Attack);
                    break;
                }

                petrolTimer += Time.deltaTime;
                enemyIdleState.UpdateState();
                break;
            
            case EnemyState.Attack:

                StartCoroutine("CheckAttackRange");
                
                if (!playerInRange && enemyAttackState.GetAtkDone())
                {
                    StopCoroutine("CheckAttackRange");
                    ChangeState(EnemyState.Move);
                    break;
                }
                
                enemyAttackState.UpdateState();
                break;
            
            case EnemyState.GetDamage:
                
                enemyGetDamageState.UpdateState();
                break;
            
            default:
                break;
        }
        
    }

    private void FixedUpdate()
    {
        switch (enemyState)
        {
            case EnemyState.Move:
                if (enemyMoveState.GetPatrolDone())
                {
                    ChangeState(EnemyState.Idle);
                    break;
                }

                StartCoroutine("CheckAttackRange");
                
                if (playerInRange)
                {
                    StopCoroutine("CheckAttackRange");
                    ChangeState(EnemyState.Attack);
                    break;
                } 
                
                enemyMoveState.UpdateState();
                break;
        }
    }

    void ChangeState(EnemyState state)
    {
        switch (enemyState)
        {
            case EnemyState.Idle:
                enemyIdleState.EndState();
                break;
            case EnemyState.Move:
                enemyMoveState.EndState();
                break;
            case EnemyState.Attack:
                enemyAttackState.EndState();
                break;
            case EnemyState.GetDamage:
                enemyGetDamageState.EndState();
                break;
            default:
                break;
        }

        enemyState = state;

        switch (enemyState)
        {
            case EnemyState.Idle:
                enemyIdleState.StartState();
                break;
            case EnemyState.Move:
                enemyMoveState.StartState();
                break;
            case EnemyState.Attack:
                enemyAttackState.StartState();
                break;
            case EnemyState.GetDamage:
                enemyGetDamageState.StartState();
                break;
            default:
                break;
        }
    }

    void ChangeState(BattleState state)
    {
        switch (battleState)
        {
            case BattleState.Idle:
                animator.SetBool("Idle", false);
                break;
            case BattleState.Battle:
                break;
            default:
                break;
        }

        battleState = state;

        switch (battleState)
        {
            case BattleState.Idle:
                animator.SetBool("Idle", true);
                break;
            case BattleState.Battle:
                animator.SetTrigger("Buff");
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && battleState == BattleState.Idle)
        {
            Debug.Log("플레이어 탐지!");
            var playerPos = player.transform.position;
            var position = tr.position;
            playerPos.y = position.y;
        
            var dir = playerPos - position;
        
            tr.LookAt(playerPos);
            
            enemyMoveState.SetFindPlayer(true);
            
            ChangeState(EnemyState.Idle);
            ChangeState(BattleState.Battle);
            Invoke(nameof(ChangeStateToMove), Random.Range(1, 5));

            detectCol.radius = 15f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && battleState == BattleState.Battle)
        {
            Debug.Log("플레이어 놓침");
            
            enemyMoveState.SetFindPlayer(false);
            ChangeState(BattleState.Idle);
            ChangeState(EnemyState.Idle);

            detectCol.radius = 7f;
        }
    }

    IEnumerator CheckAttackRange()
    {
        while (true)
        {
            playerInRange =
                Vector3.Distance(player.transform.position, tr.position) <= enemyAttackState.GetAttackRange();

            yield return new WaitForSeconds(0.1f);
        }
    }
    private void ChangeStateToMove()
    {
            animator.SetTrigger("Buff End");
            ChangeState(EnemyState.Move);
    }
    
    private void GetDamage(float value)
    {
        // Debug.Log("GetDamage 매소드 작동");
        ChangeState(EnemyState.GetDamage);
        gameObject.SendMessage("AddDownGauge", value, SendMessageOptions.DontRequireReceiver);
    }
    
    private void BackToIdle()
    {
        ChangeState(EnemyState.Idle);
    }
}
