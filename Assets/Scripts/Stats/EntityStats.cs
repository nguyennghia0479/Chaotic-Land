using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AilmentType
{
    Ignite, Chill, None
}

public enum StatType
{
    Vitality, Endurance, Strength, Dexterity, Intelligence, Agility,
    MaxHealth, Stamina,
    PhysicsDamage, CritChance, CritPower, MagicDamage,
    Evasion, Armor, Resistance
}

public class EntityStats : MonoBehaviour
{
    #region Variables
    [Header("Attribute stats info")]
    public Stat vitality;      // Increase 10 point of max health for each point.
    public Stat endurance;     // Increase 2 point of statmina for each point.
    public Stat strength;      // Increase 2 points of physics damage and 0.25 point of critical power for each point.
    public Stat dexterity;     // Increase 1 point of crit chance for each point.
    public Stat intelligence;  // Increase 1 point of magic damage and 0.25 point of resistance for each point.
    public Stat agility;       // Increase 1 point of evasion for each point.

    [Header("General stats info")]
    public Stat maxHealth;      // Responsible for player's max health.
    public Stat maxStamina;        // Responsible for player's stamina.

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

    [Header("Stamina info")]
    [SerializeField] private float rechargeDelay = 1.5f;
    [SerializeField] private float rechargeRate = 10;

    protected Entity entity;
    protected EntityFX fx;
    protected bool isIgnite;
    protected float igniteTimer;
    protected float igniteHitTimer;
    protected bool isChilled;
    protected float chilledTimer;
    protected bool isVulnerable;
    protected bool isInvisible;
    protected float vulnerableRate;
    protected float currentHealth;
    protected float currentStamina;
    protected bool isCriticalAttack;
    private Coroutine rechargeCoroutine;
    #endregion

    public event EventHandler OnInitHealth;
    public event EventHandler OnHealthChange;
    public event EventHandler OnInitStamina;
    public event EventHandler OnStaminaChange;

    protected virtual void Start()
    {
        entity = GetComponent<Entity>();
        fx = GetComponent<EntityFX>();

        InitStats();
    }

    protected virtual void Update()
    {
        igniteTimer -= Time.deltaTime;
        igniteHitTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;

        if (igniteTimer < 0)
        {
            isIgnite = false;
        }

        if (isIgnite)
        {
            DoIgniteDamage();
        }

        if (chilledTimer < 0)
        {
            isChilled = false;
        }
    }

    #region Vulnerable
    /// <summary>
    /// Handles to make character vulnerable.
    /// </summary>
    /// <param name="_vulnerableDuration"></param>
    /// <param name="_vulnerableRate"></param>
    public void MakeVulnerable(float _vulnerableDuration, float _vulnerableRate)
    {
        StartCoroutine(ApplyVulnerableRoutine(_vulnerableDuration, _vulnerableRate));
    }

    /// <summary>
    /// Handles to apply vulnerable.
    /// </summary>
    /// <param name="_vulnerableDuration"></param>
    /// <param name="_vulnerableRate"></param>
    /// <returns></returns>
    private IEnumerator ApplyVulnerableRoutine(float _vulnerableDuration, float _vulnerableRate)
    {
        isVulnerable = true;
        vulnerableRate = _vulnerableRate;
        yield return new WaitForSeconds(_vulnerableDuration);
        isVulnerable = false;
        vulnerableRate = 0;
    }

    public void MarkInvisible(bool _isInvisible) => isInvisible = _isInvisible;
    #endregion

    /// <summary>
    /// Handles to make physical damage.
    /// </summary>
    /// <param name="_targetStats"></param>
    public virtual void DoPhysicalDamage(EntityStats _targetStats, float _damage = 0)
    {
        if (_targetStats == null || _targetStats.entity.IsDead || CanTargetEvadeAttack(_targetStats) || _targetStats.isInvisible)
        {
            PlayMissAttackSound();
            fx.PlayPopupMissDamageText();
            return;
        }

        float totalDamage = _damage == 0 ? physicsDamage.GetValueWithModify() : _damage;
        bool canCrit = CanDoCritDamage();
        isCriticalAttack = canCrit;
        if (canCrit)
        {
            totalDamage = CalculateCritDamage(totalDamage);
        }

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(transform, totalDamage, canCrit);
        PlayHitAttackSound();
    }

