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
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private Vector2[] attackMovements;

    private PlayerController controller;
    private SkillManager skillManager;
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
    private PlayerPerformFireSpinState performFireSpinState;
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
    private const string PERFORM_FIRE_SPIN = "PerformFireSpin";
    private const string SPELL_CAST = "SpellCast";
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
        performFireSpinState = new PlayerPerformFireSpinState(this, stateMachine, PERFORM_FIRE_SPIN);
        spellCastState = new PlayerSpellCastState(this, stateMachine, SPELL_CAST);

        controller = GetComponent<PlayerController>();

    }

    protected override void Start()
    {
        base.Start();

        stateMachine.IntializedState(idleState);

        if (controller != null)
        {
            controller.OnDashAction += PlayerController_OnDashAction;
            controller.OnBlockActionStart += PlayerController_OnBlockActionStart;
            controller.OnBlockActionEnd += PlayerController_OnBlockActionEnd;
            controller.OnAimActionStart += PlayerController_OnAimActionStart;
            controller.OnUltimateAction += PlayerController_OnUltimateAction;
            controller.OnSpellCastAction += PlayerController_OnSpellCastAction;
        }

        skillManager = SkillManager.Instance;
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
    public void PlayerDeath()
    {
        stateMachine.ChangeState(DeathState);
        isDead = true;
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
    #endregion

    #region private methods
    /// <summary>
    /// Handles to perform dash of the character.
    /// </summary>
    private void PlayerController_OnDashAction(object sender, EventArgs e)
    {
        if (IsWallDetected()) return;

        if (skillManager.DashSkill.CanUseSkill())
        {
            stateMachine.ChangeState(dashState);
        }
    }

    /// <summary>
    /// Handles to perform block of the charater.
    /// </summary>
    private void PlayerController_OnBlockActionStart(object sender, EventArgs e)
    {
        isBlocking = true;
        stateMachine.ChangeState(blockState);
    }

    /// <summary>
    /// Handles to cancel block of the character.
    /// </summary>
    private void PlayerController_OnBlockActionEnd(object sender, EventArgs e)
    {
        isBlocking = false;
        stateMachine.ChangeState(idleState);
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
        if (skillManager.SwordSkill.CanUseSkill() && sword == null)
        {
            stateMachine.ChangeState(aimSwordState);
        }

        if (sword != null)
        {
            sword.GetComponent<SwordSkillController>().RecallSword();
        }
    }

    /// <summary>
    /// Handles to perform fire spin and trigger of the character.
    /// </summary>
    private void PlayerController_OnUltimateAction(object sender, EventArgs e)
    {
        if (skillManager.FireSpinSkill.CanUseSkill() && fireSpin == null)
        {
            stateMachine.ChangeState(performFireSpinState);
        }

        if (fireSpin != null)
        {
            fireSpin.GetComponent<FireSpinSkillController>().TriggerFireSpinGrow();
        }
    }

    /// <summary>
    /// Handles to perform spell cast of the character.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PlayerController_OnSpellCastAction(object sender, EventArgs e)
    {
        if (skillManager.CrystalSkill.CanUseSkill())
        {
            stateMachine.ChangeState(spellCastState);
        }
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

    public float AttackCooldown
    {
        get { return attackCooldown; }
    }

    public Vector2[] AttackMovements
    {
        get { return attackMovements; }
    }

    public PlayerController Controller
    {
        get { return controller; }
    }

    public SkillManager SkillManager
    {
        get { return skillManager; }
    }

    public GameObject Sword
    {
        get { return sword; }
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

    public PlayerPerformFireSpinState PerformFireSpinState
    {
        get { return performFireSpinState; }
    }

    public PlayerSpellCastState SpellCastState
    {
        get { return spellCastState; }
    }
    #endregion
}
