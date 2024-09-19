using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>, ISaveManager
{
    [SerializeField] private Player player;
    [SerializeField] private EnemyManager enemyManager;

    private int currentExp;
    private int currentLevel;
    private int levelUpThreshold;
    private int currentPoint;
    private int pointAdded;
    private readonly int pointToAdd = 2;

    public event EventHandler OnUpdateExp;

    protected override void Awake()
    {
        base.Awake();
    }

    #region Player EXP
    public void IncreaseExp(int _exp)
    {
        currentExp += _exp;

        if (currentExp >= levelUpThreshold)
        {
            currentLevel++;
            currentPoint += pointToAdd;
            currentExp -= levelUpThreshold;
            UpdateLevelUpThreshold();
        }

        InvokeUpdateExp();
    }

    /// <summary>
    /// Handles to decrease player's EXP when skill unlocked.
    /// </summary>
    /// <param name="_requiredExp"></param>
    public void SkillUnlocked(int _requiredExp)
    {
        if (currentExp < _requiredExp) return;

        currentExp -= _requiredExp;
        InvokeUpdateExp();
    }

    /// <summary>
    /// Handles to update level up threshold when player level up.
    /// </summary>
    private void UpdateLevelUpThreshold()
    {
        float levelUpRate = .4f;
        levelUpThreshold = 100;

        for (int i = 1; i < currentLevel; i++)
        {
            levelUpThreshold += Mathf.RoundToInt(levelUpThreshold * levelUpRate);
        }
    }

    /// <summary>
    /// Handles to update player's EXP UI
    /// </summary>
    private void InvokeUpdateExp()
    {
        OnUpdateExp?.Invoke(this, EventArgs.Empty);
    }
    #endregion

    #region Point to upgrade stats
    public void IncreasePointToAdd()
    {
        pointAdded++;
    }

    public void DecreasePointToAdd()
    {
        pointAdded--;
    }

    public bool CanAddPoint()
    {
        return pointAdded < currentPoint;
    }

    public void UpdatePoint()
    {
        currentPoint -= pointAdded;
        pointAdded = 0;
    }
    #endregion

    public void SaveData(ref GameData _gameData)
    {
        _gameData.currentExp = currentExp;
        _gameData.currentLevel = currentLevel;
        _gameData.currentPoint = currentPoint;
    }

    public void LoadData(GameData _gameData)
    {
        currentLevel = _gameData.currentLevel;
        currentExp = _gameData.currentExp;
        currentPoint = _gameData.currentPoint;

        UpdateLevelUpThreshold();
        InvokeUpdateExp();

        enemyManager.SetupEnemyLevel();
    }

    #region Getter
    public Player Player
    {
        get { return player; }
    }

    public int CurrentExp
    {
        get { return currentExp; }
    }

    public int CurrentLevel
    {
        get { return currentLevel; }
    }

    public int LevelUpThreshold
    {
        get { return levelUpThreshold; }
    }

    public int CurrentPoint
    {
        get { return currentPoint; }
    }
    #endregion
}
