using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StatUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private StatType statType;
    [SerializeField] private string statName;
    [SerializeField][TextArea] private string statDes;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private StatTooltipUI statTooltip;
    [Header("Modify info")]
    [SerializeField] private TextMeshProUGUI modifyText;
    [SerializeField] private Color plusColor;
    [SerializeField] private Color defaultColor;

    private PlayerStats playerStats;
    private float modify;
    private int point;

    private void OnValidate()
    {
        gameObject.name = statType.ToString();
        if (nameText != null)
        {
            nameText.text = statName;
        }
    }

    private void Start()
    {
        InitStatUI();
        UpdateStatUI();
    }

    private void InitStatUI()
    {
        if (playerStats == null)
        {
            playerStats = PlayerManager.Instance.Player.GetComponent<PlayerStats>();
        }
    }

    /// <summary>
    /// Handles to update character's stat modify.
    /// </summary>
    public void UpdateModifyUI()
    {
        if (modifyText == null) return;
        InitStatUI();

        if (point == 0)
        {
            modifyText.text = playerStats.GetStatByType(statType).GetValueWithModify().ToString();
        }
        else
        {
            modifyText.text = modify.ToString();
        }
    }

    /// <summary>
    /// Handles to update character's stat.
    /// </summary>
    public void UpdateStatUI()
    {
        if (valueText == null) return;
        InitStatUI();

        valueText.text = playerStats.GetStatByType(statType).GetValueWithModify().ToString();
    }

    /// <summary>
    /// Handles to update stat modify ui when clicked add or remove button.
    /// </summary>
    /// <param name="_point"></param>
    public void UpdateStatModify(int _point)
    {
        bool isInitStat = false;
        InitStatUI();
        point = _point;
        modifyText.color = _point > 0 ? plusColor : defaultColor;
        modify = playerStats.CalculateStatModify(statType, _point, isInitStat);
        modifyText.text = modify.ToString();
    }

    /// <summary>
    /// Handles to increase stats when confirmed.
    /// </summary>
    public void IncreaseStat()
    {
        playerStats.IncreaseStat(statType, modify);

        if (statType == StatType.MaxHealth)
        {
            GameManager.Instance.InGameUI.UpdateHealthStat();
        }

        if (statType == StatType.Stamina)
        {
            GameManager.Instance.InGameUI.UpdateStaminaStat();
        }
    }

    #region Tooltip
    /// <summary>
    /// Handles to show stat tooltip.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (statTooltip == null) return;

        statTooltip.ShowStatTooltip(statDes);
    }

    /// <summary>
    /// Handles to hide stat tooltip.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (statTooltip == null) return;

        statTooltip.HideStatTooltip();
    }
    #endregion
}
