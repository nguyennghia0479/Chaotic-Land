using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallKnight : Enemy
{
    #region Variables
    [Header("Fall Knight info")]
    [SerializeField] private Vector2 jumpVelocity;
    [SerializeField] private float jumpChance = 70;
    [SerializeField] private float blockChance = 30;

    private FallKnightIdleState idleState;
    private FallKnightAggroState aggroState;
    private FallKnightJumpState jumpState;
    private FallKnightAirState airState;
    private FallKnightDeathState deathState;
    private FallKnightBlockState blockState;
    private FallKnightAttackState attackState;

    private const string IDLE = "Idle";
    private const string AGGRO = "Aggro";
    private const string JUMP = "Jump";
    private const string DIE = "Die";
    private const string BLOCK = "Block";
    private const string ATTACK = "Attack";
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new FallKnightIdleState(this, stateMachine, IDLE, this);
        aggroState = new FallKnightAggroState(this, stateMachine, AGGRO, this);
        jumpState = new FallKnightJumpState(this, stateMachine, JUMP, this);
        airState = new FallKnightAirState(this, stateMachine, JUMP, this);
        deathState = new FallKnightDeathState(this, stateMachine, DIE, this);
        blockState = new FallKnightBlockState(this, stateMachine, BLOCK, this);
        attackState = new FallKnightAttackState(this, stateMachine, ATTACK, this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.InitializedState(idleState);
    }

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

    /// <summary>
    /// Handles jumping and flipping of the character.
    /// </summary>
    /// <param name="_xVelocity">The horizontal velecity to move the character.</param>
    public void AddForce(float _facingDir)
    {
        float xVelocity = jumpVelocity.x * _facingDir;
        rb.AddForce(new Vector2(xVelocity, jumpVelocity.y), ForceMode2D.Impulse);
        FlipController(xVelocity);
    }

    #region Getter
    public float JumpChance
    {
        get { return jumpChance; }
    }

    public float BlockChance
    {
        get { return blockChance; }
    }

    public Vector2 JumpVelocity
    {
        get { return jumpVelocity; }
    }

    public FallKnightIdleState IdleState
    {
        get { return idleState; }
    }

    public FallKnightAggroState AggroState
    {
        get { return aggroState; }
    }

    public FallKnightJumpState JumpState
    {
        get { return jumpState; }
    }

    public FallKnightAirState AirState
    {
        get { return airState; }
    }

    public FallKnightDeathState DeathState
    {
        get { return deathState; }
    }

    public FallKnightBlockState BlockState
    {
        get { return blockState; }
    }

    public FallKnightAttackState AttackState
    {
        get { return attackState; }
    }
    #endregion
}
