using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour, IPointerDownHandler
{
    public Image itemIcon;
    public TextMeshProUGUI itemText;
    public Inventory item;
    protected TabMenuUI tabMenu;

    protected virtual void Start()
    {
        tabMenu = GetComponentInParent<TabMenuUI>();
    }

    /// <summary>
    /// Handles to update item slot ui.
    /// </summary>
    public void UpdateItemSlotUI(Inventory _newItem)
    {
        item = _newItem;

        if (item != null)
        {
            itemIcon.color = Color.white;
            itemIcon.sprite = item.itemSO.sprite;

            if (item.GetQuantity() > 1)
            {
                itemText.text = item.GetQuantity().ToString();
            }
            else
            {
                itemText.text = "";
            }
        }   
    }

    /// <summary>
    /// Handles to clear slot info.
    /// </summary>
    public void ClearSlot()
    {
        item = null;
        itemIcon.color = Color.clear;
        itemIcon.sprite = null;
        itemText.text = "";
    }

    /// <summary>
    /// Handles to equip gear.
    /// </summary>
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item == null || item.itemSO == null) return;

        if (item.itemSO.type == ItemType.Gear)
        {
            InventoryManager.Instance.EquipGear(item as InventoryItem);
        }
    }
}
