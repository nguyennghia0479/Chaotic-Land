using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceDrillSkill : Skill
{
    [Header("Ice drill skill info")]
    [SerializeField] private UltimateSkillDropdown ultimateSkillDropdown;
    [SerializeField] private GameObject iceDrillPrefab;
    [SerializeField] private int moveSpeed = 10;
    [SerializeField] private float radius = 10;
    [SerializeField] private float yOffset = 5;
    [SerializeField] private float freezingDuration = 4;
    [SerializeField] private LayerMask enemyLayerMask;
    [Header("Skill unlocked")]
    [SerializeField] private SkillTreeUI iceDrillSkill;

    private bool isIceDrillUnlocked;
    private List<Transform> targets;

    #region Skill unlocked
    private void OnEnable()
    {
        if (iceDrillSkill != null)
        {
            iceDrillSkill.OnUnlocked += IceDrillSkill_OnUnlocked;
        }
    }

    private void OnDestroy()
    {
        if (iceDrillSkill != null)
        {
            iceDrillSkill.OnUnlocked -= IceDrillSkill_OnUnlocked;
        }
    }

    private void IceDrillSkill_OnUnlocked(object sender, System.EventArgs e)
    {
        UnlockedIceDrillSkill();
    }

    /// <summary>
    /// Handles to unlocked ice drill skill.
    /// </summary>
    public void UnlockedIceDrillSkill()
    {
        if (SkillManager.Instance == null || SkillManager.Instance.UltimateSkill == null) return;

        if (iceDrillSkill != null && iceDrillSkill.IsUnlocked)
        {
            isIceDrillUnlocked = true;
            SkillManager.Instance.UltimateSkill.UnlockUltiamteSkill(UltimateType.IceDrill, isIceDrillUnlocked, cooldown, skillStaminaAmount);
            ultimateSkillDropdown.AddOption(UltimateType.IceDrill);
        }
    }
    #endregion

    protected override void Start()
    {
        base.Start();

        UnlockedIceDrillSkill();
    }

    public override bool CanUseSkill()
    {
        if (!isIceDrillUnlocked) return false;

        return base.CanUseSkill();
    }

    /// <summary>
    /// Handles to determined target before create ice drill.
    /// </summary>
    public override void UseSkill()
    {
        targets = new List<Transform>();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.transform.position, radius, enemyLayerMask);

        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out Enemy enemy))
            {
                if (enemy.IsDead) continue;

                targets.Add(enemy.transform);
            }
        }
    }

    /// <summary>
    /// Handles to create ice drill.
    /// </summary>
    public void CreateIceDrill()
    {
        foreach (Transform target in targets)
        {
            Vector2 offset = new(target.position.x, target.position.y + yOffset);
            GameObject newIceDrill = Instantiate(iceDrillPrefab, offset, Quaternion.Euler(0, 0, -90));
            newIceDrill.GetComponentInChildren<IceDrillController>().SetupIceDrill(this, target);
        }
    }

    #region Getter
    public int MoveSpeed
    {
        get { return moveSpeed; }
    }

    public float FreezingDuration
    {
        get { return freezingDuration; }
    }

    public bool IsIceDrillUnlocked
    {
        get { return isIceDrillUnlocked; }
    }
    #endregion
}
