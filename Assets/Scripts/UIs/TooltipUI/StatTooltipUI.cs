using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatTooltipUI : TooltipUI
{
    [Header("Stat tooltip info")]
    [SerializeField] private TextMeshProUGUI statDes;

    /// <summary>
    /// Handles to show and set stat tooltip info.
    /// </summary>
    /// <param name="_text"></param>
    public void ShowStatTooltip(string _text)
    {
        statDes.text = _text;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Handles to hide and clear stat tooltip info.
    /// </summary>
    public void HideStatTooltip()
    {
        statDes.text = "";
        transform.position = defaultPos;
        gameObject.SetActive(false);
    }
}
