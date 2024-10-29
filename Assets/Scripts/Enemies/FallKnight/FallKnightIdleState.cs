using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallKnightIdleState : EnemyState
{
    private readonly FallKnight fallKnight;
    private Player player;

    public FallKnightIdleState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animName, FallKnight _fallKnight) : base(_enemy, _stateMachine, _animName)
    {
        fallKnight = _fallKnight;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.Instance.Player;
        fallKnight.Stats.MarkInvisible(true);
    }

    public override void Exit()
    {
        base.Exit();

        fallKnight.Stats.MarkInvisible(false);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();

        if (Vector2.Distance(player.transform.position, fallKnight.transform.position) < 10)
        {
            stateMachine.Changestate(fallKnight.AggroState);
        }
    }
}
