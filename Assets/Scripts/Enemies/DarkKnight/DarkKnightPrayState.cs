using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkKnightPrayState : EnemyState
{
    private readonly DarkKnight darkKnight;
    private Player player;

    public DarkKnightPrayState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animName, DarkKnight _darkKnight) : base(_enemy, _stateMachine, _animName)
    {
        darkKnight = _darkKnight;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.Instance.Player;
        darkKnight.Stats.MarkInvisible(true);
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

        if (player.IsDead) return;

        if (Vector2.Distance(player.transform.position, darkKnight.transform.position) < 10)
        {
            stateMachine.Changestate(darkKnight.StandupState);
        }
    }
}