    /// <summary>
    /// Handles to set up target's be taken damage.
    /// </summary>
    /// <param name="_damageDealer"></param>
    /// <param name="_damage"></param>
    /// <param name="_isCriticalAttack"></param>
    public virtual void TakeDamage(Transform _damageDealer, float _damage, bool _isCriticalAttack)
    {
        if (isInvisible) return;

        entity.SetupKnockBack(_damageDealer, _isCriticalAttack);
        if (entity.IsBlocking && !_isCriticalAttack)
        {
            if (entity.TryGetComponent(out PlayerStats playerStats))
            {
                playerStats.DecreaseStamina(_damage);
            }

            return;
        }

        fx.PlayHitFX(_isCriticalAttack);
        fx.PlayFlashFX();
        fx.PlayPopupDamageText(Mathf.Round(_damage).ToString(), _isCriticalAttack);
        DecreaseHealth(_damage);
    }

    #region Health ans Stamina
    /// <summary>
    /// Handles to decrease health.
    /// </summary>
    /// <param name="_damage"></param>
    public virtual void DecreaseHealth(float _damage)
    {
        if (entity.IsDead) return;

        if (isVulnerable && vulnerableRate > 0)
        {
            _damage = Mathf.RoundToInt(_damage * vulnerableRate);
        }

        currentHealth -= _damage;
        OnHealthChange?.Invoke(this, EventArgs.Empty);

        if (currentHealth <= 0)
        {
            entity.SetupDeath();
        }
    }

    /// <summary>
    /// Handles to increase health.
    /// </summary>
    /// <param name="_healPoint"></param>
    public void IncreaseHealth(int _healPoint)
    {
        if (_healPoint <= 0) return;

        currentHealth += _healPoint;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth.GetValueWithModify());
        OnHealthChange?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Handles to decrease stamina.
    /// </summary>
    /// <param name="_staminaAmount"></param>
    public void DecreaseStamina(float _staminaAmount)
    {
        currentStamina -= _staminaAmount;
        OnStaminaChange?.Invoke(this, EventArgs.Empty);

        if (rechargeCoroutine != null)
        {
            StopCoroutine(rechargeCoroutine);
        }
        rechargeCoroutine = StartCoroutine(RechargeStaminaRoutine());

    }

    /// <summary>
    /// Handles to recharge stamina.
    /// </summary>
    /// <returns></returns>
    private IEnumerator RechargeStaminaRoutine()
    {
        yield return new WaitForSeconds(rechargeDelay);

        while (currentStamina < maxStamina.GetValueWithModify())
        {
            currentStamina += rechargeRate * Time.deltaTime;
            if (currentStamina > maxStamina.GetValueWithModify())
            {
                currentStamina = maxStamina.GetValueWithModify();
            }
            OnStaminaChange?.Invoke(this, EventArgs.Empty);

            yield return null;
        }

        rechargeCoroutine = null;
    }
    #endregion

    #region Magic damage
    /// <summary>
    /// Handle to make magic damage.
    /// </summary>
    /// <param name="_targetStats"></param>
    /// <param name="_type"></param>
    public virtual void DoMagicDamage(EntityStats _targetStats, AilmentType _type, float damageAdd = 0)
    {
        if (_targetStats == null || _targetStats.entity.IsDead || CanTargetEvadeAttack(_targetStats)) return;

        float totalDamage = damageAdd == 0 ? magicDamage.GetValueWithModify() : damageAdd;
        totalDamage = CheckTargetResistance(_targetStats, totalDamage);
        _targetStats.TakeDamage(transform, totalDamage, false);
        _targetStats.ApplyAilement(_type);

        if (_type == AilmentType.Ignite)
        {
            _targetStats.SetIgniteDamage(igniteDamage);
        }
    }

