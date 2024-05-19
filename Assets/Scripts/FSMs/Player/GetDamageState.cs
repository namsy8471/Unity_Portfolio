using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GetDamageState : IStateBase
{
    private float _getDelayTime;

    private PlayerController _controller;
    private Animator _animator;

    public DefendSkill DefenceSkill { get; set; }

    public float Timer { get; private set;}
    
    public void Init()
    {
        _controller = Managers.Game.Player.GetComponent<PlayerController>();
        _animator = _controller.GetComponent<Animator>();

        _getDelayTime = 1.0f;
    }

    public void StartState()
    {
        
    }

    public void StartState(EnemyController enemyController)
    {
        Timer = _getDelayTime;
        
        var enemyStatus = enemyController.Status;
        DefenceSkill = _controller.CurrentSkill as DefendSkill;
        
        float dmg = (Random.Range(enemyStatus.MinDmg, enemyStatus.MaxDmg + 1)
                    - (_controller.Status.Def + (DefenceSkill?.SkillDef ?? 0)))
                    * ((100 - (DefenceSkill?.SkillDefRatio ?? 0)) / 100);
        
        _controller.Status.Hp -= dmg > 0 ? dmg : DefenceSkill is not CounterAttack ? 1 : 0;
        _controller.DownGauge += enemyStatus.DownGaugeToHit;
        
        _controller.transform.LookAt(enemyController.transform);
        
        if (DefenceSkill is not null)
        {
            Timer = DefenceSkill is CounterAttack ? DefenceSkill.SkillUseAnimClip.length
                    : Timer - DefenceSkill.SkillUseAnimClip.length;
            _controller.DownGauge = _controller.DownGauge >= 50 ? _controller.DownGauge - 50 : 0;
            
            DefenceSkill.UseSkill();
        }
        
        else if(_controller.DownGauge >= 100 || _controller.Status.Hp <= 0)
        {
            _controller.ChangeState(PlayerController.PlayerState.Down);
        }
        
        else
        {
            var randNum = Random.Range(1, 3);
            _animator.Play("GetDamage" + randNum);
            Timer += _animator.GetCurrentAnimatorStateInfo(0).length;
        }
        
    }

    public void UpdateState()
    {
        Timer -= Time.deltaTime;
    }

    public void EndState()
    {
        if(DefenceSkill is not CounterAttack)
            DefenceSkill?.StopSkill();
    }
}
