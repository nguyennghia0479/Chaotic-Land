using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    #region Variables
    [Header("Move info")]
    [SerializeField] private int moveSpeed = 8;
    [SerializeField] private int jumpForce = 12;

    [Header("Dash info")]
    [SerializeField] private int dashSpeed = 25;
    [SerializeField] private float dashDuration = .2f;
    [SerializeField] private float dashCooldown = 2;

    [Header("Attack info")]
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private Vector2[] attackMovements;

    private PlayerController controller;
    private float dashTimer;

    private PlayerStateMachine stateMachine;
    private PlayerIdleState idleState;
    private PlayerMoveState moveState;
    private PlayerJumpState jumpState;
    private PlayerAirState airState;
    private PlayerDashState dashState;
    private PlayerWallSlideState wallSlideState;
    private PlayerWallGrabState wallGrabState;
    private PlayerWallJumpState wallJumpState;
    private PlayerAttackState attackState;
    private PlayerBlockState blockState;
    private PlayerDeathState deathState;

    private const string IDLE = "Idle";
    private const string MOVE = "Move";
    private const string JUMP = "Jump";
    private const string DASH = "Dash";
    private const string WALL_SLIDE = "WallSlide";
    private const string WALL_GRAB = "WallGrab";
    private const string ATTACK = "Attack";
    private const string BLOCK = "Block";
    private const string DIE = "Die";
    #endregion

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new();
        idleState = new PlayerIdleState(this, stateMachine, IDLE);
        moveState = new PlayerMoveState(this, stateMachine, MOVE);
        jumpState = new PlayerJumpState(this, stateMachine, JUMP);
        airState = new PlayerAirState(this, stateMachine, JUMP);
        dashState = new PlayerDashState(this, stateMachine, DASH);
        wallSlideState = new PlayerWallSlideState(this, stateMachine, WALL_SLIDE);
        wallGrabState = new PlayerWallGrabState(this, stateMachine, WALL_GRAB);
        wallJumpState = new PlayerWallJumpState(this, stateMachine, JUMP);
        attackState = new PlayerAttackState(this, stateMachine, ATTACK);
        blockState = new PlayerBlockState(this, stateMachine, BLOCK);
        deathState = new PlayerDeathState(this, stateMachine, DIE);

        controller = GetComponent<PlayerController>();
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.IntializedState(idleState);

        if (controller != null)
        {
            controller.OnDashAction += PlayerController_OnDashAction;
            controller.OnBlockActionStart += PlayerController_OnBlockActionStart;
            controller.OnBlockActionEnd += PlayerController_OnBlockActionEnd;
        }
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.CurrentState.Update();

        dashTimer -= Time.deltaTime;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        stateMachine.CurrentState.FixedUpdate();
    }

    protected void OnDestroy()
    {
        if (controller != null)
        {
            controller.OnDashAction -= PlayerController_OnDashAction;
            controller.OnBlockActionStart -= PlayerController_OnBlockActionStart;
            controller.OnBlockActionEnd -= PlayerController_OnBlockActionEnd;
        }
    }

    #region public methods
    /// <summary>
    /// Handles jumping and flipping of the character.
    /// </summary>
    /// <param name="_xVelocity">The horizontal velecity to move the character.</param>
    public void AddForce(float _xVelocity)
    {
        rb.AddForce(new Vector2(_xVelocity, jumpForce), ForceMode2D.Impulse);
        FlipController(_xVelocity);
    }

    /// <summary>
    /// Handles to trigger animation of the character
    /// </summary>
    public void AnimationTrigger() => stateMachine.CurrentState.AnimationTrigger();

    /// <summary>
    /// Handles to chanage state to DeathState.
    /// </summary>
    public void PlayerDeath()
    {
        stateMachine.ChangeState(DeathState);
        isDead = true;
    }
    #endregion

    #region private methods
    /// <summary>
    /// Handles to perform dash of the character.
    /// </summary>
    private void PlayerController_OnDashAction(object sender, System.EventArgs e)
    {
        if (IsWallDetected()) return;

        if (dashTimer < 0)
        {
            stateMachine.ChangeState(dashState);
            dashTimer = dashCooldown;
        }
        else
        {
            Debug.Log("Dash is cooldown");
        }
    }

    /// <summary>
    /// Handles to perform block of the charaters.
    /// </summary>
    private void PlayerController_OnBlockActionStart(object sender, EventArgs e)
    {
        isBlocking = true;
        stateMachine.ChangeState(blockState);
    }

    /// <summary>
    /// Handles to cancel block of the characters.
    /// </summary>
    private void PlayerController_OnBlockActionEnd(object sender, EventArgs e)
    {
        isBlocking = false;
        stateMachine.ChangeState(idleState);
    }
    #endregion

    #region Getter
    public int MoveSpeed
    {
        get { return moveSpeed; }
    }

    public int JumpForce
    {
        get { return jumpForce; }
    }

    public int DashSpeed
    {
        get { return dashSpeed; }
    }

    public float DashDuration
    {
        get { return dashDuration; }
    }

    public float AttackCooldown
    {
        get { return attackCooldown; }
    }

    public Vector2[] AttackMovements
    {
        get { return attackMovements; }
    }

    public PlayerController Controller
    {
        get { return controller; }
    }

    public PlayerIdleState IdleState
    {
        get { return idleState; }
    }

    public PlayerMoveState MoveState
    {
        get { return moveState; }
    }

    public PlayerJumpState JumpState
    {
        get { return jumpState; }
    }

    public PlayerAirState AirState
    {
        get { return airState; }
    }

    public PlayerDashState DashState
    {
        get { return dashState; }
    }

    public PlayerWallSlideState WallSlideState
    {
        get { return wallSlideState; }
    }

    public PlayerWallGrabState WallGrabState
    {
        get { return wallGrabState; }
    }

    public PlayerWallJumpState WallJumpState
    {
        get { return wallJumpState; }
    }

    public PlayerAttackState AttackState
    {
        get { return attackState; }
    }

    public PlayerBlockState BlockState
    {
        get { return blockState; }
    }

    public PlayerDeathState DeathState
    {
        get { return deathState; }
    }
    #endregion
}
