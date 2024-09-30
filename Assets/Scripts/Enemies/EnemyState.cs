using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected Enemy enemy;
    protected EnemyStateMachine stateMachine;
    protected Rigidbody2D rb;
    protected Animator anim;
    protected SoundManager soundManager;
    protected float stateTimer;
    protected bool triggerCalled;
    protected float lastTimeAttacked;
    private readonly string animName;

    public EnemyState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animName)
    {
        enemy = _enemy;
        stateMachine = _stateMachine;
        animName = _animName;
    }

    public virtual void Enter()
    {
        rb = enemy.Rb;
        anim = enemy.Animator;
        enemy.Animator.SetBool(animName, true);
        soundManager = enemy.SoundManager;
        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void FixedUpdate()
    {

    }

    public virtual void Exit()
    {
        enemy.Animator.SetBool(animName, false);
    }

    public void AnimationTrigger()
    {
        triggerCalled = true;
    }
}
