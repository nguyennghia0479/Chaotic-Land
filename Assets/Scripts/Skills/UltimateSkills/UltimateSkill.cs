using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UltimateType
{
    FireSpin, IceDrill
}

public class UltimateSkill : Skill
{
    private UltimateType type;
    private bool isUltimateUnlocked;

    public override bool CanUseSkill()
    {
        if (!isUltimateUnlocked) return false;

        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        if (type == UltimateType.FireSpin)
        {
            SkillManager.Instance.FireSpinSkill.UseSkill();
        }
        else
        {
            SkillManager.Instance.IceDrillSkill.UseSkill();
        }
    }

    /// <summary>
    /// Handles to create ultimate skill.
    /// </summary>
    public void PerformSkill()
    {
        if (player.SkillManager.UltimateSkill.Type == UltimateType.FireSpin)
        {
            player.SkillManager.FireSpinSkill.CreateFireSpin();
        }
        else
        {
            player.SkillManager.IceDrillSkill.CreateIceDrill();
        }
    }

    /// <summary>
    /// Handles to setup ultiamte skill info.
    /// </summary>
    /// <param name="_type"></param>
    /// <param name="_isUltimateUnlocked"></param>
    /// <param name="_cooldown"></param>
    /// <param name="_skillStaminaAmount"></param>
    public void UpdateUltimateSkillInfo(UltimateType _type, bool _isUltimateUnlocked, float _cooldown, int _skillStaminaAmount, bool _isSkillReseted)
    {
        type = _type;
        isUltimateUnlocked = _isUltimateUnlocked;
        skillStaminaAmount = _skillStaminaAmount;
        cooldown = _cooldown;
        if (_isSkillReseted)
        {
            cooldownTimer = _cooldown;
            GameManager.Instance.InGameUI.UltimateImg.fillAmount = 1;
        }
    }

    /// <summary>
    /// Handles to setup ultimate skill when unlocked.
    /// </summary>
    /// <param name="_type"></param>
    /// <param name="_isUltimateUnlocked"></param>
    /// <param name="_cooldown"></param>
    /// <param name="_skillStaminaAmount"></param>
    public void UnlockUltiamteSkill(UltimateType _type, bool _isUltimateUnlocked, float _cooldown, int _skillStaminaAmount)
    {
        if (!isUltimateUnlocked)
        {
            type = _type;
            isUltimateUnlocked = _isUltimateUnlocked;
            cooldown = _cooldown;
            skillStaminaAmount = _skillStaminaAmount;
            cooldownTimer = 0;
            GameManager.Instance.InGameUI.UltimateImg.fillAmount = 0;
        }
    }

    public UltimateType Type
    {
        get { return type; }
    }

    public bool IsUltimateUnlocked
    {
        get { return isUltimateUnlocked; }
    }
}
