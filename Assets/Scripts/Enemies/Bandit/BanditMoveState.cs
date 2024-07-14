using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditMoveState : BanditGroundedState
{
    public BanditMoveState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animName, Bandit _bandit) : base(_enemy, _stateMachine, _animName, _bandit)
    {
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

        bandit.SetVelocity(bandit.MoveSpeed * bandit.FacingDir, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (bandit.IsWallDetected() || !bandit.IsGroundDetected())
        {
            bandit.Flip();
            stateMachine.Changestate(bandit.IdleState);
        }
    }
}
