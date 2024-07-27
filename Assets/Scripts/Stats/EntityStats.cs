using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AilementType
{
    Ignite, Chill
}

public class EntityStats : MonoBehaviour
{
    #region Variables
    [Header("Attribute stats info")]
    public Stat vitality;      // Increase 1 point of max health for each point.
    public Stat endurance;     // Increase 1 point of statmina for each point.
    public Stat strength;      // Increase 1 point of physics damage for each point.
    public Stat dexterity;     // Increase 1 point of crit chance for each point.
    public Stat intelligence;  // Increase 1 point of magic damage and resistance for each point.
    public Stat agility;       // Increase 1 point of evasion for each point.

    [Header("General stats info")]
    public Stat maxHealth;      // Responsible for player's max health.
    public Stat stamina;        // Responsible for player's stamina.

    [Header("Offensive stats info")]
    public Stat physicsDamage;  // Responsible for player's physics damage.
    public Stat critChance;     // Responsible for player's critical chance.
    public Stat critPower;      // Responsible for player's critical power.
    public Stat magicDamage;    // Responsible for player's magic damage.

    [Header("Defensive stats info")]
    public Stat evasion;        // Responsible for evading of enemy attacks.
    public Stat armor;          // Responsible for reducing physics damage of enemy attacks.
    public Stat resistance;     // Responsible for reducing magic damage of enemy attacks.

    [Header("Ailment info")]
    public Stat igniteDamage;
    [SerializeField] protected float ignitedDuration = 3;
    [SerializeField] protected float igniteHitCooldown = .5f;
    [SerializeField] protected float chilledDuration = 2;
    [SerializeField] protected float slowPercentage = .2f;

    [SerializeField] protected int level;
    [SerializeField] protected int currentHealth;
    protected Entity entity;
    protected EntityFX fx;
    protected bool isIgnite;
    protected float igniteTimer;
    protected float igniteHitTimer;
    protected bool isChilled;
    #endregion

    protected virtual void Start()
    {
        entity = GetComponent<Entity>();
        fx = GetComponent<EntityFX>();

        SetCurrentHealth();
    }

    protected virtual void Update()
    {
        igniteTimer -= Time.deltaTime;
        igniteHitTimer -= Time.deltaTime;

        if (igniteTimer < 0)
        {
            isIgnite = false;
        }

        if (isIgnite)
        {
            DoIgniteDamage();
        }
    }

    /// <summary>
    /// Handles to set up target's be taken damage.
    /// </summary>
    /// <param name="_damageDealer"></param>
    /// <param name="_damage"></param>
    /// <param name="_isCriticalAttack"></param>
    public virtual void TakeDamage(Transform _damageDealer, int _damage, bool _isCriticalAttack)
    {
        entity.SetupKnockBack(_damageDealer, _isCriticalAttack);
        if (entity.IsBlocking && !_isCriticalAttack) return;

        fx.PlayFlashFX();
        DecreaseHealth(_damage);
    }

    /// <summary>
    /// Handles to descrease health.
    /// </summary>
    /// <param name="_damage"></param>
    protected void DecreaseHealth(int _damage)
    {
        currentHealth -= _damage;

        if (currentHealth <= 0)
        {
            entity.SetupDeath();
        }
    }

    /// <summary>
    /// Handles to make physical damage.
    /// </summary>
    /// <param name="_targetStats"></param>
    public virtual void DoPhysicalDamage(EntityStats _targetStats)
    {
        if (CanTargetEvadeAttack(_targetStats) || entity.IsDead) return;

        int totalDamage = physicsDamage.GetValue() + strength.GetValue();
        bool canCrit = CanDoCritDamage();

        if (canCrit)
        {
            totalDamage = CalculateCritDamage(totalDamage);
        }

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(transform, totalDamage, canCrit);
    }

