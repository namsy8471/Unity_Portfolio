using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : MonoBehaviour, IStateBase
{
    
    enum IdleType
    {
        Idle,
        Battle
    }

    private IdleType idleType;
    private Animator animator;
    
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void StartState()
    {
        // Debug.Log("Idle State Start!");
        
        if (animator.GetBool("isBattle"))
        {
            idleType = IdleType.Battle;
        }
        
        else
        {
            idleType = IdleType.Idle;
        }
    }

    public void UpdateState()
    {
        // Debug.Log("Idle State Update!");

        switch (idleType)
        {
            case IdleType.Idle:
                
                break;
            
            case IdleType.Battle:

                break;
        }
    }

    public void EndState()
    {
        // Debug.Log("Idle State End!");
    }
}
