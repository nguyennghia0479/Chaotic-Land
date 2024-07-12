using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerController controller;
    protected Rigidbody2D rb;
    protected float stateTimer;
    protected float xInput;
    protected bool isJumping;
    private readonly string animName;

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animName)
    {
        player = _player;
        stateMachine = _stateMachine;
        animName = _animName;
    }

    public virtual void Enter()
    {
        rb = player.Rb;
        controller = player.Controller;
        player.Animator.SetBool(animName, true);
    }

    public virtual void Update()
    {
        Vector2 moveDir = controller.GetMoveDirNormalized();
        xInput = moveDir.x;
        player.Animator.SetFloat("yVelocity", rb.velocity.y);
    }

    public virtual void FixedUpdate()
    {

    }

    public virtual void Exit()
    {
        player.Animator.SetBool(animName, false);
    }

}
