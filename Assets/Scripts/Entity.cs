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

    protected Rigidbody2D rb;
    protected Animator animator;
    protected int facingDir = 1;
    protected bool isFacingRight = true;
    #endregion

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }

    protected virtual void FixedUpdate()
    {

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

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));
    }
    #endregion

    #region Getters
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
    #endregion
}
