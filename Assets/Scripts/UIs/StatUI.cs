using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatUI : MonoBehaviour
{
    [SerializeField] private StatType statType;
    [SerializeField] private string statName;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI valueText;

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

    public void UpdateStatUI()
    {
        if (PlayerManager.Instance.Player.TryGetComponent(out PlayerStats stats))
        {
            valueText.text = stats.GetStatByType(statType).GetValue().ToString();
        }
    }
}
