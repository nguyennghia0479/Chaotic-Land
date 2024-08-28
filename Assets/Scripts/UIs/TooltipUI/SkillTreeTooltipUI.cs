using TMPro;
using UnityEngine;

public class SkillTreeTooltipUI : TooltipUI
{
    [Header("Skill tooltip info")]
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDes;
    [SerializeField] private TextMeshProUGUI skillCooldown;
    [SerializeField] private TextMeshProUGUI skillUnlockedValue;
    [SerializeField] private Color unlockedColor;
    [SerializeField] private Color lockedColor;
    [SerializeField] private Color defaultColor;

    /// <summary>
    /// Handles to show and set skill tree tooltip info.
    /// </summary>
    /// <param name="_skillName"></param>
    /// <param name="_skillDes"></param>
    /// <param name="_skillUnlockedValue"></param>
    public void ShowSkillTreeTooltip(string _skillName, string _skillDes, int _skillCooldown, int _skillUnlockedValue, bool _isUnlocked, bool _canUnlockSkill)
    {
        skillName.text = _skillName;
        skillDes.text = _skillDes;
        skillCooldown.text = _skillCooldown.ToString() + "s";

        if (_isUnlocked)
        {
            skillUnlockedValue.text = "Unlocked";
            skillUnlockedValue.color = unlockedColor;
        }
        else
        {
            if (_canUnlockSkill)
            {
                if (_skillUnlockedValue <= PlayerManager.Instance.CurrentExp)
                {
                    skillUnlockedValue.color = defaultColor;
                }
                else
                {
                    skillUnlockedValue.color = lockedColor;
                }
                skillUnlockedValue.text = _skillUnlockedValue.ToString() + " EXP";

            }
            else
            {
                skillUnlockedValue.text = "Can't unlocked";
                skillUnlockedValue.color = lockedColor;
            }
        }

        gameObject.SetActive(true);
    }

    /// <summary>
    /// Handles to hide and clear skill tree tooltip info.
    /// </summary>
    public void HideSkillTreeTooltip()
    {
        skillName.text = "";
        skillDes.text = "";
        skillCooldown.text = "";
        skillUnlockedValue.text = "";
        transform.position = defaultPos;
        gameObject.SetActive(false);
    }
}
