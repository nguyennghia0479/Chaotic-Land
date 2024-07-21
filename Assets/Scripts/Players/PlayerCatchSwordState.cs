using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    private readonly float swordCatchImpact = 3;

    public PlayerCatchSwordState(Player _player, PlayerStateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetAimAndCatchSwordFlip(player.Sword.transform.position);
        player.SetVelocity(swordCatchImpact * -player.FacingDir, rb.velocity.y);
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

        if (triggerCalled)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }
}
