using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    private readonly float airMoveSpeed = .8f;

    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
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

        if (xInput != 0)
        {
            player.SetVelocityWithFlip(xInput * player.MoveSpeed * airMoveSpeed, rb.velocity.y);
        }
    }

    public override void Update()
    {
        base.Update();

        if (player.IsWallDetected())
        {
            stateMachine.ChangeState(player.WallGrabState);
        }

        if (player.IsGroundDetected() || player.IsSlopeDetected())
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }
}
