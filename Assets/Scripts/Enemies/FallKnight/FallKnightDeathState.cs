using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallKnightDeathState : EnemyState
{
    private readonly FallKnight fallKnight;
    private bool isPlayDeathFX;

    public FallKnightDeathState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animName, FallKnight _fallKnight) : base(_enemy, _stateMachine, _animName)
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

        if (triggerCalled && !isPlayDeathFX)
        {
            isPlayDeathFX = true;
            fallKnight.FX.PlayDeathFX();
        }
    }
}
