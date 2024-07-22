using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private Enemy enemy;
    private int count;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    /// <summary>
    /// Handles to finish animation of the character.
    /// </summary>
    private void AnimationFinished()
    {
        enemy.AnimationTrigger();
    }

    /// <summary>
    /// Handles to make damage of the target charaters. 
    /// </summary>
    /// <remarks>
    /// The characters receive damage will be knock back.
    /// </remarks>
    private void AnimationAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.AttackCheck.position, enemy.AttackRadius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out Player player))
            {
                player.SetupKnockBack(enemy.transform, enemy.IsCriticalAttack);
                count++;
                if (count == 3)
                {
                    player.PlayerDeath();
                }
            }
        }
    }

    /// <summary>
    /// Handles to setup of pre attack.
    /// </summary>
    private void AnimationPrepareAttack()
    {
        enemy.AnimationPrepareAttack();
    }

    /// <summary>
    /// Handles to setup of finish attack.
    /// </summary>
    private void AnimationAttackFinished()
    {
        enemy.AnimationAttackFinished();
    }
}
