using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillTreeUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string skillName;
    [SerializeField][TextArea] private string skillDes;
    [SerializeField] private int skillCooldown;
    [SerializeField] private int skillUnlockedValue;
    [SerializeField] private Image imgBG;
    [SerializeField] private SkillTreeUI[] requiredSkills;
    [SerializeField] private SkillTreeUI[] optionalSkills;
    [SerializeField] private SkillTreeTooltipUI skillTreeTooltip;

    private bool isUnlocked;
    private bool isHolding;
    private float unlockSkillTimer;
    private readonly float unlockSkillTime = 3;

    public event EventHandler OnUnlocked;

    private void Start()
    {
        SetupEventTriggers();
    }

    private void Update()
    {
        if (isHolding)
        {
            float timer = Time.unscaledTime - unlockSkillTimer;
            imgBG.fillAmount = 1 - (timer / unlockSkillTime);
            if (timer > unlockSkillTime)
            {
                UnlockedSkill();
            }
        }
    }

    /// <summary>
    /// Handles to setup event triggers.
    /// </summary>
    private void SetupEventTriggers()
    {
        EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry downEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerDown
        };
        downEntry.callback.AddListener((data) => HoldingButton());
        eventTrigger.triggers.Add(downEntry);

        EventTrigger.Entry upEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerUp
        };
        upEntry.callback.AddListener((data) => ReleaseButton());
        eventTrigger.triggers.Add(upEntry);
    }

    /// <summary>
    /// Handles holding to unlock skill.
    /// </summary>
    private void HoldingButton()
    {
        if (isUnlocked || !CanUnlockedSkill() || skillUnlockedValue > PlayerManager.Instance.CurrentExp) return;

        isHolding = true;
        unlockSkillTimer = Time.unscaledTime;
    }

    /// <summary>
    /// Handles to reset when released button.
    /// </summary>
    private void ReleaseButton()
    {
        if (isUnlocked) return;

        imgBG.fillAmount = 1;
        isHolding = false;
    }

    /// <summary>
    /// Handles to check skill can unlock.
    /// </summary>
    /// <returns></returns>
    private bool CanUnlockedSkill()
    {
        if (requiredSkills.Length == 0 && optionalSkills.Length == 0) return true;

        for (int i = 0; i < requiredSkills.Length; i++)
        {
            if (!requiredSkills[i].isUnlocked)
                return false;
        }

        if (optionalSkills.Length > 0)
        {
            for (int i = 0; i < optionalSkills.Length; i++)
            {
                if (optionalSkills[i].isUnlocked)
                    return true;
            }

            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// Handles to unlocked skill.
    /// </summary>
    private void UnlockedSkill()
    {
        imgBG.color = Color.clear;
        isUnlocked = true;
        isHolding = false;
        PlayerManager.Instance.SkillUnlocked(skillUnlockedValue);
        OnUnlocked?.Invoke(this, EventArgs.Empty);
        skillTreeTooltip.ShowSkillTreeTooltip(skillName, skillDes, skillCooldown, skillUnlockedValue, isUnlocked, CanUnlockedSkill());
    }

    /// <summary>
    /// Handles to show skill tree tooltip.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        skillTreeTooltip.ShowSkillTreeTooltip(skillName, skillDes, skillCooldown, skillUnlockedValue, isUnlocked, CanUnlockedSkill());
    }

    /// <summary>
    /// Handles to hide skill tree tooltip.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        skillTreeTooltip.HideSkillTreeTooltip();
    }

    public bool IsUnlocked
    {
        get { return isUnlocked; }
    }
}
