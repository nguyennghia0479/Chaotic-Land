using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    protected Enemy enemy;

    protected virtual void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    /// <summary>
    /// Handles to finish animation of the character.
    /// </summary>
    protected void AnimationFinished()
    {
        enemy.AnimationTrigger();
    }

    /// <summary>
    /// Handles to make damage of the target charaters. 
    /// </summary>
    /// <remarks>
    /// The characters receive damage will be knock back.
    /// </remarks>
    protected void AnimationAttack()
    {
        bool isHit = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.AttackCheck.position, enemy.AttackRadius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out PlayerStats player))
            {
                isHit = true;
                enemy.Stats.DoPhysicalDamage(player);
            }
        }

        PlayAttackSound(isHit);
    }

    /// <summary>
    /// Handles to setup of pre attack.
    /// </summary>
    protected void AnimationPrepareAttack()
    {
        enemy.AnimationPrepareAttack();
    }

    /// <summary>
    /// Handles to setup of finish attack.
    /// </summary>
    protected void AnimationAttackFinished()
    {
        enemy.AnimationAttackFinished();
    }

    /// <summary>
    /// Handles to play attack sound.
    /// </summary>
    /// <param name="_hit"></param>
    protected void PlayAttackSound(bool _hit)
    {
        if (_hit) return;

        enemy.SoundManager.PlayAttackSound(transform.position);
    }
}
