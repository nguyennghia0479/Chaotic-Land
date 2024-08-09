using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : EntityStats
{
    private Player player;

    protected override void Start()
    {
        base.Start();

        player = PlayerManager.Instance.Player;
    }

    /// <summary>
    /// Handles to make physical damage by clone.
    /// </summary>
    /// <param name="_targetStats"></param>
    /// <param name="attackPercentage"></param>
    public void CloneAttackDamage(EntityStats _targetStats, float attackPercentage)
    {
        if (entity.IsDead) return;

        int totalDamage = physicsDamage.GetValue() + strength.GetValue();
        totalDamage = Mathf.RoundToInt(totalDamage * (1 - attackPercentage));

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(transform, totalDamage, false);
    }

    /// <summary>
    /// Handles to decrease player health.
    /// </summary>
    /// <param name="_damage"></param>
    /// <remarks>
    /// If player has equip armor can execute item effect.
    /// </remarks>
    protected override void DecreaseHealth(int _damage)
    {
        base.DecreaseHealth(_damage);

        player.InventoryManager.DecreaseGearDurability(GearType.Armor);
        GearSO armorGear = player.InventoryManager.GetGearByGearType(GearType.Armor);
        if (armorGear != null)
        {
            armorGear.ExecuteItemEffects(null);
        }
    }
}
