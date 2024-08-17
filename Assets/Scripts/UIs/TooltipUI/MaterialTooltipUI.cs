using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MaterialTooltipUI : ItemTooltipUI
{
    /// <summary>
    /// Handles to show and set material tooltip info.
    /// </summary>
    /// <param name="_item"></param>
    public override void ShowItemTooltip(Inventory _item)
    {
        itemName.text = _item.itemSO.itemName;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Handles to hide and clear material tooltip info.
    /// </summary>
    public override void HideItemTooltip()
    {
        itemName.text = "";
        transform.position = defaultPos;
        gameObject.SetActive(false);
    }
}
