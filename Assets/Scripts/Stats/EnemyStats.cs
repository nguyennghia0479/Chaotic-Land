using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : EntityStats
{
    [Header("Level info")]
    [SerializeField] protected int level;
    [SerializeField] protected Stat exp;
    [SerializeField] private float percentageModify;

    private Enemy enemy;

    protected override void Start()
    {
        base.Start();
        enemy = GetComponent<Enemy>();
    }

    /// <summary>
    /// Handles to make damage on target.
    /// </summary>
    /// <param name="_targetStats">Target's be taken damage</param>
    public override void DoPhysicalDamage(EntityStats _targetStats)
    {
        if (_targetStats == null || _targetStats.Entity.IsDead || CanTargetEvadeAttack(_targetStats))
        {
            PlayMissAttackSound();
            return;
        }

        float totalDamage = physicsDamage.GetValueWithModify();
        if (enemy.IsCriticalAttack)
        {
            totalDamage = CalculateCritDamage(totalDamage);
        }

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(transform, totalDamage, enemy.IsCriticalAttack);
        PlayHitAttackSound();
    }

    /// <summary>
    /// Handles to make enemy's level up base on player's level.
    /// </summary>
    /// <param name="_level"></param>
    public void LevelUp(int _level)
    {
        level = _level;
        ModifiyStatsByLevel();
        InitStats();
        GetComponentInChildren<EnemyStatsUI>().UpdateLevelUI();
    }

    /// <summary>
    /// Handles to modify enemy stats by level
    /// </summary>
    private void ModifiyStatsByLevel()
    {
        ModifyStat(maxHealth);
        ModifyStat(physicsDamage);
        ModifyStat(magicDamage);
        ModifyStat(armor);
        ModifyStat(resistance);
        ModifyStat(exp);
    }

    /// <summary>
    /// Handles to modify stat
    /// </summary>
    /// <param name="stat">Stat need to be modified</param>
    private void ModifyStat(Stat stat)
    {
        for (int i = 1; i < level; i++)
        {
            int modifyStat = Mathf.RoundToInt(stat.GetValueWithModify() * percentageModify);
            stat.UpdateBaseValue(stat.GetValueWithModify() + modifyStat);
        }
    }

    public int Level
    {
        get { return level; }
    }

    public Stat Exp
    {
        get { return exp; }
    }
}
