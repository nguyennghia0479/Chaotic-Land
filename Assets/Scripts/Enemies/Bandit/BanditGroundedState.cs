using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditGroundedState : EnemyState
{
    protected Bandit bandit;
    protected Player player;
    private readonly float aggroDistance = 3;

    public BanditGroundedState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animName, Bandit _bandit) : base(_enemy, _stateMachine, _animName)
    {
        bandit = _bandit;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.Instance.Player;
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

        if (bandit.IsImmobilized) return;
        if (bandit.IsPlayerDetected() || Vector2.Distance(bandit.transform.position, player.transform.position) < aggroDistance)
        {
            stateMachine.Changestate(bandit.AggroState);
        }
    }
}
