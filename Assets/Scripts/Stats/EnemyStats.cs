using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : EntityStats
{
    [Header("Level info")]
    [SerializeField] protected int level;
    [SerializeField] protected Stat exp;
    [SerializeField] private float healthPerctModify;
    [SerializeField] private float physicsDamagePerctModify;
    [SerializeField] private float magicDamagePerctModify;
    [SerializeField] private float armorPerctModify;
    [SerializeField] private float resistancePerctModify;
    [SerializeField] private float expPerctModify;

    private Enemy enemy;
    private int currentLevel = 1;
    private float origMaxHealth;
    private float origPhysicsDamage;
    private float origMagicDamage;
    private float origArmor;
    private float origResistance;
    private float origExp;

    protected override void Start()
    {
        base.Start();
        enemy = GetComponent<Enemy>();

        SetOriginalStats();
    }

    /// <summary>
    /// Handles to make damage on target.
    /// </summary>
    /// <param name="_targetStats">Target's be taken damage</param>
    public override void DoPhysicalDamage(EntityStats _targetStats, float _damage = 0)
    {
        if (_targetStats == null || _targetStats.Entity.IsDead || CanTargetEvadeAttack(_targetStats))
        {
            PlayMissAttackSound();
            fx.PlayPopupMissDamageText();
            return;
        }

        float totalDamage = _damage == 0 ? physicsDamage.GetValueWithModify() : _damage;
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
    /// <param name="_targetLevel"></param>
    public void LevelUp(int _targetLevel)
    {
        EnemyStatsUI statsUI = GetComponentInChildren<EnemyStatsUI>();
        if (statsUI != null)
        {
            level = _targetLevel;
            ModifyStatsByLevel();
            InitStats();
            statsUI.UpdateLevelUI();
            currentLevel = level;
        }
    }

    /// <summary>
    /// Handles to modify enemy stats by level
    /// </summary>
    private void ModifyStatsByLevel()
    {
        ModifyStat(maxHealth, origMaxHealth, healthPerctModify);
        ModifyStat(physicsDamage, origPhysicsDamage, physicsDamagePerctModify);
        ModifyStat(magicDamage, origMagicDamage, magicDamagePerctModify);
        ModifyStat(armor, origArmor, armorPerctModify);
        ModifyStat(resistance, origResistance, resistancePerctModify);
        ModifyStat(exp, origExp, expPerctModify);
    }

    /// <summary>
    /// Handles to modify stat
    /// </summary>
    /// <param name="_stat">Stat need to be modified</param>
    private void ModifyStat(Stat _stat, float _origStat, float _percentageModify)
    {
        for (int i = currentLevel; i < level; i++)
        {
            _percentageModify = UpdatePercentageModify(i, _percentageModify);
            int modifyStat = Mathf.RoundToInt(_origStat * _percentageModify);
            _stat.UpdateBaseValue(_stat.GetValueWithModify() + modifyStat);
        }
    }

    private float UpdatePercentageModify(int _currentLevel, float _percentageModify)
    {
        float _newPerctModify = _percentageModify;
        float decreasePerct = 0;
        if (_currentLevel % 5 == 0)
        {
            decreasePerct += .1f;
        }

        _newPerctModify -= _newPerctModify * decreasePerct;
        return _newPerctModify;
    }

    /// <summary>
    /// Handles to set original stats.
    /// </summary>
    private void SetOriginalStats()
    {
        origMaxHealth = maxHealth.GetValueWithModify();
        origPhysicsDamage = physicsDamage.GetValueWithModify();
        origMagicDamage = magicDamage.GetValueWithModify();
        origArmor = armor.GetValueWithModify();
        origResistance = resistance.GetValueWithModify();
        origExp = exp.GetValueWithModify();
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
