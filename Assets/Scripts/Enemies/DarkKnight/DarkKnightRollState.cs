using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkKnightRollState : EnemyState
{
    private readonly DarkKnight darkKnight;

    public DarkKnightRollState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animName, DarkKnight _darkKnight) : base(_enemy, _stateMachine, _animName)
    {
        darkKnight = _darkKnight;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = .5f;

        darkKnight.Stats.MarkInvisible(true);
    }

    public override void Exit()
    {
        base.Exit();

        darkKnight.SetZeroVelocity();

        darkKnight.Stats.MarkInvisible(false);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        darkKnight.SetVelocity(10 * darkKnight.FacingDir, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
            stateMachine.Changestate(darkKnight.AggroState);
        }
    }
}
