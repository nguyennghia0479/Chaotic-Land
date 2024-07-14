using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandit : Enemy
{
    #region variables and states
    private readonly int defaultFacingDir = -1;
    private BanditIdleState idleState;
    private BanditMoveState moveState;
    private BanditAggroState aggroState;
    private BanditAttackState attackState;

    private const string IDLE = "Idle";
    private const string MOVE = "Move";
    private const string AGGRO = "Aggro";
    private const string ATTACK = "Attack";
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new BanditIdleState(this, stateMachine, IDLE, this);
        moveState = new BanditMoveState(this, stateMachine, MOVE, this);
        aggroState = new BanditAggroState(this, stateMachine, AGGRO, this);
        attackState = new BanditAttackState(this, stateMachine, ATTACK, this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.InitializedState(idleState);
        SetDefaultFacingDir(defaultFacingDir);
    }

    #region Getters
    public BanditIdleState IdleState
    {
        get { return idleState; }
    }

    public BanditMoveState MoveState
    {
        get { return moveState; }
    }

    public BanditAggroState AggroState
    {
        get { return aggroState; }
    }

    public BanditAttackState AttackState
    {
        get { return attackState; }
    }
    #endregion
}
