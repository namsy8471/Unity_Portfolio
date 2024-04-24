using System;
using System.Collections;
using System.Collections.Generic;
using Contents.Status;
using UnityEditor.Timeline.Actions;
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
    
    private Animator _animator;
    
    public float LastAttackCooldown { get; private set; }
    public float AttackTimer { get; private set; }
    public float AttackCount { get; private set; }
    
    public void Init()
    {
        _animator = Managers.Game.Player.GetComponentInChildren<Animator>();
        
        LastAttackCooldown = 2.0f;
        AttackTimer = 0;
        AttackCount = 0;
    }

    public void StartState()
    {
        AttackTimer = 0;

        Debug.Log("Attack State Start");

        var player = Managers.Game.Player.GetComponent<PlayerController>();

        player.IsAttackReserved = false;
        
        var playerSkill = player.CurrentSkill;
        if (playerSkill != null)
        {
            player.SetRootAnimFalseInvoke();
            playerSkill.UseSkill();
        }
        
        else
        {
            if (player.Status.Stamina < 3f) return;
            player.Status.Stamina -= 3f;
            AttackCount++;

            switch (player.Status.AtkStyle)
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
    }

    public void UpdateState()
    {
        Debug.Log("Attack State Update");

        AttackTimer += Time.deltaTime;

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
        //Managers.Game.TargetingSystem.Target = null;
        // _atkState = AtkState.Idle;
    }
    
    public void Attack()
    {
        var player = Managers.Game.Player;
        var targetSys = Managers.Game.TargetingSystem;
        
        var targetPos = targetSys.IsCurrentTargetExist() ?
            targetSys.GetCurrentTarget().transform.position :
            player.GetComponent<PlayerController>().IdleState.DestPos;
        
        targetPos.y = player.transform.position.y;
        
        player.transform.LookAt(targetPos);
        
        // Managers.Game.TargetingSystem.GetCurrentTarget().GetComponent<EnemyController>().
        //     GetDamage(player.GetComponent<PlayerController>().Status.DownGaugeToHit);
    }
    
}
