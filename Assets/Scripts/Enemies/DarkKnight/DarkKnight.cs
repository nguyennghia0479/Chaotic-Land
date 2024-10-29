using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkKnight : Enemy
{
    #region Variables
    [Header("Dark Knight info")]
    [SerializeField] private float jumpForce = 12;
    [SerializeField] private float evadeCrystalChance = 70;
    [SerializeField] private float jumpChance = 30;
    [SerializeField] private float slideChance = 30;
    [SerializeField] private float distanceToSlide = 7;
    [SerializeField] private float slideCooldown = 10;
    [SerializeField] private float rollChance = 30;
    [SerializeField] private float healingCooldown = 50;
    [SerializeField] private float percentHealing = .2f;
    [SerializeField] private float thresholdOfHealing = .5f;

    private DarkKnightPrayState prayState;
    private DarkKnightStandupState standupState;
    private DarkKnightAggroState aggroState;
    private DarkKnightSlideState slideState;
    private DarkKnightRollState rollState;
    private DarkKnightJumpState jumpState;
    private DarkKnightJumpAttackState jumpAttackState;
    private DarkKnightHealthState healthState;
    private DarkKnightDeathState deathState;
    private DarkKnightAttackState attackState;
    private DarkKnightKneelState kneelState;

    private const string PRAY = "Pray";
    private const string STANDUP = "Standup";
    private const string AGGRO = "Aggro";
    private const string SLIDE = "Slide";
    private const string ROLL = "Roll";
    private const string JUMP = "Jump";
    private const string HEALTH = "Health";
    private const string DIE = "Die";
    private const string ATTACK = "Attack";
    private const string KNEEL = "Kneel";
    #endregion

    protected override void Awake()
    {
        base.Awake();

        prayState = new DarkKnightPrayState(this, stateMachine, PRAY, this);
        standupState = new DarkKnightStandupState(this, stateMachine, STANDUP, this);
        aggroState = new DarkKnightAggroState(this, stateMachine, AGGRO, this);
        slideState = new DarkKnightSlideState(this, stateMachine, SLIDE, this);
        rollState = new DarkKnightRollState(this, stateMachine, ROLL, this);
        jumpState = new DarkKnightJumpState(this, stateMachine, JUMP, this);
        jumpAttackState = new DarkKnightJumpAttackState(this, stateMachine, JUMP, this);
        healthState = new DarkKnightHealthState(this, stateMachine, HEALTH, this);
        deathState = new DarkKnightDeathState(this, stateMachine, DIE, this);
        attackState = new DarkKnightAttackState(this, stateMachine, ATTACK, this);
        kneelState = new DarkKnightKneelState(this, stateMachine, KNEEL, this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.InitializedState(prayState);
    }

    public override void SetupDeath()
    {
        base.SetupDeath();

        if (isImmobilized)
        {
            ResetDefaultSpeed();
        }

        stateMachine.Changestate(deathState);
        Destroy(gameObject, 3);
    }

    /// <summary>
    /// Handles jumping and flipping of the character.
    /// </summary>
    /// <param name="_xVelocity">The horizontal velecity to move the character.</param>
    public void AddForce(float _xVelocity)
    {
        rb.AddForce(new Vector2(_xVelocity, jumpForce), ForceMode2D.Impulse);
        FlipController(_xVelocity);
    }

    #region Getter
    public float EvadeCrystalChance
    {
        get { return evadeCrystalChance; }
    }

    public float JumpChance
    {
        get { return jumpChance; }
    }

    public float SlideChance
    {
        get { return slideChance; }
    }

    public float DistanceToSlide
    {
        get { return distanceToSlide; }
    }

    public float SlideCooldown
    {
        get { return slideCooldown; }
    }

    public float RollChance
    {
        get { return rollChance; }
    }

    public float HealingCooldown
    {
        get { return healingCooldown; }
    }

    public float PercentHealing
    {
        get { return percentHealing; }
    }

    public float ThresholdOfHealing
    {
        get { return thresholdOfHealing; }
    }

    public DarkKnightPrayState PrayState
    {
        get { return prayState; }
    }

    public DarkKnightStandupState StandupState
    {
        get { return standupState; }
    }

    public DarkKnightAggroState AggroState
    {
        get { return aggroState; }
    }

    public DarkKnightSlideState SlideState
    {
        get { return slideState; }
    }

    public DarkKnightRollState RollState
    {
        get { return rollState; }
    }

    public DarkKnightJumpState JumpState
    {
        get { return jumpState; }
    }

    public DarkKnightJumpAttackState JumpAttackState
    {
        get { return jumpAttackState; }
    }

    public DarkKnightHealthState HealthState
    {
        get { return healthState; }
    }

    public DarkKnightDeathState DeathState
    {
        get { return deathState; }
    }

    public DarkKnightAttackState AttackState
    {
        get { return attackState; }
    }

    public DarkKnightKneelState KneelState
    {
        get { return kneelState; }
    }
    #endregion
}
