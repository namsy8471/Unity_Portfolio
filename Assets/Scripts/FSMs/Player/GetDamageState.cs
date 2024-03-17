using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GetDamageState : IStateBase
{
    private enum KnockDownState
    {
        GetDamage,
        KnockDown
    }
    
    private float timer;
    private float getDelayTime;
    private float downDelayTime;
    private float randFloat;

    private float downGauge;
    private KnockDownState knockDownState;
    
    private Animator animator;
    private Rigidbody rb;
    
    public void Init()
    {
        animator = Managers.Game.Player.GetComponentInChildren<Animator>();
        rb = Managers.Game.Player.GetComponent<Rigidbody>();
        
        downGauge = 0.0f;

        getDelayTime = 0.8f;
        downDelayTime = 2.6f;
    }

    public void StartState()
    {
        // Debug.Log("GetDamage State Start");

        timer = 0;
        randFloat = Random.value;
        
        animator.SetFloat("RandomDamageGesture", randFloat);
        animator.SetTrigger("GetDamage");
        knockDownState = KnockDownState.GetDamage;
    }

    public void UpdateState()
    {
        // Debug.Log("GetDamage State Update");

        timer += Time.deltaTime;
        
        switch (knockDownState)
        {
            case KnockDownState.GetDamage:
                if (downGauge >= 100)
                {
                    ChnageState(KnockDownState.KnockDown);
                    break;
                }

                if (timer >= getDelayTime)
                {
                    Managers.Game.Player.gameObject.SendMessage("BackToIdle", SendMessageOptions.DontRequireReceiver);
                    break;
                }
                break;
            case KnockDownState.KnockDown:
                
                if (timer >= downDelayTime)
                {
                    Managers.Game.Player.gameObject.SendMessage("BackToIdle", SendMessageOptions.DontRequireReceiver);
                    break;
                }
                break;
            default:
                break;
        }
    }

    public void EndState()
    {
        // Debug.Log("GetDamage State End");

    }

    private void ChnageState(KnockDownState state)
    {
        switch (knockDownState)
        {
            case KnockDownState.GetDamage:
                break;
            case KnockDownState.KnockDown:
                break;
            default:
                break;
        }
        
        knockDownState = state;

        switch (knockDownState)
        {
            case KnockDownState.GetDamage:
                break;
            case KnockDownState.KnockDown:
                timer = 0;
                downGauge = 0;
                animator.SetTrigger("Down");
                rb.AddForce(new Vector3(0, 1, -1) * 5.0f, ForceMode.Impulse);
                InvincibleTime();
                break;
            default:
                break;
        }
    }
    
    private void AddDownGauge(float value)
    {
        downGauge += value;
    }

    private void InvincibleTime()
    {
        Collider[] colliders = Physics.OverlapSphere(Managers.Game.Player.transform.position, 10, 1 << LayerMask.NameToLayer("Enemy"));
        foreach (var col in colliders)
        {
            col.gameObject.SendMessage("AttackFinish", SendMessageOptions.DontRequireReceiver);
        }
    }
    
}
