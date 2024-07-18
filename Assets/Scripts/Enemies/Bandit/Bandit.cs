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
    private BanditDeathState deathState;
    private BanditStunnedState stunnedState;

    private const string IDLE = "Idle";
    private const string MOVE = "Move";
    private const string AGGRO = "Aggro";
    private const string ATTACK = "Attack";
    private const string DIE = "Die";
    private const string STUNNED = "Stunned";
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new BanditIdleState(this, stateMachine, IDLE, this);
        moveState = new BanditMoveState(this, stateMachine, MOVE, this);
        aggroState = new BanditAggroState(this, stateMachine, AGGRO, this);
        attackState = new BanditAttackState(this, stateMachine, ATTACK, this);
        deathState = new BanditDeathState(this, stateMachine, DIE, this);
        stunnedState = new BanditStunnedState(this, stateMachine, STUNNED, this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.InitializedState(idleState);
        SetDefaultFacingDir(defaultFacingDir);
    }

    /// <summary>
    /// Handles to change state to DeathState.
    /// </summary>
    public override void EnemyDead()
    {
        stateMachine.Changestate(deathState);
        isDead = true;
    }

    /// <summary>
    /// Handles to determine of the character can be stunned.
    /// </summary>
    /// <returns>True if can be stunned. False if not.</returns>
    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.Changestate(StunnedState);
            return true;
        }

        return false;
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

    public BanditDeathState DeathState
    {
        get { return deathState; }
    }

    public BanditStunnedState StunnedState
    {
        get { return stunnedState; }
    }
    #endregion
}
