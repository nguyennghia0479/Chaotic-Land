using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditAggroState : EnemyState
{
    private readonly Bandit bandit;
    private Player player;
    private int facingDir;
    private bool canMove = true;
    private float attackCooldown;
    private readonly int aggroTime = 3;
    private readonly int aggroDistance = 5;
    private float footstepTimer;
    private readonly float footstepTimerMax = .4f;

    private const string X_VELOCITY = "xVelocity";

    public BanditAggroState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animName, Bandit _bandit) : base(_enemy, _stateMachine, _animName)
    {
        bandit = _bandit;
    }

    public override void Enter()
    {
        base.Enter();

        bandit.IsCombat = true;
        player = PlayerManager.Instance.Player;
        if (player != null && player.IsDead)
        {
            stateMachine.Changestate(bandit.MoveState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        bandit.IsCombat = false;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (bandit.IsImmobilized) return;

        anim.SetFloat(X_VELOCITY, rb.velocity.x);
        AggroController();
        MoveController();
    }

    public override void Update()
    {
        base.Update();
    }

    /// <summary>
    /// Handles to setup of behavior of enemy.
    /// </summary>
    /// <remarks>
    /// If player is detected and attack player if close to player.<br></br>
    /// If player is not detected but in aggro distance then enemy will flip.<br></br>
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
                AttackController();
            }
        }
        else
        {
            if (Vector2.Distance(bandit.transform.position, player.transform.position) < aggroDistance)
            {
                bandit.Flip();
            }

            float distance = Mathf.Abs(bandit.transform.position.x - player.transform.position.x);
            if (distance < 1)
            {
                stateTimer = aggroTime;
                AttackController();
            }

            if (stateTimer < 0 || Vector2.Distance(bandit.transform.position, player.transform.position) > aggroDistance)
            {
                stateMachine.Changestate(bandit.IdleState);
            }
        }
    }

    /// <summary>
    /// Handles to perform attack.
    /// </summary>
    private void AttackController()
    {
        if (CanAttack())
        {
            stateMachine.Changestate(bandit.AttackState);
        }
        canMove = false;
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

    /// <summary>
    /// Handles to move of the character.
    /// </summary>
    private void MoveController()
    {
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
            {
                bandit.SetZeroVelocity();
            }
            else
            {
                float speed = bandit.IsSlopeDetected() ? bandit.AggroSpeed * 1.5f : bandit.AggroSpeed;
                bandit.SetVelocityWithFlip(bandit.AggroSpeed * facingDir, rb.velocity.y);
                PlayFootstepsSound();
            }

        }
    }

    private void PlayFootstepsSound()
    {
        footstepTimer -= Time.deltaTime;
        if (footstepTimer < 0)
        {
            footstepTimer = footstepTimerMax;
            soundManager.PlayFootstepSound(enemy.transform.position);
        }
    }
}
