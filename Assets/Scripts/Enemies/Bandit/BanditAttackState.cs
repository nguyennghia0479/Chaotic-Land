using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditAttackState : EnemyState
{
    private Bandit bandit;

    public BanditAttackState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animName, Bandit _bandit) : base(_enemy, _stateMachine, _animName)
    {
        bandit = _bandit;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        lastTimeAttacked = Time.time;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        bandit.SetZeroVelocity();
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            stateMachine.Changestate(bandit.AggroState);
        }
    }
}
