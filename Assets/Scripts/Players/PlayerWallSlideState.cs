using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    private readonly float wallSlideSpeed = .4f;

    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (controller != null)
        {
            controller.OnJumpAction += PlayerController_OnJumpAction;
        }

        isJumping = false;
    }
    
    public override void Exit()
    {
        base.Exit();

        if (controller != null)
        {
            controller.OnJumpAction -= PlayerController_OnJumpAction;
        }

        isJumping = false;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        yInput = yInput < 0 ? rb.velocity.y : (rb.velocity.y * wallSlideSpeed);
        player.SetVelocityWithFlip(0, yInput);

        if (isJumping)
        {
            stateMachine.ChangeState(player.WallJumpState);
        }
    }

    public override void Update()
    {
        base.Update();

        if (!player.IsWallDetected())
        {
            stateMachine.ChangeState(player.AirState);
        }

        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }

    private void PlayerController_OnJumpAction(object sender, System.EventArgs e)
    {
        if (isJumping) return;

        isJumping = true;
    }
}
