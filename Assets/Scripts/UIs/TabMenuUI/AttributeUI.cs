using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttributeUI : MonoBehaviour
{
    [SerializeField] private StatType statType;
    [Header("Button info")]
    [SerializeField] private Button decreaseBtn;
    [SerializeField] private Button increaseBtn;
    [SerializeField] private Color disableColor;
    [Header("Modify info")]
    [SerializeField] private TextMeshProUGUI modifyText;
    [SerializeField] private Color plusColor;
    [SerializeField] private StatUI[] referenceStatUIs;

    private PlayerManager playerManager;
    private PlayerStats playerStats;
    private CharacterUI characterUI;
    private StatUI statUI;
    private int point;

    private void Awake()
    {
        decreaseBtn.onClick.AddListener(() =>
        {
            DecreaseButton();
            PlayAttributeSound();
        });

        increaseBtn.onClick.AddListener(() =>
        {
            IncreaseButton();
            PlayAttributeSound();
        });
    }

    private void Start()
    {
        playerManager = PlayerManager.Instance;
        if (playerManager != null)
        {
            playerStats = playerManager.Player.GetComponent<PlayerStats>();
        }

        statUI = GetComponentInChildren<StatUI>();
        characterUI = GetComponentInParent<CharacterUI>();
        characterUI.UpdateConfirmButtonStatus();
    }

    private void Update()
    {
        UpdateButtonStatus();
    }

    /// <summary>
    /// Handles to updrade player's stats.
    /// </summary>
    public void UpgradeStats()
    {
        statUI.IncreaseStat();
        foreach (StatUI statUI in referenceStatUIs)
        {
            statUI.IncreaseStat();
        }

        point = 0;
        UpdateButtonStatus();
        characterUI.UpdateConfirmButtonStatus();
        statUI.UpdateStatModify(point);
        UpdateStatModify();
    }

    /// <summary>
    /// Handles to decrease attribute stat.
    /// </summary>
    private void DecreaseButton()
    {
        if (playerStats != null)
        {
            point--;
            playerManager.DecreasePointToAdd();
            statUI.UpdateStatModify(point);
            characterUI.UpdateConfirmButtonStatus();
            UpdateStatModify();
        }
    }

    /// <summary>
    /// Handles to increase attribute stat.
    /// </summary>
    private void IncreaseButton()
    {
        if (playerStats != null)
        {
            point++;
            playerManager.IncreasePointToAdd();
            statUI.UpdateStatModify(point);
            characterUI.UpdateConfirmButtonStatus();
            UpdateStatModify();
        }
    }

    /// <summary>
    /// Handles to update button status.
    /// </summary>
    private void UpdateButtonStatus()
    {
        if (point > 0)
        {
            decreaseBtn.enabled = true;
            decreaseBtn.image.color = Color.white;
        }
        else
        {
            decreaseBtn.enabled = false;
            decreaseBtn.image.color = disableColor;
        }

        if (playerManager.CanAddPoint())
        {
            increaseBtn.enabled = true;
            increaseBtn.image.color = Color.white;
        }
        else
        {
            increaseBtn.enabled = false;
            increaseBtn.image.color = disableColor; 
        }
    }

    /// <summary>
    /// Handles to update stat modify.
    /// </summary>
    private void UpdateStatModify()
    {
        foreach (StatUI statUI in referenceStatUIs)
        {
            statUI.UpdateStatModify(point);
        }
    }

    /// <summary>
    /// Handles to play attribute sound.
    /// </summary>
    private void PlayAttributeSound()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayAttributeSound();
        }
    }

    public int Point
    {
        get { return point; }
    }
}
