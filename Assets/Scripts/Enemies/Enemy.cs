using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DefaultFacingDir
{
    Left = -1, Right = 1
}

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent (typeof(CapsuleCollider2D))]
[RequireComponent(typeof(KnockBack))]
[RequireComponent(typeof(EntityFX))]
[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(ItemDrop))]
public class Enemy : Entity
{
    #region Variables
    [Header("Move info")]
    [SerializeField] protected DefaultFacingDir defaultFacingDir;
    [SerializeField] protected int moveSpeed = 2;
    [SerializeField] protected float idleTime = 1;

    [Header("Aggro info")]
    [HideInInspector] protected bool isCombat;
    [SerializeField] protected int aggroSpeed = 4;
    [SerializeField] protected Transform playerCheck;
    [SerializeField] protected float playerCheckDistance = 10;
    [SerializeField] protected LayerMask playerLayerMask;

    [Header("Attack info")]
    [SerializeField] protected float attackDistance = 2;
    [SerializeField] protected float minAttackCooldown = 1;
    [SerializeField] protected float maxAttackCooldown = 1;
    [SerializeField] protected Transform criticalFX;

    [Space]
    [SerializeField] protected bool isBoss;
    [SerializeField] protected string bossName;
    [SerializeField] protected BossInfoUI bossInfoUI;
    [SerializeField] protected Arena arena;

    protected EnemyStateMachine stateMachine;
    protected bool isCriticalAttack;
    protected bool canBeStunned;
    protected int defaultMoveSpeed;
    protected bool isAddExp;

    public event EventHandler OnDie;
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

        defaultMoveSpeed = moveSpeed;
        SetDefaultFacingDir((int)defaultFacingDir);
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
    /// Handles to setup death of enemy.
    /// </summary>
    public override void SetupDeath()
    {
        base.SetupDeath();

        if (!isAddExp)
        {
            isAddExp = true;
            PlayerManager.Instance.IncreaseExp(Mathf.RoundToInt((Stats as EnemyStats).Exp.GetValueWithModify()));
        }

        if (isBoss)
        {
            Checkpoint checkpoint = FindObjectOfType<Checkpoint>();
            if (checkpoint != null)
            {
                checkpoint.Activate();
            }

            OnDie?.Invoke(this, EventArgs.Empty);
            arena.BossFightEnd();
        }
    }

    public virtual void InstantDeath()
    {
    }

    /// <summary>
    /// Handles to detect player.
    /// </summary>
    /// <returns>True if detect player. False if not, beside if wall is detected will return false.</returns>
    public bool IsPlayerDetected()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerCheck.position, Vector2.right * facingDir, playerCheckDistance, playerLayerMask);
        if (IsWallDetected() || !IsGroundDetected())
        {
            return false;
        }

        return hit.collider != null;
    }

    #region Enemy animation
    /// <summary>
    /// Handles to trigger animation of the character
    /// </summary>
    public void AnimationTrigger() => stateMachine.CurrentState.AnimationTrigger();

    /// <summary>
    /// Handles to setup of pre attack.
    /// </summary>
    /// <remarks>
    /// If will be peformed critical or normal attack depend on critical chance.
    /// </remarks>
    public void AnimationPrepareAttack()
    {
        float totalCritChance = Stats.critChance.GetValueWithModify();
        if (Utils.RandomChance(totalCritChance))
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
    /// Handles to setup of finish attack.
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
    #endregion

    #region Enemy effect
    /// <summary>
    /// Handles to freezing enemy in duration.
    /// </summary>
    /// <param name="_duration"></param>
    public void FreezingEffect(float _duration)
    {
        if (isDead) return;

        ImmobilizedEffect(_duration);
        FX.PlayChilledFX(_duration);
    }

    /// <summary>
    /// Handles to make enemy immoblized in duration.
    /// </summary>
    /// <param name="_duration"></param>
    public void ImmobilizedEffect(float _duration)
    {
        if (isDead) return;

        SlowEntityEffect(0, _duration);
    }

    /// <summary>
    /// Handles to make character speed slowly.
    /// </summary>
    /// <param name="_slowAffect">Value to slow speed</param>
    /// <param name="_duration">Time of slow effect</param>
    public override void SlowEntityEffect(float _slowAffect, float _duration)
    {
        base.SlowEntityEffect(_slowAffect, _duration);

        if (_slowAffect == 0)
        {
            isImmobilized = true;
        }

        moveSpeed = Mathf.RoundToInt(moveSpeed * _slowAffect);
        Invoke(nameof(ResetDefaultSpeed), _duration);
    }

    /// <summary>
    /// Handles to reset default speed.
    /// </summary>
    protected override void ResetDefaultSpeed()
    {
        base.ResetDefaultSpeed();

        isImmobilized = false;
        moveSpeed = defaultMoveSpeed;
    }
    #endregion

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

    public bool IsCombat
    {
        get { return isCombat; }
        set { isCombat = value; }
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

    public bool IsBoss
    {
        get { return isBoss; }
    }

    public string BossName
    {
        get { return bossName; }
    }

    public BossInfoUI BossInfoUI
    {
        get { return bossInfoUI; }
    }
    #endregion
}
