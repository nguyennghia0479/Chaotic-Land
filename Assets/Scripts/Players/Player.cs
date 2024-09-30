using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    #region Variables
    [Header("Move info")]
    [SerializeField] private int moveSpeed = 8;
    [SerializeField] private int jumpForce = 12;

    [Header("Attack info")]
    [SerializeField] private int attackStaminaAmount = 10;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private Vector2[] attackMovements;

    [Header("Physics material 2D")]
    [SerializeField] private PhysicsMaterial2D wallStick;
    [SerializeField] private PhysicsMaterial2D slopeSlide;

    private PlayerController controller;
    private PlayerStats playerStats;
    private SkillManager skillManager;
    private InventoryManager inventoryManager;
    private GameManager gameManager;
    private GameObject sword;
    private GameObject fireSpin;
    private bool isAiming;
    private bool hasThrown;

    private PlayerStateMachine stateMachine;
    private PlayerIdleState idleState;
    private PlayerMoveState moveState;
    private PlayerJumpState jumpState;
    private PlayerAirState airState;
    private PlayerDashState dashState;
    private PlayerWallSlideState wallSlideState;
    private PlayerWallGrabState wallGrabState;
    private PlayerWallJumpState wallJumpState;
    private PlayerAttackState attackState;
    private PlayerBlockState blockState;
    private PlayerDeathState deathState;
    private PlayerAimSwordState aimSwordState;
    private PlayerCatchSwordState catchSwordState;
    private PlayerPerformUltimateState performUltimateState;
    private PlayerSpellCastState spellCastState;

    private const string IDLE = "Idle";
    private const string MOVE = "Move";
    private const string JUMP = "Jump";
    private const string DASH = "Dash";
    private const string WALL_SLIDE = "WallSlide";
    private const string WALL_GRAB = "WallGrab";
    private const string ATTACK = "Attack";
    private const string BLOCK = "Block";
    private const string DIE = "Die";
    private const string AIM_SWORD = "AimSword";
    private const string CATCH_SWORD = "CatchSword";
    private const string PERFORM_ULTIMATE = "PerformUltimate";
    private const string SPELL_CAST = "SpellCast";

    public event EventHandler OnDie;
    #endregion

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new();
        idleState = new PlayerIdleState(this, stateMachine, IDLE);
        moveState = new PlayerMoveState(this, stateMachine, MOVE);
        jumpState = new PlayerJumpState(this, stateMachine, JUMP);
        airState = new PlayerAirState(this, stateMachine, JUMP);
        dashState = new PlayerDashState(this, stateMachine, DASH);
        wallSlideState = new PlayerWallSlideState(this, stateMachine, WALL_SLIDE);
        wallGrabState = new PlayerWallGrabState(this, stateMachine, WALL_GRAB);
        wallJumpState = new PlayerWallJumpState(this, stateMachine, JUMP);
        attackState = new PlayerAttackState(this, stateMachine, ATTACK);
        blockState = new PlayerBlockState(this, stateMachine, BLOCK);
        deathState = new PlayerDeathState(this, stateMachine, DIE);
        aimSwordState = new PlayerAimSwordState(this, stateMachine, AIM_SWORD);
        catchSwordState = new PlayerCatchSwordState(this, stateMachine, CATCH_SWORD);
        performUltimateState = new PlayerPerformUltimateState(this, stateMachine, PERFORM_ULTIMATE);
        spellCastState = new PlayerSpellCastState(this, stateMachine, SPELL_CAST);

        controller = GetComponent<PlayerController>();

    }

    protected override void Start()
    {
        base.Start();

        if (controller != null)
        {
            controller.OnDashAction += PlayerController_OnDashAction;
            controller.OnBlockActionStart += PlayerController_OnBlockActionStart;
            controller.OnBlockActionEnd += PlayerController_OnBlockActionEnd;
            controller.OnAimActionStart += PlayerController_OnAimActionStart;
            controller.OnUltimateAction += PlayerController_OnUltimateAction;
            controller.OnSpellCastAction += PlayerController_OnSpellCastAction;
            controller.OnUseFlaskAction += PlayerController_OnUseFlaskAction;
        }

        playerStats = Stats as PlayerStats;
        skillManager = SkillManager.Instance;
        inventoryManager = InventoryManager.Instance;
        gameManager = GameManager.Instance;

        stateMachine.IntializedState(idleState);
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

    protected void OnDestroy()
    {
        if (controller != null)
        {
            controller.OnDashAction -= PlayerController_OnDashAction;
            controller.OnBlockActionStart -= PlayerController_OnBlockActionStart;
            controller.OnBlockActionEnd -= PlayerController_OnBlockActionEnd;
            controller.OnAimActionStart -= PlayerController_OnAimActionStart;
            controller.OnUltimateAction -= PlayerController_OnUltimateAction;
            controller.OnSpellCastAction -= PlayerController_OnSpellCastAction;
            controller.OnUseFlaskAction -= PlayerController_OnUseFlaskAction;
        }
    }

    #region public methods
    /// <summary>
    /// Handles jumping and flipping of the character.
    /// </summary>
    /// <param name="_xVelocity">The horizontal velecity to move the character.</param>
    public void AddForce(float _xVelocity)
    {
        rb.AddForce(new Vector2(_xVelocity, jumpForce), ForceMode2D.Impulse);
        FlipController(_xVelocity);
    }

    /// <summary>
    /// Handles to trigger animation of the character
    /// </summary>
    public void AnimationTrigger() => stateMachine.CurrentState.AnimationTrigger();

    /// <summary>
    /// Handles to chanage state to DeathState.
    /// </summary>
    public override void SetupDeath()
    {
        soundManager.PlayDeathSound(transform.position);
        stateMachine.ChangeState(DeathState);
        isDead = true;
        OnDie?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Handles to flip the character by target pos.
    /// </summary>
    /// <param name="_targetPos">The value to determine position of target.</param>
    public void SetAimAndCatchSwordFlip(Vector2 _targetPos)
    {
        if (transform.position.x < _targetPos.x && !isFacingRight)
        {
            Flip();
        }
        else if (transform.position.x > _targetPos.x && isFacingRight)
        {
            Flip();
        }
    }

    /// <summary>
    /// Handles to set sword after thrown.
    /// </summary>
    /// <param name="_sword">The value to set sword agme object for player.</param>
    public void AssignSword(GameObject _sword)
    {
        sword = _sword;
    }

    /// <summary>
    /// Handles to catch sword after recalled sword.
    /// </summary>
    public void CatchSword()
    {
        stateMachine.ChangeState(catchSwordState);
        soundManager.PlayCatchSwordSound(transform.position);
        Destroy(sword);
    }

    /// <summary>
    /// Handles to set fire spin.
    /// </summary>
    /// <param name="_fireSpin">The value to set fire spin game object for player.</param>
    public void AssignFireSpin(GameObject _fireSpin)
    {
        fireSpin = _fireSpin;
    }

    /// <summary>
    /// Handles to update physics material of the character.
    /// </summary>
    /// <param name="_physicsMaterial"></param>
    public void UpdatePhysicsMaterial(PhysicsMaterial2D _physicsMaterial)
    {
        if (TryGetComponent(out CapsuleCollider2D collider2D))
        {
            collider2D.sharedMaterial = _physicsMaterial;
        }
    }

    public void CancelBlock()
    {
        if (isDead) return;

        isBlocking = false;
        stateMachine.ChangeState(idleState);
    }
    #endregion

    #region private methods
    /// <summary>
    /// Handles to perform dash of the character.
    /// </summary>
    private void PlayerController_OnDashAction(object sender, EventArgs e)
    {
        int dashStaminaAmount = skillManager.DashSkill.SkillStaminaAmount;
        if (IsWallDetected() || isDead || gameManager.IsGamePaused || playerStats.CurrentStamina < dashStaminaAmount) return;

        if (skillManager.DashSkill.CanUseSkill())
        {
            playerStats.DecreaseStamina(dashStaminaAmount);
            stateMachine.ChangeState(dashState);
        }
    }

    /// <summary>
    /// Handles to perform block of the charater.
    /// </summary>
    private void PlayerController_OnBlockActionStart(object sender, EventArgs e)
    {
        if (isDead || gameManager.IsGamePaused) return;

        if (skillManager.ParrySkill.CanUseSkill())
        {
            isBlocking = true;
            stateMachine.ChangeState(blockState);
        }
    }

    /// <summary>
    /// Handles to cancel block of the character.
    /// </summary>
    private void PlayerController_OnBlockActionEnd(object sender, EventArgs e)
    {
        if (isDead || gameManager.IsGamePaused) return;

        CancelBlock();
    }

    /// <summary>
    /// Handles to perform aim and recall of the character.
    /// </summary>
    /// <remarks>
    /// Change to aim sword state if character has not been assigned sword.<br></br>
    /// Recall sword if sword has been assigned by character.
    /// </remarks>
    private void PlayerController_OnAimActionStart(object sender, EventArgs e)
    {
        if (sword != null)
        {
            sword.GetComponent<SwordSkillController>().RecallSword();
            return;
        }

        int throwSwordStaminaAmount = skillManager.SwordSkill.SkillStaminaAmount;
        if (isDead || gameManager.IsGamePaused || playerStats.CurrentStamina < throwSwordStaminaAmount) return;

        if (skillManager.SwordSkill.CanUseSkill() && sword == null)
        {
            stateMachine.ChangeState(aimSwordState);
        }
    }

    /// <summary>
    /// Handles to perform fire spin and trigger of the character.
    /// </summary>
    private void PlayerController_OnUltimateAction(object sender, EventArgs e)
    {
        if (skillManager.UltimateSkill.Type == UltimateType.FireSpin)
        {
            if (fireSpin != null)
            {
                fireSpin.GetComponent<FireSpinSkillController>().TriggerFireSpinGrow();
                return;
            }
        }

        int ultimateStaminaAmount = skillManager.UltimateSkill.SkillStaminaAmount;
        if (isDead || gameManager.IsGamePaused || playerStats.CurrentStamina < ultimateStaminaAmount) return;

        if (skillManager.UltimateSkill.CanUseSkill())
        {
            playerStats.DecreaseStamina(ultimateStaminaAmount);
            stateMachine.ChangeState(performUltimateState);
        }
    }

    /// <summary>
    /// Handles to perform spell cast of the character.
    /// </summary>
    private void PlayerController_OnSpellCastAction(object sender, EventArgs e)
    {
        int spellCastStaminaAmount = skillManager.CrystalSkill.SkillStaminaAmount;
        if (isDead || gameManager.IsGamePaused || playerStats.CurrentStamina < spellCastStaminaAmount) return;

        if (skillManager.CrystalSkill.CanUseSkill())
        {
            playerStats.DecreaseStamina(spellCastStaminaAmount);
            stateMachine.ChangeState(spellCastState);
        }
    }

    /// <summary>
    /// Handles to perform use flask of the character.
    /// </summary>
    private void PlayerController_OnUseFlaskAction(object sender, EventArgs e)
    {
        if (isDead || gameManager.IsGamePaused) return;

        InventoryManager.UseFlask();
        soundManager.PlayUsePotionSound(transform.position);
    }
    #endregion

    #region Getter
    public int MoveSpeed
    {
        get { return moveSpeed; }
    }

    public int JumpForce
    {
        get { return jumpForce; }
    }

    public int AttackStaminaAmount
    {
        get { return attackStaminaAmount; }
    }

    public float AttackCooldown
    {
        get { return attackCooldown; }
    }

    public Vector2[] AttackMovements
    {
        get { return attackMovements; }
    }

    public PhysicsMaterial2D WallStick
    {
        get { return wallStick; }
    }

    public PhysicsMaterial2D SlopeSlide
    {
        get { return slopeSlide; }
    }

    public PlayerController Controller
    {
        get { return controller; }
    }


    public PlayerStats PlayerStats
    {
        get { return playerStats; }
    }

    public SkillManager SkillManager
    {
        get { return skillManager; }
    }

    public GameManager GameManager
    {
        get { return gameManager; }
    }

    public InventoryManager InventoryManager
    {
        get { return inventoryManager; }
    }

    public GameObject Sword
    {
        get { return sword; }
    }

    public GameObject FireSpin
    {
        get { return fireSpin; }
    }

    public bool IsAiming
    {
        get { return isAiming; }
        set { isAiming = value; }
    }

    public bool HasThrown
    {
        get { return hasThrown; }
        set { hasThrown = value; }
    }

    public PlayerIdleState IdleState
    {
        get { return idleState; }
    }

    public PlayerMoveState MoveState
    {
        get { return moveState; }
    }

    public PlayerJumpState JumpState
    {
        get { return jumpState; }
    }

    public PlayerAirState AirState
    {
        get { return airState; }
    }

    public PlayerDashState DashState
    {
        get { return dashState; }
    }

    public PlayerWallSlideState WallSlideState
    {
        get { return wallSlideState; }
    }

    public PlayerWallGrabState WallGrabState
    {
        get { return wallGrabState; }
    }

    public PlayerWallJumpState WallJumpState
    {
        get { return wallJumpState; }
    }

    public PlayerAttackState AttackState
    {
        get { return attackState; }
    }

    public PlayerBlockState BlockState
    {
        get { return blockState; }
    }

    public PlayerDeathState DeathState
    {
        get { return deathState; }
    }

    public PlayerAimSwordState AimSwordState
    {
        get { return aimSwordState; }
    }

    public PlayerCatchSwordState CatchSwordState
    {
        get { return catchSwordState; }
    }

    public PlayerPerformUltimateState PerformUltimateState
    {
        get { return performUltimateState; }
    }

    public PlayerSpellCastState SpellCastState
    {
        get { return spellCastState; }
    }
    #endregion
}
