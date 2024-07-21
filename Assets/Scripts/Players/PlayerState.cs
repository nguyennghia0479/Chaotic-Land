using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerController controller;
    protected Rigidbody2D rb;
    protected Animator anim;
    protected SkillManager skillManager;
    protected float stateTimer;
    protected float xInput;
    protected float yInput;
    protected bool isJumping;
    protected bool triggerCalled;
    private readonly string animName;

    private const string Y_VELOCITY = "yVelocity";

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animName)
    {
        player = _player;
        stateMachine = _stateMachine;
        animName = _animName;
    }

    public virtual void Enter()
    {
        rb = player.Rb;
        anim = player.Animator;
        controller = player.Controller;
        skillManager = player.SkillManager;
        player.Animator.SetBool(animName, true);
        triggerCalled = false;
    }

    public virtual void Update()
    {
        Vector2 moveDir = controller.GetMoveDirNormalized();
        xInput = moveDir.x;
        yInput = moveDir.y;
        player.Animator.SetFloat(Y_VELOCITY, rb.velocity.y);
        stateTimer -= Time.deltaTime;
    }

    public virtual void FixedUpdate()
    {

    }

    public virtual void Exit()
    {
        player.Animator.SetBool(animName, false);
    }

    public void AnimationTrigger()
    {
        triggerCalled = true;
    }
}
