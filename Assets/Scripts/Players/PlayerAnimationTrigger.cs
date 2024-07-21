using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTrigger : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    /// <summary>
    /// Handles to finish animation of the character.
    /// </summary>
    private void AnimationFinished()
    {
        player.AnimationTrigger();
    }

    /// <summary>
    /// Handles to make damage of the target charaters. 
    /// </summary>
    /// <remarks>
    /// The characters receive damage will be knock back.
    /// </remarks>
    private void AnimationAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.AttackCheck.position, player.AttackRadius);

        foreach(Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out Enemy enemy))
            {
                if (enemy.IsDead) return;

                enemy.SetupKnockBack(player.transform, false);
                enemy.EnemyDead();
            }
        }
    }

    /// <summary>
    /// Handles to create a sword after thrown
    /// </summary>
    private void AnimationThrowSword()
    {
        player.SkillManager.SwordSkill.ThrowSword();
    }
}
