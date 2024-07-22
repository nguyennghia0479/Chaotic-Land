using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = skillManager.DashSkill.DashDuration;
        skillManager.DashSkill.CreateCloneOnDash();
    }

    public override void Exit()
    {
        base.Exit();

        player.SetZeroVelocity();
        skillManager.DashSkill.CreateCloneOnDash();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        player.SetVelocityWithFlip(skillManager.DashSkill.DashSpeed * player.FacingDir, rb.velocity.y);
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
    }
}
