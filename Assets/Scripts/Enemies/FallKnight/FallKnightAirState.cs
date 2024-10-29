using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallKnightAirState : EnemyState
{
    private readonly FallKnight fallKnight;

    public FallKnightAirState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animName, FallKnight _fallKnight) : base(_enemy, _stateMachine, _animName)
    {
        fallKnight = _fallKnight;
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
    }

    public override void Update()
    {
        base.Update();

        if (fallKnight.IsGroundDetected() || fallKnight.IsSlopeDetected())
        {
            stateMachine.Changestate(fallKnight.AggroState);
        }
    }
}
