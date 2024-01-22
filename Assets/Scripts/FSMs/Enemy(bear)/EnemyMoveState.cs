using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyMoveState : MonoBehaviour, IStateBase
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
    
    private WalkType walkType;
    private MovingState moveState;
    private PatrolState patrolState;
    
    private GameObject player;
    private Animator animator;
    
    private Transform tr;
    private Rigidbody rb;
    
    // 속도 관련 변수들
    [SerializeField]private float walkSpeedOffset = 100f;
    [SerializeField]private float runSpeedOffset = 500f;

    [SerializeField] private float speed = 100f;

    
    // 주변 배회 관련 변수들
    private float patrolTimer;
    private float randomPatrolTime;
    
    private bool patrolDone;
    private bool isFindPlayer;

    private Vector3 randDirection;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponentInChildren<Animator>();
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();

        patrolDone = false;
        isFindPlayer = false;
    }
    
    public void StartState()
    {
        // Debug.Log("Enemy Move State Update");

        ChangeState(isFindPlayer ? MovingState.Player : MovingState.Patrol);
        ChangeState(WalkType.Walk);
        
        patrolTimer = 0;
        patrolDone = false;
        randomPatrolTime = Random.Range(1, 4);
    }

    public void UpdateState()
    {
        // Debug.Log("Enemy Move State Update");

        switch (moveState)
        {
            case MovingState.Patrol:
                switch (patrolState)
                {
                    case PatrolState.SetRandPos:
                        randDirection = new Vector3(Random.value * 2 - 1, 0 , Random.value * 2 - 1).normalized;
                        
                        patrolState = PatrolState.Move;
                        break;
                    
                    case PatrolState.Move:
                        if (patrolTimer > randomPatrolTime)
                        {
                            patrolTimer = 0;
                            patrolState = PatrolState.Finish;
                            break;
                        }

                        patrolTimer += Time.deltaTime;
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
        switch (walkType)
        {
            case WalkType.Walk:
            {
                float percent = Random.Range(0, 301) / 300f;
                if (moveState == MovingState.Player && percent < 0.002f)
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

        rb.velocity = Vector3.zero;
        animator.SetBool("WalkForward", false);
        animator.SetBool("Run Forward", false);
    }

    void Patrol()
    {
        var lookDir = randDirection + tr.position;
        tr.LookAt(lookDir);
        rb.velocity = randDirection * (speed * Time.deltaTime);
    }
    
    void Chase()
    {
        var playerPos = player.transform.position;
        var pos = tr.position;
        playerPos.y = pos.y;
        
        var dir = playerPos - pos;
        
        tr.LookAt(playerPos);
        rb.velocity = dir.normalized * (speed * Time.deltaTime);
    }
    
    void ChangeState(WalkType state)
    {
        switch (walkType)
        {
            case WalkType.Walk:
                animator.SetBool("WalkForward", false);
                break;
            case WalkType.Run:
                animator.SetBool("Run Forward", false);

                break;
            default:
                break;
        }

        walkType = state;

        switch (walkType)
        {
            case WalkType.Walk:
                animator.SetBool("WalkForward", true);
                speed = walkSpeedOffset;
                break;
            case WalkType.Run:
                animator.SetBool("Run Forward", true);
                speed = runSpeedOffset;
                break;
            default:
                break;
        }
    }

    void ChangeState(MovingState state)
    {
        switch (moveState)
        {
            case MovingState.Patrol:
                break;
            case MovingState.Player:
                break;
            default:
                break;
        }

        moveState = state;

        switch (moveState)
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
        patrolState = PatrolState.SetRandPos;
        patrolDone = true;
    }

    public bool GetPatrolDone()
    {
        return patrolDone;
    }

    public void SetFindPlayer(bool value)
    {
        isFindPlayer = value;
    }
}
