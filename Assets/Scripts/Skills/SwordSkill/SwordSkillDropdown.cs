using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class SwordSkillDropdown : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown swordSkillDropdown;

    private Dictionary<int, SwordType> swordSkillDictionaries;
    private List<string> swordSkillOptions;
    private int currentIndex;

    private void Awake()
    {
        swordSkillDropdown.onValueChanged.AddListener(UpdateSwordSkill);
    }

    private void Start()
    {
        swordSkillDropdown.ClearOptions();
        swordSkillDictionaries = new Dictionary<int, SwordType>();
        swordSkillOptions = new List<string>();
        swordSkillDropdown.enabled = false;
    }

    /// <summary>
    /// Handles to add option when sword skill unlocked.
    /// </summary>
    /// <param name="_swordType"></param>
    public void AddOption(SwordType _swordType)
    {
        string option = SetOption(_swordType);
        swordSkillOptions.Add(option);
        swordSkillDropdown.ClearOptions();
        swordSkillDropdown.enabled = true;
        swordSkillDropdown.AddOptions(swordSkillOptions);
        swordSkillDropdown.RefreshShownValue();
        swordSkillDropdown.value = currentIndex;
    }

    /// <summary>
    /// Handles to add sword type.
    /// </summary>
    /// <param name="_swordType"></param>
    /// <returns></returns>
    private string SetOption(SwordType _swordType)
    {
        int id = swordSkillDictionaries.Count;
        switch (_swordType)
        {
            default:
            case SwordType.Regular:
                {
                    swordSkillDictionaries.Add(id, SwordType.Regular);
                    return "Regular sword";
                }
            case SwordType.Pierce:
                {
                    swordSkillDictionaries.Add(id, SwordType.Pierce);
                    return "Pierce sword";
                }
            case SwordType.Bounce:
                {
                    swordSkillDictionaries.Add(id, SwordType.Bounce);
                    return "Bounce sword";
                }
            case SwordType.Spin:
                {
                    swordSkillDictionaries.Add(id, SwordType.Spin);
                    return "Spin sword";
                }

        }
    }

    /// <summary>
    /// Handles to update sword skill when change sword.
    /// </summary>
    /// <param name="_index"></param>
    private void UpdateSwordSkill(int _index)
    {
        if (swordSkillDictionaries.TryGetValue(_index, out SwordType swordType))
        {
            currentIndex = _index;
            SkillManager.Instance.SwordSkill.UpdateSwordSkill(swordType);
        }

    }
}