    /// <summary>
    /// Handles to apply ailment.
    /// </summary>
    /// <param name="_type"></param>
    protected void ApplyAilement(AilmentType _type)
    {
        if (_type == AilmentType.Ignite && !isIgnite)
        {
            isIgnite = true;
            igniteTimer = ignitedDuration;
            fx.PlayIgnitedFX(ignitedDuration);
        }
        else if (_type == AilmentType.Chill && !isChilled)
        {
            isChilled = true;
            chilledTimer = chilledDuration;
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
            DecreaseHealth(igniteDamage.GetValueWithModify());
        }
    }
    #endregion

    #region Stats
    /// <summary>
    /// Handles to calculate stat modify.
    /// </summary>
    public float CalculateStatModify(StatType _type, float _point, bool _isInitStat = true)
    {
        float maxHealthAdjust = 20;
        float maxStaminaAdjust = 10;
        float physicsDamageAdjust = 5;
        float cirtPowerAdjust = .5f;
        float magicDamageAdjust = 4;
        float resistanceAdjust = .5f;
        float statAdjustDecreasePerct;
        float maxHealthAdded = 0;
        float maxStaminaAdded = 0;
        float physicsDamageAdded = 0;
        float magicDamageAdded = 0;


        switch (_type)
        {
            case StatType.MaxHealth:
                {
                    if (_isInitStat)
                    {
                        _point = 0;
                    }

                    statAdjustDecreasePerct = .4f;
                    float maxHealthPoint = vitality.GetValueWithModify() + _point;
                    maxHealthAdded = CalculateStatAdd(maxHealthPoint, maxHealthAdjust, statAdjustDecreasePerct);
                    if (_isInitStat)
                    {
                        maxHealthAdded += maxHealth.GetValueWithModify();
                    }
                    else
                    {
                        maxHealthAdded += maxHealth.DefaultValue;
                    }
                    break;
                }
            case StatType.Stamina:
                {
                    if (_isInitStat)
                    {
                        _point = 0;
                    }

                    statAdjustDecreasePerct = .4f;
                    float maxStaminaPoint = endurance.GetValueWithModify() + _point;
                    maxStaminaAdded = CalculateStatAdd(maxStaminaPoint, maxStaminaAdjust, statAdjustDecreasePerct);
                    if (_isInitStat)
                    {
                        maxStaminaAdded += maxStamina.GetValueWithModify();
                    }
                    else
                    {
                        maxStaminaAdded += maxStamina.DefaultValue;
                    }
                    break;
                }
            case StatType.PhysicsDamage:
                {
                    if (_isInitStat)
                    {
                        _point = 0;
                    }

                    statAdjustDecreasePerct = .2f;
                    float physicsDamagePoint = strength.GetValueWithModify() + _point;
                    physicsDamageAdded = CalculateStatAdd(physicsDamagePoint, physicsDamageAdjust, statAdjustDecreasePerct);
                    if (_isInitStat)
                    {
                        physicsDamageAdded += physicsDamage.GetValueWithModify();
                    }
                    else
                    {
                        physicsDamageAdded += physicsDamage.DefaultValue;
                    }
                    break;
                }
            case StatType.MagicDamage:
                {
                    if (_isInitStat)
                    {
                        _point = 0;
                    }

                    statAdjustDecreasePerct = .1f;
                    float magicDamagePoint = intelligence.GetValueWithModify() + _point;
                    magicDamageAdded = CalculateStatAdd(magicDamagePoint, magicDamageAdjust, statAdjustDecreasePerct);    
                    if (_isInitStat)
                    {
                        magicDamageAdded += magicDamage.GetValueWithModify();
                    }
                    else
                    {
                        magicDamageAdded += magicDamage.DefaultValue;
                    }
                    break;
                }
            default: break;
        }

        return _type switch
        {
            StatType.Vitality => vitality.GetValueWithModify() + _point,
            StatType.Endurance => endurance.GetValueWithModify() + _point,
            StatType.Strength => strength.GetValueWithModify() + _point,
            StatType.Dexterity => dexterity.GetValueWithModify() + _point,
            StatType.Intelligence => intelligence.GetValueWithModify() + _point,
            StatType.Agility => agility.GetValueWithModify() + _point,
            StatType.MaxHealth => maxHealthAdded,
            StatType.Stamina => maxStaminaAdded,
            StatType.PhysicsDamage => physicsDamageAdded,
            StatType.CritChance => critChance.GetValueWithModify() + _point,
            StatType.CritPower => critPower.GetValueWithModify() + _point * cirtPowerAdjust,
            StatType.MagicDamage => magicDamageAdded,
            StatType.Evasion => evasion.GetValueWithModify() + _point,
            StatType.Armor => armor.GetValueWithModify(),
            StatType.Resistance => resistance.GetValueWithModify() + _point * resistanceAdjust,
            _ => 0
        };
    }

