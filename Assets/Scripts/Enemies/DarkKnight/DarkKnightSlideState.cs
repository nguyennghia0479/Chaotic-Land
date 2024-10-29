using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkKnightSlideState : EnemyState
{
    private readonly DarkKnight darkKnight;

    public DarkKnightSlideState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animName, DarkKnight _darkKnight) : base(_enemy, _stateMachine, _animName)
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

        darkKnight.SetZeroVelocity();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        darkKnight.SetVelocityWithFlip(10 * darkKnight.FacingDir, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled || darkKnight.IsWallDetected() || darkKnight.IsSlopeDetected())
        {
            stateMachine.Changestate(darkKnight.AggroState);
        }
    }
}
