using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkKnightJumpAttackState : EnemyState
{
    private readonly DarkKnight darkKnight;

    public DarkKnightJumpAttackState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animName, DarkKnight _darkKnight) : base(_enemy, _stateMachine, _animName)
    {
        darkKnight = _darkKnight;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        lastTimeAttacked = Time.time;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
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
