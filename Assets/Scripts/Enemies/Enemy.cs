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

    protected EnemyStateMachine stateMachine;
    #endregion

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new EnemyStateMachine();
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

    public void AnimationTrigger() => stateMachine.CurrentState.AnimationTrigger();

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
    #endregion
}
