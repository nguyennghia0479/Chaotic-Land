using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallKnightAggroState : EnemyState
{
    private readonly FallKnight fallKnight;
    private Player player;
    private float evadeCrystalCooldownTimer;
    private float blockCooldownTimer;
    private float attackCooldown;
    private readonly float cooldown = 2;
    private bool canMove;
    private int facingDir;

    private const string X_VELOCITY = "xVelocity";

    public FallKnightAggroState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animName, FallKnight _fallKnight) : base(_enemy, _stateMachine, _animName)
    {
        fallKnight = _fallKnight;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.Instance.Player;
        player.OnAttack += Player_OnAttack;

        if (player.IsDead)
        {
            stateMachine.Changestate(fallKnight.IdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        player.OnAttack -= Player_OnAttack;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        anim.SetFloat(X_VELOCITY, rb.velocity.x);
        AggroController();
        MoveController();
    }

    public override void Update()
    {
        base.Update();

        evadeCrystalCooldownTimer -= Time.deltaTime;
        blockCooldownTimer -= Time.deltaTime;
    }

    /// <summary>
    /// Handles to perform behavior of the character.
    /// </summary>
    private void AggroController()
    {
        canMove = true;

        if (fallKnight.IsPlayerDetected())
        {
            EvadeCrystalController();

            if (CheckDistance(fallKnight.AttackDistance))
            {
                AttackController();
            }
        }
        else
        {
            float distance = Mathf.Abs(fallKnight.transform.position.x - player.transform.position.x);
            if (distance < 1)
            {
                AttackController();
            }
        }
    }

    /// <summary>
    /// Handles to perform move of the character.
    /// </summary>
    private void MoveController()
    {
        if (!canMove) return;

        facingDir = player.transform.position.x < fallKnight.transform.position.x ? -1 : 1;
        fallKnight.SetVelocityWithFlip(fallKnight.AggroSpeed * facingDir, rb.velocity.y);
    }

    /// <summary>
    /// Handles to perform attack of the character.
    /// </summary>
    private void AttackController()
    {
        if (CanAttack() && !player.IsDead)
        {
            stateMachine.Changestate(fallKnight.AttackState);
        }

        canMove = false;
    }

    /// <summary>
    /// Handles to check character can perform attack.
    /// </summary>
    /// <returns></returns>
    private bool CanAttack()
    {
        if (Time.time > lastTimeAttacked + attackCooldown)
        {
            lastTimeAttacked = Time.time;
            attackCooldown = Random.Range(fallKnight.MinAttackCooldown, fallKnight.MaxAttackCooldown);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Handles to perform evade crystal of the character.
    /// </summary>
    private void EvadeCrystalController()
    {
        if (!fallKnight.IsPlayerDetected()) return;

        if (player.Crystal != null && Vector2.Distance(fallKnight.transform.position, player.Crystal.transform.position) < fallKnight.AttackDistance && evadeCrystalCooldownTimer < 0)
        {
            if (Utils.RandomChance(fallKnight.JumpChance))
            {
                stateMachine.Changestate(fallKnight.JumpState);
            }

            evadeCrystalCooldownTimer = cooldown;
        }
    }

    /// <summary>
    /// Handles to perform block of the character.
    /// </summary>
    private void BlockController()
    {
        if (player.FacingDir == fallKnight.FacingDir) return;

        if (CheckDistance(fallKnight.AttackDistance) && blockCooldownTimer < 0)
        {
            if (Utils.RandomChance(fallKnight.BlockChance))
            {
                stateMachine.Changestate(fallKnight.BlockState);
            }

            blockCooldownTimer = cooldown;
        }
    }

    private void Player_OnAttack(object sender, System.EventArgs e)
    {
        BlockController();
    }

    /// <summary>
    /// Handles to check distance between player and the character.
    /// </summary>
    /// <param name="_distance"></param>
    /// <returns></returns>
    private bool CheckDistance(float _distance)
    {
        return Vector2.Distance(player.transform.position, fallKnight.transform.position) < _distance;
    }
}
