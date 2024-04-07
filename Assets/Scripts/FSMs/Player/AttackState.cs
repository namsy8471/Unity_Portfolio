using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AttackState : IStateBase
{
    
     /*
     * 내가 필요한 거
     * 사거리? => 플레이어 스탯에서 들고오면 됨
     * 어택 쿨다운? => 플레이어 스탯에서 공속을 만들자!
     * 대미지 => 플레이어 스탯에서!
     * 최대 공격횟수 => 플레이어스탯
     */
     
    private float _lastAttackCooldown;
    private float _attackTimer;
    
    private Animator _animator;

    public void Init()
    {
        _animator = Managers.Game.Player.GetComponentInChildren<Animator>();
        
        _lastAttackCooldown = 2.0f;
        _attackTimer = 0;
    }

    public void StartState()
    {
        _attackTimer = 0;

        Debug.Log("Attack State Start");
        
        if (Managers.Game.Player.GetComponent<Stat>().Stamina < 3f) return;
        Managers.Game.Player.GetComponent<Stat>().Stamina -= 3f;
        
        switch (Managers.Game.Player.GetComponent<PlayerStat>().AtkStyle)
        {
            case ItemDataWeapon.AttackStyle.Punch:
                _animator.Play("HandAtk" + Random.Range(1, 4));
                break;
            case ItemDataWeapon.AttackStyle.Sword:
                _animator.Play("Atk" + Random.Range(1, 4));
                break;
            case ItemDataWeapon.AttackStyle.Bow:
                break;
            case ItemDataWeapon.AttackStyle.Wand:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void UpdateState()
    {
        Debug.Log("Attack State Update");

        // _attackTimer += Time.deltaTime;
        //
        // if(_attackTimer >= 2)
        //     ChangeState(AtkState.AtkFinish);
        //
        // switch (_atkState)
        // {
        //     case AtkState.Idle:
        //         
        //         if (Input.GetMouseButtonDown(0) && !Managers.Game.TargetingSystem.GetCurrentTarget().GetComponent<EnemyGetDamageState>().IsInvincible())
        //         {
        //             ChangeState(AtkState.Atk1);
        //             break;
        //         }
        //         
        //         break;
        //     
        //     case AtkState.Atk1:
        //         if (Input.GetMouseButtonDown(0) && _attackTimer >= Managers.Game.Player.GetComponent<Stat>().AtkSpeed)
        //         {
        //             ChangeState(AtkState.Atk2);
        //             break;
        //         }
        //         
        //         break;
        //     
        //     case AtkState.Atk2:
        //         if (Input.GetMouseButtonDown(0) && _attackTimer >= Managers.Game.Player.GetComponent<Stat>().AtkSpeed)
        //         {
        //             ChangeState(AtkState.Atk3);
        //             break;
        //         }
        //         break;
        //     
        //     case AtkState.Atk3:
        //         if (_attackTimer >= _lastAttackCooldown)
        //         {
        //             ChangeState(AtkState.AtkFinish);
        //             break;
        //         }
        //         break;
        //     case AtkState.AtkFinish:
        //         
        //         break;
        // }
    }

    public void EndState()
    {
        Debug.Log("Attack State End!");
        // _atkState = AtkState.Idle;
    }
    
    public void Attack()
    {
        var targetPos = Managers.Game.TargetingSystem.GetCurrentTarget().transform.position;
        targetPos.y = Managers.Game.Player.transform.position.y;
        
        Managers.Game.Player.transform.LookAt(targetPos);
        
        Managers.Game.TargetingSystem.GetCurrentTarget().GetComponent<EnemyFSM>().
            GetDamage(Managers.Game.Player.GetComponent<Stat>().DownGaugeToHit);
        
        Managers.Input.StopInputUpdate();
        IEnumerator coroutine = RestartInputUpdate();

        Debug.Log("코루틴 작동 전");

        while (coroutine.MoveNext())
        {
            Debug.Log("코루틴 작동");
        }
    }

    IEnumerator RestartInputUpdate()
    {
        yield return new WaitForSeconds(Managers.Game.Player.GetComponent<PlayerStat>().AtkSpeed);
        AttackEnd();
        Debug.Log("코루틴 종료");
    }
    
    private void AttackEnd()
    {
        Managers.Input.StartInputUpdate();
    }

    private void ChangeState()
    {
        // switch (_atkState)
        // {
        //     case AtkState.Idle:
        //         break;
        //     case AtkState.Atk1:
        //         break;
        //     case AtkState.Atk2:
        //         break;
        //     case AtkState.Atk3:
        //         break;
        //     default:
        //         break;
        // }
        //
        // _atkState = state;
        // _attackTimer = 0;
        //
        // switch (_atkState)
        // {
        //     case AtkState.Idle:
        //         break;
        //     case AtkState.Atk1:
        //         _animator.SetTrigger("Attack");
        //         Attack();
        //         break;
        //     case AtkState.Atk2:
        //         _animator.SetTrigger("NextAttack");
        //         Attack();
        //         break;
        //     case AtkState.Atk3:
        //         _animator.SetTrigger("LastAttack");
        //         Attack();
        //         break;
        //     case AtkState.AtkFinish:
        //         Managers.Game.Player.gameObject.SendMessage("BackToIdle", SendMessageOptions.DontRequireReceiver);
        //         break;
        //     default:
        //         break;
        // }
        
    }
    
}
