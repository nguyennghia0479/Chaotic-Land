using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : EntityStats
{
    [SerializeField] private float percentageModify;

    private Enemy enemy;

    protected override void Start()
    {
        ModifiyStatsByLevel();
        base.Start();
        enemy = GetComponent<Enemy>();
    }

    /// <summary>
    /// Handles to make damage on target.
    /// </summary>
    /// <param name="_targetStats">Target's be taken damage</param>
    public override void DoPhysicalDamage(EntityStats _targetStats)
    {
        if (CanTargetEvadeAttack(_targetStats)) return;

        int totalDamage = physicsDamage.GetValue() + strength.GetValue();
        if (enemy.IsCriticalAttack)
        {
            totalDamage = CalculateCritDamage(totalDamage);
        }

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(transform, totalDamage, enemy.IsCriticalAttack);
    }

    /// <summary>
    /// Handles to modify enemy stats by level
    /// </summary>
    private void ModifiyStatsByLevel()
    {
        ModifyStat(physicsDamage);
        ModifyStat(magicDamage);
        ModifyStat(maxHealth);
        ModifyStat(armor);
        ModifyStat(resistance);
    }

    /// <summary>
    /// Handles to modify stat
    /// </summary>
    /// <param name="stat">Stat need to be modified</param>
    private void ModifyStat(Stat stat)
    {
        for (int i = 1; i < level; i++)
        {
            int modifyStat = Mathf.RoundToInt(stat.GetValue() * percentageModify);
            stat.AddModify(modifyStat);
        }
    }
}
