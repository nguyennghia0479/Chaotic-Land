using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditStunnedState : EnemyState
{
    private Bandit bandit;

    public BanditStunnedState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animName, Bandit _bandit) : base(_enemy, _stateMachine, _animName)
    {
        bandit = _bandit;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = bandit.StunnedDuration;
        StunnedVelocity();
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

        if (stateTimer < 0)
        {
            stateMachine.Changestate(bandit.IdleState);
        }
    }

    /// <summary>
    /// Handles to set stunned velocity of the charater.
    /// </summary>
    private void StunnedVelocity()
    {
        float offset = Random.Range(0, bandit.StunnedOffset);
        float stunnedX = bandit.StunnedPower.x + offset;
        float stunnedY = bandit.StunnedPower.y + offset;
        bandit.SetVelocity(stunnedX * -bandit.FacingDir, stunnedY);
    }
}
