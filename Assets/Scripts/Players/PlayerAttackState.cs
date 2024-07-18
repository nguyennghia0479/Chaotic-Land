using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    private int attackCombo;
    private float lastTimeAttacked;

    private const string ATTACK_COMBO = "AttackCombo";

    public PlayerAttackState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (attackCombo > 2 || Time.time > lastTimeAttacked + player.AttackCooldown)
        {
            attackCombo = 0;
        }

        anim.SetInteger(ATTACK_COMBO, attackCombo);
        AttackVelocity();
    }

    public override void Exit()
    {
        base.Exit();
        attackCombo++;
        lastTimeAttacked = Time.time;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        if (triggerCalled)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }

    /// <summary>
    /// Handles to move and flippng of the character by attack.
    /// </summary>
    private void AttackVelocity()
    {
        int attackDir = player.FacingDir;
        xInput = 0;

        if (xInput != 0)
        {
            attackDir = (int)xInput;
        }

        Vector2 attackMovement = player.AttackMovements[attackCombo];
        player.SetVelocityWithFlip(attackMovement.x * attackDir, attackMovement.y);
    }
}
