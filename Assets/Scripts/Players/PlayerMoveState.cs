using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    private float footstepTimer;
    private readonly float footstepTimerMax = .5f;

    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        footstepTimer = 0;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        float xVelocity = xInput * player.MoveSpeed;
        player.SetVelocityWithFlip(xVelocity, rb.velocity.y);
        PlayFootstepSound();
    }

    public override void Update()
    {
        base.Update();

        if (xInput == 0 || player.IsWallDetected())
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }

    /// <summary>
    /// Handles to play footstep sound.
    /// </summary>
    private void PlayFootstepSound()
    {
        footstepTimer -= Time.deltaTime;
        if (footstepTimer < 0)
        {
            footstepTimer = footstepTimerMax;
            soundManager.PlayFootstepSound(player.transform.position);
        }
    }
}
