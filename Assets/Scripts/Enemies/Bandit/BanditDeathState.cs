using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditDeathState : EnemyState
{
    private readonly Bandit bandit;
    private bool isPlayDeathFX;

    public BanditDeathState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animName, Bandit _bandit) : base(_enemy, _stateMachine, _animName)
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
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        bandit.SetZeroVelocity();
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled && !isPlayDeathFX)
        {
            isPlayDeathFX = true;
            bandit.FX.PlayDeathFX();
        }
    }
}
