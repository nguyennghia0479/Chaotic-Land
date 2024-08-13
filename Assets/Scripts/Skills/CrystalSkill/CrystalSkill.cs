using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSkill : Skill
{
    #region Variables
    [SerializeField] private GameObject crystalPrefab;
    [SerializeField] private float lifeTime = 3;

    [Header("Aggressive crystal info")]
    [SerializeField] private bool canMove;
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float checkRadius = 10;
    [SerializeField] private LayerMask enemyLayerMask;

    [Header("Multiple crystal info")]
    [SerializeField] private bool canUseMultiCrystal;
    [SerializeField] private int numberOfCrystal = 3;
    [SerializeField] private float multiCrystalCooldown = 4;
    [SerializeField] private float useTimeCooldown = 3;

    private List<GameObject> crystalLeft;
    #endregion

    public event EventHandler OnResetSkill;

    protected override void Start()
    {
        base.Start();

        if (canUseMultiCrystal)
        {
            crystalLeft = new List<GameObject>();
            RefillCrystal();
        }
    }

    /// <summary>
    /// Handles to use skill of crystal.
    /// </summary>
    public override void UseSkill()
    {
        base.UseSkill();

        if (CanUseMultiCrystal()) return;

        CreateCrysal();
    }

    #region Crystal skill
    /// <summary>
    /// Handles to create crystal.
    /// </summary>
    private void CreateCrysal()
    {
        Vector3 offset = new(1 * player.FacingDir, 0);
        GameObject newCrystal = Instantiate(crystalPrefab, player.transform.position + offset, Quaternion.identity);
        newCrystal.GetComponent<CrystalSkillController>().SetupCrystal(this);
    }

    /// <summary>
    /// Handles to spell cast multi crystal.
    /// </summary>
    /// <returns>True if can use multi crystal and have crystals. False if not.</returns>
    /// <remarks>
    /// If not spell over crystal will reset ability by use time cooldown.
    /// </remarks>
    private bool CanUseMultiCrystal()
    {
        if (canUseMultiCrystal && crystalLeft.Count > 0)
        {
            if (crystalLeft.Count == numberOfCrystal)
            {
                Invoke(nameof(ResetAbility), useTimeCooldown);
            }

            CreateCrysal();
            cooldown = 0;
            crystalLeft.Remove(crystalLeft[^1]);

            if (crystalLeft.Count == 0)
            {
                RefillCrystal();
                cooldown = multiCrystalCooldown;
            }

            return true;
        }

        return false;
    }

    /// <summary>
    /// Handles to reset ability.
    /// </summary>
    /// <remarks>
    /// If cooldown is greater than zero will refill crystal and set cooldown skill.
    /// </remarks>
    private void ResetAbility()
    {
        if (cooldown > 0) return;

        RefillCrystal();
        cooldown = multiCrystalCooldown;
        cooldownTimer = multiCrystalCooldown;
        OnResetSkill?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Handles to refill crystal.
    /// </summary>
    private void RefillCrystal()
    {
        int numberOfAdd = numberOfCrystal - crystalLeft.Count;
        for (int i = 0; i < numberOfAdd; i++)
        {
            crystalLeft.Add(crystalPrefab);
        }
    }
    #endregion

    #region Getters
    public float LifeTime
    {
        get { return lifeTime; }
    }

    public bool CanMove
    {
        get { return canMove; }
    }

    public float MoveSpeed
    {
        get { return moveSpeed; }
    }

    public float CheckRadius
    {
        get { return checkRadius; }
    }

    public LayerMask EnemyLayerMask
    {
        get { return enemyLayerMask; }
    }
    #endregion
}
