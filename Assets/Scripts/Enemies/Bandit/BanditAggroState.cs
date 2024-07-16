using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditAggroState : EnemyState
{
    private Bandit bandit;
    private Player player;
    private int facingDir;
    private bool canMove = true;
    private float attackCooldown;
    private readonly int aggroTime = 3;
    private readonly int aggroDistance = 5;

    private const string X_VELOCITY = "xVelocity";

    public BanditAggroState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animName, Bandit _bandit) : base(_enemy, _stateMachine, _animName)
    {
        bandit = _bandit;
    }

    public override void Enter()
    {
        base.Enter();

        player = Object.FindObjectOfType<Player>();
    }

    public override void Exit()
    {
        base.Exit();
        rb.gravityScale = 5;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        
        AggroController();

        if (!bandit.IsGroundDetected() && !bandit.IsSlopeDetected())
        {
            canMove = false;
            
        }

        if (bandit.IsSlopeDetected())
        {
            bandit.SetZeroVelocity();
        }

        if (canMove)
        {
            facingDir = (player.transform.position.x < bandit.transform.position.x) ? -1 : 1;
            if (bandit.IsWallDetected())
                bandit.SetZeroVelocity();
            else
                bandit.SetVelocity(bandit.AggroSpeed * facingDir, rb.velocity.y);
        }
    }

    public override void Update()
    {
        base.Update();

        anim.SetFloat(X_VELOCITY, rb.velocity.x);
    }

    /// <summary>
    /// Handles to behavior of enemy.
    /// </summary>
    /// <remarks>
    /// If player is detected and attack player if close to player.<br></br>
    /// If player is not detected but enemy and player has almost same postion then enmy will attack player.<br></br>
    /// If aggro time is over or distance of enemy and player is greater than attack distance will change to Idle state.
    /// </remarks>
    private void AggroController()
    {
        canMove = true;
        if (bandit.IsPlayerDetected())
        {
            stateTimer = aggroTime;
            if (Vector2.Distance(bandit.transform.position, player.transform.position) < bandit.AttackDistance)
            {
                if (CanAttack())
                {
                    stateMachine.Changestate(bandit.AttackState);
                }
                canMove = false;
            }
        }
        else
        {
            float distance = Mathf.Abs(bandit.transform.position.x - player.transform.position.x);
            if (distance < 1)
            {
                stateTimer = aggroTime;
                if (CanAttack())
                {
                    stateMachine.Changestate(bandit.AttackState);
                }
                canMove = false;
            }

            if (stateTimer < 0 || Vector2.Distance(bandit.transform.position, player.transform.position) > aggroDistance)
            {
                stateMachine.Changestate(bandit.IdleState);
            }
        }
    }

    /// <summary>
    /// Handles to determine enemy can attack player.
    /// </summary>
    /// <returns>True if attack cooldown is over. False if not</returns>
    /// <remarks>
    /// Attack cooldown is random by min and max attack cooldown.
    /// </remarks>
    private bool CanAttack()
    {
        if (Time.time > lastTimeAttacked + attackCooldown)
        {
            attackCooldown = Random.Range(bandit.MinAttackCooldown, bandit.MaxAttackCooldown);
            lastTimeAttacked = Time.time;
            return true;
        }

        return false;
    }
}
