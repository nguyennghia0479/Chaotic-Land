using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditIdleState : BanditGroundedState
{
    public BanditIdleState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animName, Bandit _bandit) : base(_enemy, _stateMachine, _animName, _bandit)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = bandit.IdleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
            stateMachine.Changestate(bandit.MoveState);
        }
    }
}
