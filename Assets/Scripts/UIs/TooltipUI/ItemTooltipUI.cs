using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemTooltipUI : TooltipUI
{
    [Header("Item tooltip info")]
    [SerializeField] protected TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemType;
    [SerializeField] private TextMeshProUGUI itemDes;
    [SerializeField] private TextMeshProUGUI itemDurability;

    /// <summary>
    /// Handles to show and set item tooltip info.
    /// </summary>
    /// <param name="_item"></param>
    public virtual void ShowItemTooltip(Inventory _item)
    {
        InventoryItem item = _item as InventoryItem;
        itemName.text = item.itemSO.itemName;
        itemDes.text = (item.itemSO as GearSO).GetDescription();
        itemDurability.text = item.Durability.ToString() + "%";

        if (_item.itemSO.type == ItemType.Gear)
        {
            itemType.text = (_item.itemSO as GearSO).gearType.ToString();
        }

        gameObject.SetActive(true);
    }

    /// <summary>
    /// Handles to hide and clear item tooltip info.
    /// </summary>
    public virtual void HideItemTooltip()
    {
        itemName.text = "";
        itemType.text = "";
        itemDes.text = "";
        itemDurability.text = "";
        transform.position = defaultPos;
        gameObject.SetActive(false);
    }
}
