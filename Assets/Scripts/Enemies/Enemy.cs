using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    #region Variables
    [Header("Move info")]
    [SerializeField] protected int moveSpeed = 2;
    [SerializeField] protected float idleTime = 1;

    [Header("Aggro info")]
    [SerializeField] protected int aggroSpeed = 4;
    [SerializeField] protected Transform playerCheck;
    [SerializeField] protected float playerCheckDistance = 10;
    [SerializeField] protected LayerMask playerLayerMask;

    [Header("Attack info")]
    [SerializeField] protected float attackDistance = 2;
    [SerializeField] protected float minAttackCooldown = 1;
    [SerializeField] protected float maxAttackCooldown = 1;
    [SerializeField] private Transform criticalFX;
    [SerializeField] private float criticalChance = 25;

    protected EnemyStateMachine stateMachine;
    protected bool isCriticalAttack;
    protected bool canBeStunned;
    #endregion

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new EnemyStateMachine();
        criticalFX.gameObject.SetActive(false);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.CurrentState.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        stateMachine.CurrentState.FixedUpdate();
    }

    /// <summary>
    /// Handles to detect player.
    /// </summary>
    /// <returns>True if detect player. False if not, beside if wall is detected will return false.</returns>
    public bool IsPlayerDetected()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerCheck.position, Vector2.right * facingDir, playerCheckDistance, playerLayerMask);
        if (IsWallDetected())
        {
            return false;
        }

        return hit.collider != null;
    }

    /// <summary>
    /// Handles to trigger animation of the character
    /// </summary>
    public void AnimationTrigger() => stateMachine.CurrentState.AnimationTrigger();

    public virtual void EnemyDead()
    {
    }

    /// <summary>
    /// Handles to setup of pre attack.
    /// </summary>
    /// <remarks>
    /// If will be peformed critical or normal attack depend on critical chance.
    /// </remarks>
    public void AnimationPrepareAttack()
    {
        if (Random.Range(0, 100) < criticalChance)
        {
            isCriticalAttack = true;
            canBeStunned = false;
        }
        else
        {
            canBeStunned = true;
        }

        if (isCriticalAttack)
        {
            criticalFX.gameObject.SetActive(true);
        } 
    }

    /// <summary>
    /// Handles to setup of finish attack/
    /// </summary>
    public void AnimationAttackFinished()
    {
        criticalFX.gameObject.SetActive(false);
        isCriticalAttack = false;
        canBeStunned = false;
    }

    /// <summary>
    /// Handles to determine if the character can be stunned.
    /// </summary>
    /// <returns>True if can be stunned. False if not.</returns>
    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            AnimationAttackFinished();
            return true;
        }

        return false;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + playerCheckDistance * facingDir, playerCheck.position.y));
    }

    #region Getters
    public int MoveSpeed
    {
        get { return moveSpeed; }
    }

    public float IdleTime
    {
        get { return idleTime; }
    }

    public int AggroSpeed
    {
        get { return aggroSpeed; }
    }

    public float AttackDistance
    {
        get { return attackDistance; }
    }

    public float MinAttackCooldown
    {
        get { return minAttackCooldown; }
    }

    public float MaxAttackCooldown
    {
        get { return maxAttackCooldown; }
    }

    public bool IsCriticalAttack
    {
        get { return isCriticalAttack; }
    }
    #endregion
}
