using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashSkill : Skill
{
    [Header("Dash skill basic info")]
    [SerializeField] private int dashSpeed = 25;
    [SerializeField] private float dashDuration = .2f;
    [Header("Skills unlocked info")]
    [SerializeField] private SkillTreeUI dashSkill;
    [SerializeField] private SkillTreeUI cloneOnStartSkill;
    [SerializeField] private SkillTreeUI cloneOnArrivalSkill;

    private bool isDashUnlocked;
    private bool isCloneOnStartUnlocked;
    private bool isCloneOnArrivalUnlocked;

    #region Skills unlocked
    private void OnEnable()
    {
        if (dashSkill != null)
        {
            dashSkill.OnUnlocked += DashSkill_OnUnlocked;
        }

        if (cloneOnStartSkill != null)
        {
            cloneOnStartSkill.OnUnlocked += CloneOnStartSkill_OnUnlocked;
        }

        if (cloneOnArrivalSkill != null)
        {
            cloneOnArrivalSkill.OnUnlocked += CloneOnArrivalSkill_OnUnlocked;
        }
    }

    private void OnDestroy()
    {
        if (dashSkill != null)
        {
            dashSkill.OnUnlocked -= DashSkill_OnUnlocked;
        }

        if (cloneOnStartSkill != null)
        {
            cloneOnStartSkill.OnUnlocked -= CloneOnStartSkill_OnUnlocked;
        }

        if (cloneOnArrivalSkill != null)
        {
            cloneOnArrivalSkill.OnUnlocked -= CloneOnArrivalSkill_OnUnlocked;
        }
    }

    private void DashSkill_OnUnlocked(object sender, System.EventArgs e)
    {
        if (dashSkill != null && dashSkill.IsUnlocked)
        {
            isDashUnlocked = true;
            GameManager.Instance.InGameUI.DashSkillImg.fillAmount = 0;
        }
    }

    private void CloneOnStartSkill_OnUnlocked(object sender, System.EventArgs e)
    {
        if (cloneOnStartSkill != null && cloneOnStartSkill.IsUnlocked)
        {
            isCloneOnStartUnlocked = true;
        }
    }

    private void CloneOnArrivalSkill_OnUnlocked(object sender, System.EventArgs e)
    {
        if (cloneOnArrivalSkill != null && cloneOnArrivalSkill.IsUnlocked)
        {
            isCloneOnArrivalUnlocked = true;
        }
    }
    #endregion

    public override bool CanUseSkill()
    {
        if (!isDashUnlocked) return false;

        return base.CanUseSkill();
    }

    /// <summary>
    /// Handles to create clone on starting dash.
    /// </summary>
    public void CreateCloneOnDash()
    {
        SkillManager.Instance.CloneSkill.CreateClone(player.transform, Vector2.zero);
    }

    #region Getter
    public int DashSpeed
    {
        get { return dashSpeed; }
    }

    public float DashDuration
    {
        get { return dashDuration; }
    }

    public bool IsDashUnlocked
    {
        get { return isDashUnlocked; }
    }

    public bool IsCloneOnStart
    {
        get { return isCloneOnStartUnlocked; }
    }

    public bool IsCloneOnArrival
    {
        get { return isCloneOnArrivalUnlocked; }
    }
    #endregion
}
