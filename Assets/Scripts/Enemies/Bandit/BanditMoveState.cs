using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditMoveState : BanditGroundedState
{
    private float slopedSpeed = 0;
    private float footstepTimer;
    private readonly float footstepTimerMax = .5f;

    public BanditMoveState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animName, Bandit _bandit) : base(_enemy, _stateMachine, _animName, _bandit)
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

        int moveSpeed = Mathf.CeilToInt(bandit.MoveSpeed + bandit.MoveSpeed * slopedSpeed);
        bandit.SetVelocityWithFlip(moveSpeed * bandit.FacingDir, rb.velocity.y);
        PlayFootstepsSound();
    }

    public override void Update()
    {
        base.Update();

        if (bandit.IsImmobilized) return;

        slopedSpeed = bandit.IsSlopeDetected() ? .15f : 0;

        if (bandit.IsWallDetected() || (!bandit.IsGroundDetected() && !bandit.IsSlopeDetected()))
        {
            bandit.Flip();
            stateMachine.Changestate(bandit.IdleState);
        }
    }

    private void PlayFootstepsSound()
    {
        footstepTimer -= Time.deltaTime;
        if (footstepTimer < 0)
        {
            footstepTimer = footstepTimerMax;
            soundManager.PlayFootstepSound(enemy.transform.position);
        }
    }
}
