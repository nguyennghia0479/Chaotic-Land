using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    private bool isAttacking;

    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (controller != null)
        {
            controller.OnJumpAction += PlayerController_OnJumpAction;
            controller.OnAttackAction += PlayerController_OnAttackAction;
        }

        isJumping = false;
        isAttacking = false;
    }

    public override void Exit()
    {
        base.Exit();

        if (controller != null)
        {
            controller.OnJumpAction -= PlayerController_OnJumpAction;
            controller.OnAttackAction -= PlayerController_OnAttackAction;
        }

        isJumping = false;
        isAttacking= false;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (isJumping)
        {
            stateMachine.ChangeState(player.JumpState);
        }

        if (isAttacking)
        {
            stateMachine.ChangeState(player.AttackState);
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

    private void PlayerController_OnAttackAction(object sender, System.EventArgs e)
    {
        if (isAttacking || player.GameManager.IsGamePaused || playerStats.CurrentStamina < player.AttackStaminaAmount) return;

        isAttacking = true;
        playerStats.DecreaseStamina(player.AttackStaminaAmount);
    }
}
