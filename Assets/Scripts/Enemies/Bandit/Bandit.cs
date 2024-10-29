using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandit : Enemy
{
    #region variables and states
    protected BanditIdleState idleState;
    protected BanditMoveState moveState;
    protected BanditAggroState aggroState;
    protected BanditAttackState attackState;
    protected BanditDeathState deathState;
    protected BanditStunnedState stunnedState;

    protected const string IDLE = "Idle";
    protected const string MOVE = "Move";
    protected const string AGGRO = "Aggro";
    protected const string ATTACK = "Attack";
    protected const string DIE = "Die";
    protected const string STUNNED = "Stunned";
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
    }

    /// <summary>
    /// Handles to change state to DeathState.
    /// </summary>
    public override void SetupDeath()
    {
        base.SetupDeath();

        if (isImmobilized)
        {
            ResetDefaultSpeed();
        }

        stateMachine.Changestate(deathState);
        Destroy(gameObject, 3);
    }

    public override void InstantDeath()
    {
        if (isImmobilized)
        {
            ResetDefaultSpeed();
        }

        stateMachine.Changestate(deathState);
        Destroy(gameObject, 3);
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
