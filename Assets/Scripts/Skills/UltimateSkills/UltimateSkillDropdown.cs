using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UltimateSkillDropdown : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown ultimateDropdown;

    private Dictionary<int, UltimateType> ultimateDictionaries;
    private List<string> ultimateOptions;
    private int currentIndex;

    private void Awake()
    {
        ultimateDropdown.onValueChanged.AddListener(UpdateUltimateSkill);
    }

    private void Start()
    {
        ultimateDropdown.ClearOptions();
        ultimateDictionaries = new Dictionary<int, UltimateType>();
        ultimateOptions = new List<string>();
        ultimateDropdown.enabled = false;
    }

    /// <summary>
    /// Handles to add option when ultimate skill unlocked.
    /// </summary>
    /// <param name="_type"></param>
    public void AddOption(UltimateType _type)
    {
        string option = SetOption(_type);
        ultimateOptions.Add(option);
        ultimateDropdown.enabled = true;
        ultimateDropdown.ClearOptions();
        ultimateDropdown.AddOptions(ultimateOptions);
        ultimateDropdown.RefreshShownValue();
        ultimateDropdown.value = currentIndex;
    }

    /// <summary>
    /// Handles to add ultimate type.
    /// </summary>
    /// <param name="_type"></param>
    /// <returns></returns>
    private string SetOption(UltimateType _type)
    {
        int id = ultimateDictionaries.Count;
        switch (_type)
        {
            default:
            case UltimateType.FireSpin:
                {
                    ultimateDictionaries.Add(id, UltimateType.FireSpin);
                    return "Fire Spin";
                }
            case UltimateType.IceDrill:
                {
                    ultimateDictionaries.Add(id, UltimateType.IceDrill);
                    return "Ice Drill";
                }
        }
    }

    /// <summary>
    /// Handles to update ultimate skill when change ultimate.
    /// </summary>
    /// <param name="_index"></param>
    private void UpdateUltimateSkill(int _index)
    {
        if (ultimateDictionaries.TryGetValue(_index, out UltimateType type))
        {
            currentIndex = _index;
            switch (type)
            {
                case UltimateType.FireSpin:
                    {
                        FireSpinSkill fireSpinSkill = SkillManager.Instance.FireSpinSkill;
                        SkillManager.Instance.UltimateSkill
                            .UpdateUltimateSkillInfo(UltimateType.FireSpin, fireSpinSkill.IsFireSpinUnlocked, fireSpinSkill.Cooldown, fireSpinSkill.SkillStaminaAmount);
                        break;
                    }
                case UltimateType.IceDrill:
                    {
                        IceDrillSkill iceDrillSkill = SkillManager.Instance.IceDrillSkill;
                        SkillManager.Instance.UltimateSkill
                            .UpdateUltimateSkillInfo(UltimateType.IceDrill, iceDrillSkill.IsIceDrillUnlocked, iceDrillSkill.Cooldown, iceDrillSkill.SkillStaminaAmount);
                        break;
                    }
            }
        }
    }
}   
