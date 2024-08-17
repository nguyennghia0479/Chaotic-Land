using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillTreeUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string skillName;
    [SerializeField][TextArea] private string skillDes;
    [SerializeField] private int skillPrice;
    [SerializeField] private Button unlockBtn;
    [SerializeField] private Image imgBG;
    [SerializeField] private SkillTreeUI[] requiredSkills;
    [SerializeField] private SkillTreeTooltipUI skillTreeTooltip;

    private bool isUnlocked;

    private void Awake()
    {
        unlockBtn.onClick.AddListener(() =>
        {
            UnlockSkill();
        });
    }

    /// <summary>
    /// Handles to unlock skill.
    /// </summary>
    private void UnlockSkill()
    {
        if (isUnlocked) return;

        if (CanUnlockSkill())
        {
            isUnlocked = true;
            imgBG.color = Color.clear;
        }
    }

    /// <summary>
    /// Handles to check skill can unlock.
    /// </summary>
    /// <returns></returns>
    private bool CanUnlockSkill()
    {
        if (requiredSkills.Length == 0) return true;

        for (int i = 0; i < requiredSkills.Length; i++)
        {
            if (!requiredSkills[i].isUnlocked)
                return false;
        }

        return true;
    }

    /// <summary>
    /// Handles to show skill tree tooltip.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        skillTreeTooltip.ShowSkillTreeTooltip(skillName, skillDes, skillPrice);
    }

    /// <summary>
    /// Handles to hide skill tree tooltip.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        skillTreeTooltip.HideSkillTreeTooltip();
    }
}
