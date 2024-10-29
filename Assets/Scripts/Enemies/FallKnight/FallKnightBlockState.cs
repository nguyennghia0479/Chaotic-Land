using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallKnightBlockState : EnemyState
{
    private readonly FallKnight fallKnight;
    private bool hasCounter;

    private const string COUNTER = "Counter";

    public FallKnightBlockState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animName, FallKnight _fallKnight) : base(_enemy, _stateMachine, _animName)
    {
        fallKnight = _fallKnight;
    }

    public override void Enter()
    {
        base.Enter();

        fallKnight.IsBlocking = true;
        hasCounter = false;
        anim.SetBool(COUNTER, false);
    }

    public override void Exit()
    {
        base.Exit();

        fallKnight.IsBlocking = false;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(fallKnight.AttackCheck.position, fallKnight.AttackRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out Player player))
            {
                if (!player.Stats.IsCriticalAttack && !hasCounter)
                {
                    hasCounter = true;
                    anim.SetBool(COUNTER, true);
                }
            }
        }

        if (triggerCalled)
        {
            stateMachine.Changestate(fallKnight.AggroState);
        }
    }
}
