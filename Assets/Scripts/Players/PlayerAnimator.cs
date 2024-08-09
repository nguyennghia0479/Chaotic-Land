using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayerMask;

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
    /// The characters receive damage will be knock back.<br></br>
    /// If player has equip weapon can execute item effect.
    /// </remarks>
    private void AnimationAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.AttackCheck.position, player.AttackRadius, enemyLayerMask);

        if (colliders.Length > 0)
        {
            player.InventoryManager.DecreaseGearDurability(GearType.Weapon);
        }

        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out EnemyStats enemy))
            {
                if (enemy.GetComponent<Enemy>().IsDead) return;

                player.Stats.DoPhysicalDamage(enemy);

                GearSO weaponGear = player.InventoryManager.GetGearByGearType(GearType.Weapon);
                if (weaponGear != null)
                {
                    weaponGear.ExecuteItemEffects(enemy.transform);
                }
            }
        }
    }

    /// <summary>
    /// Handles to create a sword after thrown.
    /// </summary>
    private void AnimationThrowSword()
    {
        player.SkillManager.SwordSkill.ThrowSword();
    }

    /// <summary>
    /// Handles to create fire spin.
    /// </summary>
    private void AnimationPerformFireSpin()
    {
        player.SkillManager.FireSpinSkill.CreateFireSpin();
    }
}
