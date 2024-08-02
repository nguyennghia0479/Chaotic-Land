using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkillAnimator : MonoBehaviour
{
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackRadius;
    [SerializeField] private float attackPercentage = .4f;
    [SerializeField] private int maxNumberOfClone = 5;

    private Player player;
    private CloneSkillController cloneSkillController;

    private void Start()
    {
        player = PlayerManager.Instance.Player;
        cloneSkillController = GetComponentInParent<CloneSkillController>();
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
    /// If the random value is lesser then clone can create another clone until random value is equal or greater.
    /// </remarks>
    private void AnimationAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackRadius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out EnemyStats enemy))
            {
                if (enemy.GetComponent<Enemy>().IsDead) return;

                player.GetComponent<PlayerStats>().CloneAttackDamage(enemy, attackPercentage);

                if (Utils.RandomChance(cloneSkillController.ChanceToMulti) && !HasMaxNumberOfClone())
                {
                    float offset = 1 * cloneSkillController.FacingDir;
                    SkillManager.Instance.CloneSkill.CreateClone(enemy.transform, new Vector2(offset, 0));
                }
            }
        }
    }

    private bool HasMaxNumberOfClone()
    {
        CloneSkillController[] clones = FindObjectsOfType<CloneSkillController>();

        return clones.Length >= maxNumberOfClone;
    }
}
