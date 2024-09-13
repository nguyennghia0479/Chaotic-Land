using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] private Player player;

    private int currentExp;
    private int level;
    private int levelUpThreshold;
    private int currentPoint;
    private int pointAdded;
    private readonly int pointToAdd = 2;

    public event EventHandler OnUpdateExp;

    protected override void Awake()
    {
        base.Awake();

        currentExp = 0;
        level = 1;
        levelUpThreshold = 100;
        currentPoint = 0;
    }

    protected void Start()
    {
        UpdateLevelUpThreshold();
        InvokeUpdateExp();
    }

    #region Player EXP
    public void IncreaseExp(int _exp)
    {
        currentExp += _exp;

        if (currentExp >= levelUpThreshold)
        {
            level++;
            currentPoint += pointToAdd;
            currentExp -= levelUpThreshold;
            UpdateLevelUpThreshold();
        }

        InvokeUpdateExp();
    }

    public void SkillUnlocked(int _requiredExp)
    {
        if (currentExp < _requiredExp) return;

        currentExp -= _requiredExp;
        InvokeUpdateExp();
    }

    private void UpdateLevelUpThreshold()
    {
        float levelUpRate = .5f;
        for (int i = 1; i < level; i++)
        {
            levelUpThreshold += Mathf.RoundToInt(levelUpThreshold * levelUpRate);
        }
    }

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

    #region Getter
    public Player Player
    {
        get { return player; }
    }

    public int CurrentExp
    {
        get { return currentExp; }
    }

    public int Level
    {
        get { return level; }
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
