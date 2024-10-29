using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallKnightJumpState : EnemyState
{
    private readonly FallKnight fallKnight;
    private Player player;

    public FallKnightJumpState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animName, FallKnight _fallKnight) : base(_enemy, _stateMachine, _animName)
    {
        fallKnight = _fallKnight;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.Instance.Player;
        float facingDir = player.transform.position.x < fallKnight.transform.position.x ? -1 : 1;

        fallKnight.AddForce(facingDir);
        soundManager.PlayJumpSound(fallKnight.transform.position);
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

        if (rb.velocity.y < 0)
        {
            stateMachine.Changestate(fallKnight.AirState);
        }
    }
}
