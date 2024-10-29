using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallKnightAttackState : EnemyState
{
    private readonly FallKnight fallKnight;

    private const string ATTACK_COMBO = "AttackCombo";

    public FallKnightAttackState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animName, FallKnight _fallKnight) : base(_enemy, _stateMachine, _animName)
    {
        fallKnight = _fallKnight;
    }

    public override void Enter()
    {
        base.Enter();

        int attackCombo = Random.Range(0, 3);
        anim.SetInteger(ATTACK_COMBO, attackCombo);
    }

    public override void Exit()
    {
        base.Exit();

        lastTimeAttacked = Time.time;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        fallKnight.SetZeroVelocity();
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            stateMachine.Changestate(fallKnight.AggroState);
        }
    }
}