    #region Magic damage
    /// <summary>
    /// Handle to make magic damage.
    /// </summary>
    /// <param name="_targetStats"></param>
    /// <param name="_type"></param>
    public virtual void DoMagicDamage(EntityStats _targetStats, AilementType _type)
    {
        if (CanTargetEvadeAttack(_targetStats) || entity.IsDead) return;

        int totalDamage = magicDamage.GetValue() + intelligence.GetValue();
        totalDamage = CheckTargetResistance(_targetStats, totalDamage);
        _targetStats.TakeDamage(transform, totalDamage, false);
        _targetStats.ApplyAilement(_type);

        if (_type == AilementType.Ignite)
        {
            _targetStats.SetIgniteDamage(igniteDamage);
        }
    }

    /// <summary>
    /// Handles to apply ailment.
    /// </summary>
    /// <param name="_type"></param>
    protected void ApplyAilement(AilementType _type)
    {
        if (_type == AilementType.Ignite && !isIgnite)
        {
            isIgnite = true;
            igniteTimer = ignitedDuration;
            fx.PlayIgnitedFX(ignitedDuration);
        }
        else if (_type == AilementType.Chill)
        {
            isChilled = true;
            fx.PlayChilledFX(chilledDuration);
            entity.SlowEntityEffect(slowPercentage, chilledDuration);
        }
    }

    /// <summary>
    /// Handles to set ignite damage
    /// </summary>
    /// <param name="_igniteDamage"></param>
    protected void SetIgniteDamage(Stat _igniteDamage) => igniteDamage = _igniteDamage;

    /// <summary>
    /// Handles to make ignite damage.
    /// </summary>
    protected void DoIgniteDamage()
    {
        if (igniteHitTimer < 0)
        {
            igniteHitTimer = igniteHitCooldown;
            DecreaseHealth(igniteDamage.GetValue());
        }
    }
    #endregion

    #region Stats
    /// <summary>
    /// Handles to set current health.
    /// </summary>
    protected void SetCurrentHealth()
    {
        currentHealth = maxHealth.GetValue() + vitality.GetValue();
    }

    /// <summary>
    /// Handles to check target can evade.
    /// </summary>
    /// <param name="_targetStats"></param>
    /// <returns>True if can evade. False if not.</returns>
    protected bool CanTargetEvadeAttack(EntityStats _targetStats)
    {
        int totalEvasionChance = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        return Utils.RandomChance(totalEvasionChance);
    }

    /// <summary>
    /// Handles to check can make critical damage.
    /// </summary>
    /// <returns>True if can do critical. False if not.</returns>
    protected bool CanDoCritDamage()
    {
        int totalCritChance = critChance.GetValue() + dexterity.GetValue();

        return Utils.RandomChance(totalCritChance);
    }

    /// <summary>
    /// Handles to calculate critical damage.
    /// </summary>
    /// <param name="_totalDamage"></param>
    /// <returns>Critical damage.</returns>
    protected int CalculateCritDamage(int _totalDamage)
    {
        _totalDamage += _totalDamage * critPower.GetValue() / 100;

        return _totalDamage;
    }

    /// <summary>
    /// Handles to calculate damage vs armor
    /// </summary>
    /// <param name="_targetStats"></param>
    /// <param name="_totalDamage"></param>
    /// <returns>Final damage.</returns>
    protected int CheckTargetArmor(EntityStats _targetStats, int _totalDamage)
    {
        _totalDamage -= _targetStats.armor.GetValue();
        _totalDamage = Mathf.Clamp(_totalDamage, 0, int.MaxValue);

        return _totalDamage;
    }

    /// <summary>
    /// Handles to calculate magic damage vs resistance.
    /// </summary>
    /// <param name="_targetStats"></param>
    /// <param name="_totalDamage"></param>
    /// <returns>Final damage.</returns>
    protected int CheckTargetResistance(EntityStats _targetStats, int _totalDamage)
    {
        _totalDamage -= _targetStats.resistance.GetValue() + _targetStats.intelligence.GetValue();
        _totalDamage = Mathf.Clamp(_totalDamage, 0, int.MaxValue);

        return _totalDamage;
    }
    #endregion
}
