using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockState : PlayerState
{
    private readonly float timeToParry = .2f;
    private const string COUNTER_SUCCESS = "CounterSuccess";
    private bool hasCreateClone;
    private bool hasPlaySound;

    public PlayerBlockState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetZeroVelocity();
        stateTimer = timeToParry;
        anim.SetBool(COUNTER_SUCCESS, false); // Reset to block animation.
        hasCreateClone = false;
        hasPlaySound = false;
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

        if (playerStats.CurrentStamina <= 0)
        {
            player.CancelBlock();

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
                    anim.SetBool(COUNTER_SUCCESS, true);
                    skillManager.ParrySkill.RestoreHealth();
                    PlayParrySound();

                    if (!hasCreateClone)
                    {
                        hasCreateClone = true;
                        skillManager.ParrySkill.CanCreateClone(enemy.transform);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Handles to play parry sound.
    /// </summary>
    private void PlayParrySound()
    {
        if (SoundManager.Instance != null && !hasPlaySound)
        {
            hasPlaySound = true;
            SoundManager.Instance.PlayParrySound(player.transform.position);
        }
    }
}
