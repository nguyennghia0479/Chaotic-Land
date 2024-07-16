using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetZeroVelocity();

        if (player.IsSlopeDetected())
        {
            rb.gravityScale = 0;
        }
    }

    public override void Exit()
    {
        base.Exit();

        rb.gravityScale = 4;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();

        if (xInput == player.FacingDir && player.IsWallDetected())
        {
            return;
        }

        if (xInput != 0)
        {
            stateMachine.ChangeState(player.MoveState);
        }
    }
}