    /// <summary>
    /// Handles to caclulate stats add.
    /// </summary>
    /// <param name="_statPoint"></param>
    /// <param name="_statAdjust"></param>
    /// <param name="_statAdjustDecreasePerct"></param>
    /// <returns></returns>
    private float CalculateStatAdd(float _statPoint, float _statAdjust, float _statAdjustDecreasePerct)
    {
        float thresholdPoint = 10;
        float minStatAdjust = 1;
        float statAdded = 0;

        while (_statPoint > 0)
        {
            float point = Mathf.Min(_statPoint, thresholdPoint);
            statAdded += Mathf.Round(point * _statAdjust);
            _statAdjust -= _statAdjust * _statAdjustDecreasePerct;
            _statAdjust = Mathf.Clamp(_statAdjust, minStatAdjust, float.MaxValue);
            _statPoint -= point;
        }

        return statAdded;
    }

    /// <summary>
    /// Handles to init character's stats.
    /// </summary>
    public virtual void InitStats()
    {
        currentHealth = CalculateStatModify(StatType.MaxHealth, vitality.GetValueWithModify());
        maxHealth.UpdateBaseValue(currentHealth);
        OnInitHealth?.Invoke(this, EventArgs.Empty);

        currentStamina = CalculateStatModify(StatType.Stamina, endurance.GetValueWithModify());
        maxStamina.UpdateBaseValue(currentStamina);
        OnInitStamina?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Handles to check target can evade. Maximum evasion chance is 50.
    /// </summary>
    /// <param name="_targetStats"></param>
    /// <returns>True if can evade. False if not.</returns>
    protected bool CanTargetEvadeAttack(EntityStats _targetStats)
    {
        if (isVulnerable) return false;

        float totalEvasionChance = _targetStats.evasion.GetValueWithModify();
        int maxEvasionChance = 50;

        totalEvasionChance = Mathf.Clamp(totalEvasionChance, 0, maxEvasionChance);

        return Utils.RandomChance(totalEvasionChance);
    }

    /// <summary>
    /// Handles to check can make critical damage. Maximum critical chance is 50.
    /// </summary>
    /// <returns>True if can do critical. False if not.</returns>
    protected bool CanDoCritDamage()
    {
        float totalCritChance = critChance.GetValueWithModify();
        int maxCritChance = 50;

        totalCritChance = Mathf.Clamp(totalCritChance, 0, maxCritChance);

        return Utils.RandomChance(totalCritChance);
    }

    /// <summary>
    /// Handles to calculate critical damage.
    /// </summary>
    /// <param name="_totalDamage"></param>
    /// <returns>Critical damage.</returns>
    protected float CalculateCritDamage(float _totalDamage)
    {
        _totalDamage += _totalDamage * critPower.GetValueWithModify() / 100;

        return _totalDamage;
    }

    /// <summary>
    /// Handles to calculate damage vs armor
    /// </summary>
    /// <param name="_targetStats"></param>
    /// <param name="_totalDamage"></param>
    /// <returns>Final damage.</returns>
    protected float CheckTargetArmor(EntityStats _targetStats, float _totalDamage)
    {
        _totalDamage -= _targetStats.armor.GetValueWithModify();
        _totalDamage = Mathf.Clamp(_totalDamage, 0, int.MaxValue);

        return _totalDamage;
    }

    /// <summary>
    /// Handles to calculate magic damage vs resistance.
    /// </summary>
    /// <param name="_targetStats"></param>
    /// <param name="_totalDamage"></param>
    /// <returns>Final damage.</returns>
    protected float CheckTargetResistance(EntityStats _targetStats, float _totalDamage)
    {
        _totalDamage -= _targetStats.resistance.GetValueWithModify();
        _totalDamage = Mathf.Clamp(_totalDamage, 0, int.MaxValue);

        return _totalDamage;
    }

    /// <summary>
    /// Handles to buff stat of the character.
    /// </summary>
    /// <param name="_statToBuff"></param>
    /// <param name="_modify"></param>
    /// <param name="_duration"></param>
    public void BuffStat(Stat _statToBuff, int _modify, float _duration)
    {
        if (_statToBuff == null) return;

        StartCoroutine(BuffStatRoutine(_statToBuff, _modify, _duration));
    }

    /// <summary>
    /// Handles to buff stat of the character.
    /// </summary>
    /// <param name="_statToBuff"></param>
    /// <param name="_modify"></param>
    /// <param name="_duration"></param>
    /// <returns></returns>
    private IEnumerator BuffStatRoutine(Stat _statToBuff, int _modify, float _duration)
    {
        if (_statToBuff != null)
        {
            _statToBuff.AddModify(_modify);
            yield return new WaitForSeconds(_duration);
            _statToBuff.RemoveModify(_modify);

            InventoryManager.Instance.UpdateStatUIs();
        }
        else
        {
            yield return new WaitForSeconds(0);
        }
    }

    /// <summary>
    /// Handles to get stat by type.
    /// </summary>
    /// <param name="_statType"></param>
    /// <returns></returns>
    public Stat GetStatByType(StatType _statType)
    {
        return _statType switch
        {
            StatType.Vitality => vitality,
            StatType.Endurance => endurance,
            StatType.Strength => strength,
            StatType.Dexterity => dexterity,
            StatType.Intelligence => intelligence,
            StatType.Agility => agility,
            StatType.MaxHealth => maxHealth,
            StatType.Stamina => maxStamina,
            StatType.PhysicsDamage => physicsDamage,
            StatType.CritChance => critChance,
            StatType.CritPower => critPower,
            StatType.MagicDamage => magicDamage,
            StatType.Evasion => evasion,
            StatType.Armor => armor,
            StatType.Resistance => resistance,
            _ => null,
        };
    }
    #endregion

    #region Play sound
    /// <summary>
    /// Handles to play miss attack sound.
    /// </summary>
    protected void PlayMissAttackSound()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayMissAttackSound(transform.position);
        }
    }

    /// <summary>
    /// Handles to play hit attach sound.
    /// </summary>
    protected void PlayHitAttackSound()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayHitAttackSound(transform.position);
        }
    }
    #endregion

    public float CurrentHealth
    {
        get { return currentHealth; }
    }

    public float CurrentStamina
    {
        get { return currentStamina; }
    }

    public Entity Entity
    {
        get { return entity; }
    }

    public bool IsInvisible
    {
        get { return isInvisible; }
    }

    public bool IsCriticalAttack
    {
        get { return isCriticalAttack; }
    }
}
