using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentLevel;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI pointText;
    [SerializeField] private Image levelBar;
    [SerializeField] private Button confirmBtn;

    private PlayerManager playerManager;
    private StatUI[] statUIs;
    private AttributeUI[] attributeUIs;

    private void Awake()
    {
        confirmBtn.onClick.AddListener(() =>
        {
            ConfirmUpgradeStats();
        });
    }

    /// <summary>
    /// Handles to update character's stats.
    /// </summary>
    public void UpdateCharacter()
    {
        InitCharater();
        UpdateStats();

        int level = playerManager.CurrentLevel;
        int currentExp = playerManager.CurrentExp;
        int levelUpThreshold = playerManager.LevelUpThreshold;
        int point = playerManager.CurrentPoint;

        currentLevel.text = "Level " + level.ToString();
        levelText.text = currentExp + "/" + levelUpThreshold;
        pointText.text = point.ToString();
        levelBar.fillAmount = (float)currentExp / levelUpThreshold;
    }

    /// <summary>
    /// Handles to setup variables.
    /// </summary>
    private void InitCharater()
    {
        if (playerManager == null)
        {
            playerManager = PlayerManager.Instance;
        }

        if (statUIs == null)
        {
            statUIs = GetComponentsInChildren<StatUI>();
        }

        if (attributeUIs == null)
        {
            attributeUIs = GetComponentsInChildren<AttributeUI>();
        }
    }

    /// <summary>
    /// Handles to update stats.
    /// </summary>
    private void UpdateStats()
    {
        foreach (StatUI stat in statUIs)
        {
            stat.UpdateStatUI();
            stat.UpdateModifyUI();
        }
    }

    /// <summary>
    /// Handles to update confirm button status.
    /// </summary>
    public void UpdateConfirmButtonStatus()
    {
        InitCharater();

        bool isPointAdded = false;
        TextMeshProUGUI confirmText = confirmBtn.GetComponentInChildren<TextMeshProUGUI>();

        foreach (AttributeUI attribute in attributeUIs)
        {
            if (attribute.Point > 0)
            {
                isPointAdded = true;
                confirmText.color = Color.white;
                confirmBtn.enabled = true;
                break;
            }
        }

        if (!isPointAdded)
        {
            float alpha = 100;
            Color color = confirmText.color;
            confirmText.color = new Color(color.r, color.g, color.b, alpha / 255);
            confirmBtn.enabled = false;
        }
    }

    /// <summary>
    /// Handles to confirm upgrade stats.
    /// </summary>
    private void ConfirmUpgradeStats()
    {
        foreach (AttributeUI attribute in attributeUIs)
        {
            if (attribute.Point > 0)
            {
                attribute.UpgradeStats();
            }
        }
                
        UpdateStats();
        playerManager.UpdatePoint();
        pointText.text = playerManager.CurrentPoint.ToString();
    }
}
