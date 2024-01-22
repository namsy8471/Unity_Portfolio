using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : MonoBehaviour, IStateBase
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void StartState()
    {
        // Debug.Log("Enemy Idle Start");
        animator.SetBool("Idle", true);
    }

    public void UpdateState()
    {
        // Debug.Log("Enemy Idle Update");
    }

    public void EndState()
    {
        // Debug.Log("Enemy Idle End");
        animator.SetBool("Idle", false);
    }
}
