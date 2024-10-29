using System.Collections;
using UnityEngine;

public class DarkKnightAggroState : EnemyState
{
    private readonly DarkKnight darkKnight;
    private Player player;
    private float attackCoodown;
    private float slideCooldownTimer;
    private float rollCooldownTimer;
    private float healingCooldownTimer;
    private float evadeCrystalCooldownTimer;
    private readonly float cooldown = 2;
    private int facingDir;
    private bool canMove;

    private const string X_VELOCITY = "xVelocity";

    public DarkKnightAggroState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animName, DarkKnight _darkKnight) : base(_enemy, _stateMachine, _animName)
    {
        darkKnight = _darkKnight;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.Instance.Player;
        player.OnAttack += Player_OnAttack;
        if (player.IsDead)
        {
            darkKnight.StartCoroutine(PrayControllerRoutine());
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
        HealingController();
    }

    public override void Update()
    {
        base.Update();

        slideCooldownTimer -= Time.deltaTime;
        rollCooldownTimer -= Time.deltaTime;
        healingCooldownTimer -= Time.deltaTime;
        evadeCrystalCooldownTimer -= Time.deltaTime;
    }

    /// <summary>
    /// Handles to setup behavior of the character.
    /// </summary>
    private void AggroController()
    {
        canMove = true;

        if (darkKnight.IsPlayerDetected())
        {
            EvadeCrsystalController();

            if (CheckDistance(darkKnight.AttackDistance))
            {
                AttackController();
            }
            else
            {
                SlideController();
            }
        }
        else
        {
            float distance = Mathf.Abs(darkKnight.transform.position.x - player.transform.position.x);
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

        facingDir = player.transform.position.x < darkKnight.transform.position.x ? -1 : 1;
        darkKnight.SetVelocityWithFlip(darkKnight.AggroSpeed * facingDir, rb.velocity.y);
    }

    /// <summary>
    /// Handles to perfomr attack of the character.
    /// </summary>
    private void AttackController()
    {
        if (CanAttack() && !player.IsDead)
        {
            if (Utils.RandomChance(darkKnight.JumpChance))
            {
                stateMachine.Changestate(darkKnight.JumpState);
            }
            else
            {
                stateMachine.Changestate(darkKnight.AttackState);
            }
        }

        canMove = false;
    }

    /// <summary>
    /// Handles to check character can perform attack.
    /// </summary>
    /// <returns></returns>
    private bool CanAttack()
    {
        if (Time.time > lastTimeAttacked + attackCoodown)
        {
            lastTimeAttacked = Time.time;
            attackCoodown = Random.Range(darkKnight.MinAttackCooldown, darkKnight.MaxAttackCooldown);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Handles to perform evade crystal of the character.
    /// </summary>
    private void EvadeCrsystalController()
    {
        if (!darkKnight.IsPlayerDetected()) return;

        if (player.Crystal != null && Vector2.Distance(darkKnight.transform.position, player.Crystal.transform.position) < darkKnight.AttackDistance && evadeCrystalCooldownTimer < 0)
        {
            if (Utils.RandomChance(darkKnight.EvadeCrystalChance))
            {
                stateMachine.Changestate(darkKnight.RollState);
            }

            evadeCrystalCooldownTimer = cooldown;
        }
    }

    /// <summary>
    /// Handles to perform slide of the character.
    /// </summary>
    private void SlideController()
    {
        if (player.FacingDir == darkKnight.FacingDir) return;

        if (CheckDistance(darkKnight.DistanceToSlide) && slideCooldownTimer < 0)
        {
            if (Utils.RandomChance(darkKnight.SlideChance))
            {
                stateMachine.Changestate(darkKnight.SlideState);
            }

            slideCooldownTimer = darkKnight.SlideCooldown;
        }
    }

    /// <summary>
    /// Handles to perform roll of the character.
    /// </summary>
    private void RollController()
    {
        if (player.FacingDir == darkKnight.FacingDir) return;

        if (CheckDistance(darkKnight.AttackDistance) && rollCooldownTimer < 0)
        {
            if (Utils.RandomChance(darkKnight.RollChance))
            {
                stateMachine.Changestate(darkKnight.RollState);
            }

            rollCooldownTimer = cooldown;
        }
    }

    /// <summary>
    /// Handles to perform healing of the character.
    /// </summary>
    private void HealingController()
    {
        if (!CheckDistance(darkKnight.AttackDistance) && healingCooldownTimer < 0)
        {
            if (darkKnight.TryGetComponent(out EnemyStats enemyStats))
            {
                if (enemyStats.CurrentHealth / enemyStats.maxHealth.GetValueWithModify() < darkKnight.ThresholdOfHealing)
                {
                    healingCooldownTimer = darkKnight.HealingCooldown;
                    stateMachine.Changestate(darkKnight.HealthState);
                }
            }
        }
    }

    /// <summary>
    /// Handles to chanage pray state when player died.
    /// </summary>
    /// <returns></returns>
    private IEnumerator PrayControllerRoutine()
    {
        yield return new WaitForSeconds(.2f);

        stateMachine.Changestate(darkKnight.KneelState);
    }

    /// <summary>
    /// Handles to check distance between player and the character.
    /// </summary>
    /// <param name="_distance"></param>
    /// <returns></returns>
    private bool CheckDistance(float _distance)
    {
        return Vector2.Distance(player.transform.position, darkKnight.transform.position) < _distance;
    }

    private void Player_OnAttack(object sender, System.EventArgs e)
    {
        RollController();
    }
}
