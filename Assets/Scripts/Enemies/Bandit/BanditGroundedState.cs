using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditGroundedState : EnemyState
{
    protected Bandit bandit;
    protected Player player;

    public BanditGroundedState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animName, Bandit _bandit) : base(_enemy, _stateMachine, _animName)
    {
        bandit = _bandit;
    }

    public override void Enter()
    {
        base.Enter();

        player = Object.FindObjectOfType<Player>();
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

        if (bandit.IsPlayerDetected() || Vector2.Distance(bandit.transform.position, player.transform.position) < 3)
        {
            stateMachine.Changestate(bandit.AggroState);
        }
    }
}
