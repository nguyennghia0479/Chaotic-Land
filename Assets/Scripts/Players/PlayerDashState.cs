using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    private DashSkill dashSkill;

    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        dashSkill = skillManager.DashSkill;
        stateTimer = dashSkill.DashDuration;

        if (dashSkill.IsCloneOnStart)
        {
            dashSkill.CreateCloneOnDash();
        }

        soundManager.PlayDashSound(player.transform.position);
    }

    public override void Exit()
    {
        base.Exit();

        player.SetZeroVelocity();

        if (dashSkill.IsCloneOnArrival)
        {
            dashSkill.CreateCloneOnDash();
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        player.SetVelocityWithFlip(dashSkill.DashSpeed * player.FacingDir, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(player.IdleState);
        }

        if (player.IsWallDetected() && !player.IsGroundDetected() && !player.IsSlopeDetected())
        {
            stateMachine.ChangeState(player.WallGrabState);
        }

        (player.FX as PlayerFX).PlayAfterImage();
    }
}
