using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkKnightJumpState : EnemyState
{
    private readonly DarkKnight darkKnight;
    private Player player;  

    public DarkKnightJumpState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animName, DarkKnight _darkKnight) : base(_enemy, _stateMachine, _animName)
    {
        darkKnight = _darkKnight;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.Instance.Player;

        float xVelocity = Vector2.Distance(player.transform.position, darkKnight.transform.position);
        float facingDir = player.transform.position.x < darkKnight.transform.position.x ? -1 : 1;
        
        darkKnight.AddForce(xVelocity * facingDir);
        soundManager.PlayJumpSound(darkKnight.transform.position);
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
            stateMachine.Changestate(darkKnight.JumpAttackState);
        }
    }
}
