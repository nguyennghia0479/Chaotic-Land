using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UltimateSkillDropdown : MonoBehaviour, ISaveManager
{
    [SerializeField] private TMP_Dropdown ultimateDropdown;

    private Dictionary<int, UltimateType> ultimateDictionaries;
    private List<string> ultimateOptions;
    private int currentIndex;
    private GameData gameData;
    private bool isSkillReseted;

    private void Awake()
    {
        ultimateDropdown.onValueChanged.AddListener(UpdateUltimateSkill);
    }

    private void Start()
    {
        InitUltimateSkillDropdown();
    }

    /// <summary>
    /// Handles to init variables.
    /// </summary>
    private void InitUltimateSkillDropdown()
    {
        if (ultimateDictionaries == null)
        {
            ultimateDictionaries = new Dictionary<int, UltimateType>();
        }

        if (ultimateOptions == null)
        {
            ultimateOptions = new List<string>();
        }

        if (ultimateOptions.Count == 0)
        {
            ultimateDropdown.ClearOptions();
            ultimateDropdown.enabled = false;
        }
    }

    /// <summary>
    /// Handles to update ultimate skill when change ultimate.
    /// </summary>
    /// <param name="_index"></param>
    private void UpdateUltimateSkill(int _index)
    {
        if (SkillManager.Instance == null || SkillManager.Instance.UltimateSkill == null
            || SkillManager.Instance.FireSpinSkill == null || SkillManager.Instance.IceDrillSkill == null)
            return;

        if (ultimateDictionaries.TryGetValue(_index, out UltimateType type))
        {
            currentIndex = _index;
            switch (type)
            {
                case UltimateType.FireSpin:
                    {
                        FireSpinSkill fireSpinSkill = SkillManager.Instance.FireSpinSkill;
                        SkillManager.Instance.UltimateSkill
                            .UpdateUltimateSkillInfo(UltimateType.FireSpin, fireSpinSkill.IsFireSpinUnlocked, fireSpinSkill.Cooldown, fireSpinSkill.SkillStaminaAmount, isSkillReseted);
                        isSkillReseted = true;
                        break;
                    }
                case UltimateType.IceDrill:
                    {
                        IceDrillSkill iceDrillSkill = SkillManager.Instance.IceDrillSkill;
                        SkillManager.Instance.UltimateSkill
                            .UpdateUltimateSkillInfo(UltimateType.IceDrill, iceDrillSkill.IsIceDrillUnlocked, iceDrillSkill.Cooldown, iceDrillSkill.SkillStaminaAmount, isSkillReseted);
                        isSkillReseted = true;
                        break;
                    }
            }
        }
    }

    #region Add option
    /// <summary>
    /// Handles to add option when ultimate skill unlocked.
    /// </summary>
    /// <param name="_type"></param>
    public void AddOption(UltimateType _type)
    {
        string option = SetOption(_type);
        if (option == null) return;

        ultimateOptions.Add(option);
        ultimateDropdown.enabled = true;
        ultimateDropdown.ClearOptions();
        ultimateDropdown.AddOptions(ultimateOptions);
        ultimateDropdown.RefreshShownValue();
        isSkillReseted = false;
        ultimateDropdown.value = ultimateDropdown.options.Count;
    }

    /// <summary>
    /// Handles to add ultimate type.
    /// </summary>
    /// <param name="_type"></param>
    /// <returns></returns>
    private string SetOption(UltimateType _type)
    {
        if (ultimateDictionaries == null)
        {
            return null;
        }

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
    #endregion

    #region Save and Load
    /// <summary>
    /// Handles to save ultimate skill dropdown.
    /// </summary>
    /// <param name="_gameData"></param>
    public void SaveData(ref GameData _gameData)
    {
        if (ultimateDictionaries == null) return;

        _gameData.ultimateIdSelected = currentIndex;
        _gameData.ultimateTypes.Clear();
        foreach (KeyValuePair<int, UltimateType> ultimateType in ultimateDictionaries)
        {
            _gameData.ultimateTypes.Add(ultimateType.Key, ultimateType.Value);
        }
    }

    /// <summary>
    /// Handles to load ultimate skill dropdown.
    /// </summary>
    /// <param name="_gameData"></param>
    public void LoadData(GameData _gameData)
    {
        gameData = _gameData;
        if (gameData.ultimateTypes.Count > 0)
        {
            Invoke(nameof(LoadUltimateTypeUnlocked), .1f);
        }
    }

    /// <summary>
    /// Handles to load ultimate unlocked and last ultimate type used.
    /// </summary>
    private void LoadUltimateTypeUnlocked()
    {
        ultimateDropdown.ClearOptions();
        ultimateDictionaries = new Dictionary<int, UltimateType>();
        ultimateOptions = new List<string>();

        foreach (KeyValuePair<int, UltimateType> ultimateType in gameData.ultimateTypes)
        {
            string option = SetOption(ultimateType.Value);
            ultimateOptions.Add(option);
        }

        ultimateDropdown.AddOptions(ultimateOptions);
        ultimateDropdown.RefreshShownValue();
        ultimateDropdown.enabled = true;
        ultimateDropdown.value = gameData.ultimateIdSelected;
    }
    #endregion
}
