using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpinSkill : Skill
{
    [Header("Fire spin skill info")]
    [SerializeField] private UltimateSkillDropdown ultimateSkillDropdown;
    [SerializeField] private GameObject fireSpinPrefab;
    [SerializeField] private float moveTime = 3;
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float maxGrowSize = 5;
    [SerializeField] private float scaleSpeed = 2;
    [SerializeField] private float spinHitCooldown = .5f;
    [SerializeField] private float yOffset = 2;
    [Header("Skill unlocked")]
    [SerializeField] private SkillTreeUI fireSpinSkill;

    private bool isFireSpinUnlocked;

    #region Skill unlocked
    private void OnEnable()
    {
        if (fireSpinSkill != null)
        {
            fireSpinSkill.OnUnlocked += FireSpinSkill_OnUnlocked;
        }
    }

    private void OnDestroy()
    {
        if (fireSpinSkill != null)
        {
            fireSpinSkill.OnUnlocked -= FireSpinSkill_OnUnlocked;
        }
    }

    private void FireSpinSkill_OnUnlocked(object sender, System.EventArgs e)
    {
        UnlockedFireSpinSkill();
    }

    /// <summary>
    /// Handles to unlocked fire spin skill.
    /// </summary>
    public void UnlockedFireSpinSkill()
    {
        if (SkillManager.Instance == null || SkillManager.Instance.UltimateSkill == null) return;

        if (fireSpinSkill != null && fireSpinSkill.IsUnlocked)
        {
            isFireSpinUnlocked = true;
            SkillManager.Instance.UltimateSkill.UnlockUltiamteSkill(UltimateType.FireSpin, isFireSpinUnlocked, cooldown, skillStaminaAmount);
            ultimateSkillDropdown.AddOption(UltimateType.FireSpin);
        }

    }
    #endregion

    protected override void Start()
    {
        base.Start();

        UnlockedFireSpinSkill();
    }

    public override bool CanUseSkill()
    {
        if (!isFireSpinUnlocked) return false;

        return base.CanUseSkill();
    }

    /// <summary>
    /// Handles to create fire spin.
    /// </summary>
    public void CreateFireSpin()
    {
        Vector3 fireSpinPos = new(player.transform.position.x, player.transform.position.y + yOffset);

        GameObject newFireSpin = Instantiate(fireSpinPrefab, fireSpinPos, Quaternion.identity);
        newFireSpin.GetComponent<FireSpinSkillController>().SetupFireSpin(this);
        player.AssignFireSpin(newFireSpin);
    }

    #region Getter
    public float MoveTime
    {
        get { return moveTime; }
    }

    public float MoveSpeed
    {
        get { return moveSpeed; }
    }

    public float MaxGrowSize
    {
        get { return maxGrowSize; }
    }

    public float ScaleSpeed
    {
        get { return scaleSpeed; }
    }

    public float SpinHitCooldown
    {
        get { return spinHitCooldown; }
    }

    public bool IsFireSpinUnlocked
    {
        get { return isFireSpinUnlocked; }
    }
    #endregion
}
