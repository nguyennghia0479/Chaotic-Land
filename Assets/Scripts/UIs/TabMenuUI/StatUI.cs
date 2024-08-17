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
        UpdateStatUI();
    }

    /// <summary>
    /// Handles to update character's stat.
    /// </summary>
    public void UpdateStatUI()
    {
        if (PlayerManager.Instance.Player.TryGetComponent(out PlayerStats stats))
        {
            valueText.text = stats.GetStatByType(statType).GetValue().ToString();
        }
    }

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
}
