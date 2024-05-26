using System;
using System.Collections;
using System.Collections.Generic;
using Contents.Status;
using UnityEngine;
using Random = UnityEngine.Random;

public class AttackState : IStateBase
{

    private PlayerController _controller;
    private Animator _animator;

    public AttackSkill Skill { get; set;}
    public float LastAttackCooldown { get; private set; }
    public float AttackTimer { get; private set; }
    public float AttackCount { get; private set; }

    public void Init()
    {
        _controller = Managers.Game.Player.GetComponent<PlayerController>();
        _animator = _controller.GetComponent<Animator>();
        
        LastAttackCooldown = 2.0f;
        AttackTimer = 0;
        AttackCount = 0;
    }

    public void StartState()
    {
        Debug.Log("Attack State Start");

        _controller.IsAttackReserved = false;
        
        Skill = _controller.CurrentSkill as AttackSkill;
        if (Skill != null)
        {
            AttackTimer = Skill.SkillUseAnimClip.length;
            Skill.UseSkill();
        }
        
        else
        {
            AttackTimer = _controller.Status.AtkSpeed;
            if (_controller.Status.Stamina < 3f) return;
            _controller.Status.Stamina -= 3f;
            AttackCount++;

            switch (_controller.Status.AtkStyle)
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
        AttackTimer -= Time.deltaTime;
    }

    public void EndState()
    {
        
    }
    
    public void Attack(EnemyController enemyController)
    {
        var targetPos = enemyController.transform.position;
        targetPos.y = _controller.transform.position.y;
        
        _controller.transform.LookAt(targetPos);
        AttackTimer = _animator.GetCurrentAnimatorStateInfo(0).length + 0.4f;
        
        enemyController.GetDamage();

        _controller.CurrentEnemy = null;
        
        Debug.Log("애니메이션 클립 시간 : " + AttackTimer);
    }
}
