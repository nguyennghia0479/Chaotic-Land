using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Variables
    [Header("Move info")]
    [SerializeField] private int moveSpeed = 8;
    [SerializeField] private int jumpForce = 12;

    [Header("Collision info")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance = .5f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance = .5f;
    [SerializeField] private LayerMask groundLayerMask;

    [Header("Dash info")]
    [SerializeField] private int dashSpeed = 25;
    [SerializeField] private float dashDuration = .2f;
    [SerializeField] private float dashCooldown = 2;

    [Header("Attack info")]
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private Vector2[] attackMovements;

    private Rigidbody2D rb;
    private Animator animator;
    private PlayerController controller;
    private int facingDir = 1;
    private bool isFacingRight = true;
    private float dashTimer;

    private const string IDLE = "Idle";
    private const string MOVE = "Move";
    private const string JUMP = "Jump";
    private const string DASH = "Dash";
    private const string WALL_SLIDE = "WallSlide";
    private const string WALL_GRAB = "WallGrab";
    private const string ATTACK = "Attack";
    #endregion

    #region States
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
    #endregion

    private void Awake()
    {
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

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<PlayerController>();
    }

    private void Start()
    {
        stateMachine.IntializedState(idleState);
        controller.OnDashAction += PlayerController_OnDashAction;
    }

    private void Update()
    {
        stateMachine.CurrentState.Update();

        dashTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        stateMachine.CurrentState.FixedUpdate();
    }

    #region public methods
    /// <summary>
    /// Handles movement and flipping of the character.
    /// </summary>
    /// <param name="_xVelocity">The horizontal velocity to move the character.</param>
    /// <param name="_yVelocity">The vertical velocity to move the character.</param>
    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }

    /// <summary>
    /// Handles to stop the character move with Vector2.zero.
    /// </summary>
    public void SetZeroVelocity() => rb.velocity = Vector2.zero;

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
    /// Handles to detect ground.
    /// </summary>
    /// <returns>True if detect ground, false if not.</returns>
    public bool IsGroundDetected()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayerMask);

        return hit.collider != null;
    }

    /// <summary>
    /// Handles to detect wall.
    /// </summary>
    /// <returns>True if detect wall, false if not.</returns>
    public bool IsWallDetected()
    {
        RaycastHit2D hit = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, groundLayerMask);

        return hit.collider != null;
    }

    /// <summary>
    /// Handle to trigger animation of the character
    /// </summary>
    public void AnimationTrigger() => stateMachine.CurrentState.AnimationTrigger();
    #endregion

    #region private methods
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));
    }

    /// <summary>
    /// Handles to flipping of the characters.
    /// </summary>
    /// <param name="_xVelocity">The horizontal velocity to move the character.</param>
    private void FlipController(float _xVelocity)
    {
        if (_xVelocity > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (_xVelocity < 0 && isFacingRight)
        {
            Flip();
        }
    }

    /// <summary>
    /// Handles to update facing direction and rotation of the character.
    /// </summary>
    private void Flip()
    {
        facingDir *= -1;
        isFacingRight = !isFacingRight;
        if (isFacingRight)
        {
            transform.rotation = Quaternion.identity;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        }
    }

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

    public int FacingDir
    {
        get { return facingDir; }
    }

    public Rigidbody2D Rb
    {
        get { return rb; }
    }

    public Animator Animator
    {
        get { return animator; }
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
    #endregion
}
