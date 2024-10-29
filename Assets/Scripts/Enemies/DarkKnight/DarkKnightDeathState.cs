using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkKnightDeathState : EnemyState
{
    private readonly DarkKnight darkKnight;
    private bool isPlayDeathFX;

    public DarkKnightDeathState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animName, DarkKnight _darkKnight) : base(_enemy, _stateMachine, _animName)
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
            darkKnight.FX.PlayDeathFX();
        }
    }
}
