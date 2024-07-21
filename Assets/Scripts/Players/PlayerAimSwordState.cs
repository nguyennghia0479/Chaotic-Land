using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (controller != null)
        {
            controller.OnAimActionEnd += PlayerController_OnAimActionEnd;
        }

        player.IsAiming = true;
    }

    public override void Exit()
    {
        base.Exit();

        if (controller != null)
        {
            controller.OnAimActionEnd -= PlayerController_OnAimActionEnd;
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        HandleAimPosition();
    }

    public override void Update()
    {
        base.Update();

        if (player.HasThrown)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }

    /// <summary>
    /// Handles to set aim position by mouse.
    /// </summary>
    private void HandleAimPosition()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        player.SetAimAndCatchSwordFlip(mousePos);
        player.SetZeroVelocity();
    }

    /// <summary>
    /// Handles to cancel aiming when thrown a sword.
    /// </summary>
    private void PlayerController_OnAimActionEnd(object sender, System.EventArgs e)
    {
        if (player.HasThrown) return;

        player.IsAiming = false;
        player.HasThrown = true;
    }
}
