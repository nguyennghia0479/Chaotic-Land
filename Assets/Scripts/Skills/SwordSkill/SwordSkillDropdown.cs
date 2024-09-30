using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class SwordSkillDropdown : MonoBehaviour, ISaveManager
{
    [SerializeField] private TMP_Dropdown swordSkillDropdown;

    private Dictionary<int, SwordType> swordSkillDictionaries;
    private List<string> swordSkillOptions;
    private int currentIndex;
    private GameData gameData;
    private bool isReseted;

    private void Awake()
    {
        swordSkillDropdown.onValueChanged.AddListener(UpdateSwordSkill);
    }

    private void Start()
    {
        InitSwordSkillDropdown();
    }

    /// <summary>
    /// Handles to init variables.
    /// </summary>
    private void InitSwordSkillDropdown()
    {
        if (swordSkillDictionaries == null)
        {
            swordSkillDictionaries = new Dictionary<int, SwordType>();
        }

        if (swordSkillOptions == null)
        {
            swordSkillOptions = new List<string>();
        }

        if (swordSkillOptions.Count == 0)
        {
            swordSkillDropdown.ClearOptions();
            swordSkillDropdown.enabled = false;
        }
    }

    /// <summary>
    /// Handles to update sword skill when change sword.
    /// </summary>
    /// <param name="_index"></param>
    private void UpdateSwordSkill(int _index)
    {
        if (SkillManager.Instance == null || SkillManager.Instance.SwordSkill == null) return;

        if (swordSkillDictionaries.TryGetValue(_index, out SwordType swordType))
        {
            currentIndex = _index;
            SkillManager.Instance.SwordSkill.UpdateSwordSkill(swordType, isReseted);
            isReseted = true;
        }
    }

    #region Add option
    /// <summary>
    /// Handles to add option when sword skill unlocked.
    /// </summary>
    /// <param name="_swordType"></param>
    public void AddOption(SwordType _swordType)
    {
        string option = SetOption(_swordType);
        if (option == null) return;

        swordSkillOptions.Add(option);
        swordSkillDropdown.ClearOptions();
        swordSkillDropdown.enabled = true;
        swordSkillDropdown.AddOptions(swordSkillOptions);
        swordSkillDropdown.RefreshShownValue();
        isReseted = false;
        swordSkillDropdown.value = swordSkillDropdown.options.Count;
    }

    /// <summary>
    /// Handles to add sword type.
    /// </summary>
    /// <param name="_swordType"></param>
    /// <returns></returns>
    private string SetOption(SwordType _swordType)
    {
        if (swordSkillDictionaries == null) return null;

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
    #endregion

    #region Save and Load
    /// <summary>
    /// Handles to save sword skill dropdown.
    /// </summary>
    /// <param name="_gameData"></param>
    public void SaveData(ref GameData _gameData)
    {
        if (_gameData == null || swordSkillDictionaries == null) return;

        _gameData.swordIdSelected = currentIndex;
        _gameData.swordTypes.Clear();
        foreach (KeyValuePair<int, SwordType> swordType in swordSkillDictionaries)
        {
            _gameData.swordTypes.Add(swordType.Key, swordType.Value);
        }
    }

    /// <summary>
    /// Handles to load sword skill dropdown.
    /// </summary>
    /// <param name="_gameData"></param>
    public void LoadData(GameData _gameData)
    {
        if (_gameData == null) return;

        gameData = _gameData;
        if (gameData.swordTypes.Count > 0)
        {
            Invoke(nameof(LoadSwordTypeUnlocked), .1f);
        }
    }

    /// <summary>
    /// Handles to load sword type has unlocked and last sword type used.
    /// </summary>
    private void LoadSwordTypeUnlocked()
    {
        swordSkillDropdown.ClearOptions();
        swordSkillDictionaries = new Dictionary<int, SwordType>();
        swordSkillOptions = new List<string>();
 
        foreach (KeyValuePair<int, SwordType> swordType in gameData.swordTypes)
        {
            string option = SetOption(swordType.Value);
            swordSkillOptions.Add(option);
        }

        swordSkillDropdown.AddOptions(swordSkillOptions);
        swordSkillDropdown.RefreshShownValue();
        swordSkillDropdown.enabled = true;
        swordSkillDropdown.value = gameData.swordIdSelected;
    }
    #endregion
}
