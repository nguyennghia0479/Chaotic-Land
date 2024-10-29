using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkKnightStandupState : EnemyState
{
    private readonly DarkKnight darkKnight;

    public DarkKnightStandupState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animName, DarkKnight _darkKnight) : base(_enemy, _stateMachine, _animName)
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

        darkKnight.Stats.MarkInvisible(false);
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
