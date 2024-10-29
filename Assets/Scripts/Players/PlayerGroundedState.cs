using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
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

        if (isJumping)
        {
            stateMachine.ChangeState(player.JumpState);
        }
    }

    public override void Update()
    {
        base.Update();

        if (!player.IsGroundDetected() && !player.IsSlopeDetected()) {
            stateMachine.ChangeState(player.AirState);
        }
    }

    private void PlayerController_OnJumpAction(object sender, System.EventArgs e)
    {
        if (isJumping || (!player.IsGroundDetected() && !player.IsSlopeDetected()) || player.GameManager.IsGamePaused) return;

        isJumping = true;
    }
}
