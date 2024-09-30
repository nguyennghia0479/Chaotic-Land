using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : EntityStats, ISaveManager
{
    private Player player;

    protected override void Start()
    {
        base.Start();

        player = PlayerManager.Instance.Player;
    }

    #region Player stats
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
    /// Handles to increase player's stat.
    /// </summary>
    /// <param name="_type"></param>
    /// <param name="_point"></param>
    public void IncreaseStat(StatType _type, float _point)
    {
        Stat stat = GetStatByType(_type);
        stat.UpdateBaseValue(stat.GetValueWithoutModify(_point));
    }
    #endregion

    #region Player attack and decrease health.
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
    #endregion

    /// <summary>
    /// Handles to save player's stats.
    /// </summary>
    /// <param name="_gameData"></param>
    public void SaveData(ref GameData _gameData)
    {
        if (_gameData == null) return;

        _gameData.vitality = vitality.GetValueWithModify();
        _gameData.endurance = endurance.GetValueWithModify();
        _gameData.strength = strength.GetValueWithModify();
        _gameData.dexterity = dexterity.GetValueWithModify();
        _gameData.intelligence = intelligence.GetValueWithModify();
        _gameData.agility = agility.GetValueWithModify();
    }

    /// <summary>
    /// Handles to load player's stats.
    /// </summary>
    /// <param name="_gameData"></param>
    public void LoadData(GameData _gameData)
    {
        if (_gameData == null) return;

        vitality.UpdateBaseValue(_gameData.vitality);
        endurance.UpdateBaseValue(_gameData.endurance);
        strength.UpdateBaseValue(_gameData.strength);
        dexterity.UpdateBaseValue(_gameData.dexterity);
        intelligence.UpdateBaseValue(_gameData.intelligence);
        agility.UpdateBaseValue(_gameData.agility);
    }
}
