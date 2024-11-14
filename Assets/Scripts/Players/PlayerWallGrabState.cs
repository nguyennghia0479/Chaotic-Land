using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallGrabState : PlayerState
{
    private float defaultGravityScale;
    private readonly float grabWallTime = 2;

    public PlayerWallGrabState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (controller != null)
        {
            controller.OnJumpAction += PlayerController_OnJumpAction;
        }

        defaultGravityScale = rb.gravityScale;
        rb.gravityScale = 0;
        stateTimer = grabWallTime;
        isJumping = false;
        player.IsWallGrab = true;
    }

    public override void Exit()
    {
        base.Exit();

        if (controller != null)
        {
            controller.OnJumpAction -= PlayerController_OnJumpAction;
        }

        rb.gravityScale = defaultGravityScale;
        isJumping = false;

        player.IsWallGrab = false;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        player.SetZeroVelocity();

        if (xInput != 0 && xInput != player.FacingDir && isJumping)
        {
            stateMachine.ChangeState(player.WallJumpState);
        }

        if (isJumping)
        {
            stateMachine.ChangeState(player.JumpState);
        }
    }

    public override void Update()
    {
        base.Update();

        if (!player.IsWallDetected())
        {
            stateMachine.ChangeState(player.AirState);
        }

        if (stateTimer < 0 || yInput < 0)
        {
            stateMachine.ChangeState(player.WallSlideState);
        }
    }

    private void PlayerController_OnJumpAction(object sender, System.EventArgs e)
    {
        if (isJumping) return;

        isJumping = true;
    }
}
