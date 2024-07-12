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

        controller.OnJumpAction += PlayerController_OnJumpAction;
        isJumping = false;
    }

    public override void Exit()
    {
        base.Exit();

        controller.OnJumpAction -= PlayerController_OnJumpAction;
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
    }

    private void PlayerController_OnJumpAction(object sender, System.EventArgs e)
    {
        if (isJumping || !player.IsGroundDetected()) return;

        isJumping = true;
    }
}
