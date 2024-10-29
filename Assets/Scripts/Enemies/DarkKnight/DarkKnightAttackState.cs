using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkKnightAttackState : EnemyState
{
    private readonly DarkKnight darkKnight;
    private int attackCombo;
    private float lastTimeAttackCombo;

    private const string ATTACK_COMBO = "AttackCombo";

    public DarkKnightAttackState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animName, DarkKnight _darkKnight) : base(_enemy, _stateMachine, _animName)
    {
        darkKnight = _darkKnight;
    }

    public override void Enter()
    {
        base.Enter();

        if (attackCombo > 1 || Time.time > lastTimeAttackCombo + 5)
        {
            attackCombo = 0;
        }

        anim.SetInteger(ATTACK_COMBO, attackCombo);
    }

    public override void Exit()
    {
        base.Exit();

        attackCombo++;
        lastTimeAttacked = Time.time;
        lastTimeAttackCombo = Time.time;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        darkKnight.SetZeroVelocity();
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            stateMachine.Changestate(darkKnight.AggroState);
        }
    }
}
