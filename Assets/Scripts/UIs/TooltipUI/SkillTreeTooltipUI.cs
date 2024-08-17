using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillTreeTooltipUI : TooltipUI
{
    [Header("Skill tooltip info")]
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDes;
    [SerializeField] private TextMeshProUGUI skillPrice;

    /// <summary>
    /// Handles to show and set skill tree tooltip info.
    /// </summary>
    /// <param name="_skillName"></param>
    /// <param name="_skillDes"></param>
    /// <param name="_skillPrice"></param>
    public void ShowSkillTreeTooltip(string _skillName, string _skillDes, int _skillPrice)
    {
        skillName.text = _skillName;
        skillDes.text = _skillDes;
        skillPrice.text = _skillPrice.ToString();
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Handles to hide and clear skill tree tooltip info.
    /// </summary>
    public void HideSkillTreeTooltip()
    {
        skillName.text = "";
        skillDes.text = "";
        skillPrice.text = "";
        transform.position = defaultPos;
        gameObject.SetActive(false);
    }
}
