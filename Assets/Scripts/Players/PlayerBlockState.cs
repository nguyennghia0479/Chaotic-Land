using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockState : PlayerState
{
    private float timeToParry = .2f;

    public PlayerBlockState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetZeroVelocity();
        stateTimer = timeToParry;
        anim.SetBool("CounterSuccess", false); // Reset to block animation.
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();

        PerformParry();

        if (triggerCalled)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }

    /// <summary>
    /// Handles to perform parry of the charater in limit time.
    /// </summary>
    /// <remarks>
    /// If parry success will be perform counter animation.
    /// </remarks>
    private void PerformParry()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.AttackCheck.position, player.AttackRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out Enemy enemy))
            {
                if (stateTimer > 0 && enemy.CanBeStunned())
                {
                    anim.SetBool("CounterSuccess", true);
                }
            }
        }
    }
}
