using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Variables
    [Header("Collision info")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance = .5f;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance = .5f;
    [SerializeField] protected LayerMask groundLayerMask;
    [SerializeField] protected LayerMask slopeLayerMask;
    [SerializeField] protected Transform attackCheck;
    [SerializeField] protected float attackRadius;

    [Header("Stunned info")]
    [SerializeField] protected Vector2 stunnedPower;
    [SerializeField] protected float stunnedOffset;
    [SerializeField] protected float stunnedDuration = 2;

    protected Rigidbody2D rb;
    protected Animator animator;
    protected KnockBack knockBack;
    protected EntityFX fx;
    protected EntityStats stats;
    protected int facingDir = 1;
    protected bool isFacingRight = true;
    protected bool isBlocking;
    protected bool isDead;
    #endregion

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    protected virtual void Start()
    {
        knockBack = GetComponent<KnockBack>();
        fx = GetComponent<EntityFX>();
        stats = GetComponent<EntityStats>();
    }

    protected virtual void Update()
    {

    }

    protected virtual void FixedUpdate()
    {

    }

    #region public methods
    /// <summary>
    /// Handles movement of the character.
    /// </summary>
    /// <param name="_xVelocity">The horizontal velocity to move the character.</param>
    /// <param name="_yVelocity">The vertical velocity to move the character.</param>
    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if (knockBack != null && knockBack.IsKnockBack) return;

        rb.velocity = new Vector2(_xVelocity, _yVelocity);
    }

    /// <summary>
    /// Handles movement and flipping of the character.
    /// </summary>
    /// <param name="_xVelocity">The horizontal velocity to move the character.</param>
    /// <param name="_yVelocity">The vertical velocity to move the character.</param>
    public void SetVelocityWithFlip(float _xVelocity, float _yVelocity)
    {
        SetVelocity(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }

    /// <summary>
    /// Handles to stop the character move with Vector2.zero.
    /// </summary>
    public void SetZeroVelocity()
    {
        if (knockBack != null && knockBack.IsKnockBack) return;

        rb.velocity = Vector2.zero;
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
    /// Handles to detect slope ground.
    /// </summary>
    /// <returns></returns>
    public virtual bool IsSlopeDetected()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, slopeLayerMask);

        return hit.collider != null;
    }

    /// <summary>
    /// Set default facing direction of the character.
    /// </summary>
    /// <param name="_facingDir">Value to determine the facing direction. 1 is facing right and -1 is opposite.</param>
    public void SetDefaultFacingDir(int _facingDir)
    {
        facingDir = _facingDir;
        if (facingDir == -1)
        {
            isFacingRight = false;
        }
    }

    /// <summary>
    /// Handles to update facing direction and rotation of the character.
    /// </summary>
    public void Flip()
    {
        facingDir *= -1;
        isFacingRight = !isFacingRight;
        transform.Rotate(0, -180, 0);
    }

    /// <summary>
    /// Handles to knock back of the target.
    /// </summary>
    /// <param name="_damageDealer">Value to determine who's made damage.</param>
    /// <param name="isCriticalAttack">Value to determine if is critical attack or not.</param>
    public void SetupKnockBack(Transform _damageDealer, bool isCriticalAttack)
    {
        if (knockBack != null)
        {
            knockBack.SetupKnockBack(this, _damageDealer, isCriticalAttack);
        }
    }

    /// <summary>
    /// Handles to make character speed slowly.
    /// </summary>
    /// <param name="_slowPercentage">Value to slow speed</param>
    /// <param name="_duration">Time of slow effect</param>
    public virtual void SlowEntityEffect(float _slowPercentage, float _duration)
    {
        animator.speed = 1 - (animator.speed * _slowPercentage);
    }

    /// <summary>
    /// Handles to make character die.
    /// </summary>
    public virtual void SetupDeath()
    {

    }
    #endregion

    #region protected methods
    /// <summary>
    /// Handles to flipping of the characters.
    /// </summary>
    /// <param name="_xVelocity">The horizontal velocity to move the character.</param>
    protected void FlipController(float _xVelocity)
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
    /// Handles to reset default speed.
    /// </summary>
    protected virtual void ResetDefaultSpeed()
    {
        animator.speed = 1;
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackRadius);
    }
    #endregion

    #region Getters
    public Transform AttackCheck
    {
        get { return attackCheck; }
    }

    public float AttackRadius
    {
        get { return attackRadius; }
    }

    public Vector2 StunnedPower
    {
        get { return stunnedPower; }
    }

    public float StunnedOffset
    {
        get { return stunnedOffset; }
    }

    public float StunnedDuration
    {
        get { return stunnedDuration; }
    }

    public int FacingDir
    {
        get { return facingDir; }
    }

    public bool IsFacingRight
    {
        get { return isFacingRight; }
    }

    public Rigidbody2D Rb
    {
        get { return rb; }
    }

    public Animator Animator
    {
        get { return animator; }
    }

    public EntityFX FX
    {
        get { return fx; }
    }

    public EntityStats Stats
    {
        get { return stats; }
    }

    public bool IsBlocking
    {
        get { return isBlocking; }
    }

    public bool IsDead
    {
        get { return isDead; }
    }
    #endregion
}
