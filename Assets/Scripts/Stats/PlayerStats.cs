using System;
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
    /// Handles to init stats of player.
    /// </summary>
    public override void InitStats()
    {
        base.InitStats();

        physicsDamage.UpdateBaseValue(CalculateStatModify(StatType.PhysicsDamage, strength.GetValueWithModify()));
        critChance.UpdateBaseValue(CalculateStatModify(StatType.CritChance, dexterity.GetValueWithModify()));
        critPower.UpdateBaseValue(CalculateStatModify(StatType.CritPower, strength.GetValueWithModify()));
        magicDamage.UpdateBaseValue(CalculateStatModify(StatType.MagicDamage, intelligence.GetValueWithModify()));
        evasion.UpdateBaseValue(CalculateStatModify(StatType.Evasion, agility.GetValueWithModify()));
        resistance.UpdateBaseValue(CalculateStatModify(StatType.Resistance, intelligence.GetValueWithModify()));
    }

    /// <summary>
    /// Handles to make physical damage by clone.
    /// </summary>
    /// <param name="_targetStats"></param>
    /// <param name="attackPercentage"></param>
    public void CloneAttackDamage(EntityStats _targetStats, float attackPercentage)
    {
        if (entity.IsDead) return;

        float totalDamage = physicsDamage.GetValueWithModify();
        totalDamage = Mathf.RoundToInt(totalDamage * (1 - attackPercentage));

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(transform, totalDamage, false);
    }

    /// <summary>
    /// Handles to make player taken damage.
    /// </summary>
    /// <param name="_damageDealer"></param>
    /// <param name="_damage"></param>
    /// <param name="_isCriticalAttack"></param>
    public override void TakeDamage(Transform _damageDealer, float _damage, bool _isCriticalAttack)
    {
        base.TakeDamage(_damageDealer, _damage, _isCriticalAttack);

        if (_isCriticalAttack)
        {
            player.CancelBlock();
        }
    }

    /// <summary>
    /// Handles to decrease player health.
    /// </summary>
    /// <param name="_damage"></param>
    /// <remarks>
    /// If player has equip armor can execute item effect.
    /// </remarks>
    protected override void DecreaseHealth(float _damage)
    {
        base.DecreaseHealth(_damage);

        player.InventoryManager.DecreaseGearDurability(GearType.Armor);
        GearSO armorGear = player.InventoryManager.GetGearByGearType(GearType.Armor);
        if (armorGear != null)
        {
            armorGear.ExecuteItemEffects(null);
        }
    }

    /// <summary>
    /// Handles to increase player's stat.
    /// </summary>
    /// <param name="_type"></param>
    /// <param name="_point"></param>
    public void IncreaseStat(StatType _type, float _point)
    {
        Stat stat = GetStatByType(_type);
        stat.UpdateBaseValue(stat.GetValueWithoutModify(_point));
    }
}
