using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGetDamageState : IStateBase
{

    public float Timer { get; private set;}
    
    private Animator _animator;
    private EnemyController _controller;

    public EnemyGetDamageState(GameObject go) => _controller = go.GetComponent<EnemyController>();
    
    public void Init()
    {
        _animator = _controller.GetComponentInChildren<Animator>();
    }

    public void StartState()
    {
        // Debug.Log("GetDamage State Start");

        Timer = 2;
        
        var pos = Managers.Game.Player.transform.position;
        _controller.transform.LookAt(pos);

        var player = Managers.Game.Player.GetComponent<PlayerController>();
        var playerStatus = player.Status;
        var playerSkill = player.CurrentSkill;

        _controller.DownGauge += (playerSkill as AttackSkill)?.SkillDownGauge ?? playerStatus.DownGaugeToHit;
        _controller.Status.Hp -= (Random.Range(playerStatus.MinDmg, playerStatus.MaxDmg + 1)
                                  + ((playerSkill as AttackSkill)?.SkillDamage ?? 0))
                                  * ((playerSkill as AttackSkill)?.SkillDamageRatio ?? 1);

        if (playerSkill is CounterAttack)
        {
            _controller.DownGauge = 101;
            _controller.Status.Hp -= Random.Range(_controller.Status.MinDmg, _controller.Status.MaxDmg + 1) * 1.5f;
        }
        
        playerSkill?.StopSkill();
        
        
        Debug.Log("적 DownGauge = " + _controller.DownGauge);
        
        if (_controller.DownGauge >= 100)
        {
            _controller.ChangeState(EnemyController.EnemyState.Down);
            return;
        }
        
        if (_controller.Status.Hp <= 0)
        {
            _controller.ChangeState(EnemyController.EnemyState.Dead);
            return;
        }

        
        _animator.SetTrigger("Get Hit Front");
    }

    public void UpdateState()
    {
        Timer -= Time.deltaTime;
    }

    public void EndState()
    {
        _animator.SetBool("Stunned Loop", false);
    }
}
