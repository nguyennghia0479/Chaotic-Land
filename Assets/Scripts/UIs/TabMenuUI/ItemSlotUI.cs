using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image itemIcon;
    public TextMeshProUGUI itemText;
    public Inventory item;
    [SerializeField] protected Image itemDurabilityBar;
    [SerializeField] protected Image itemDurability;
    public ItemTooltipUI itemTooltip;

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
            UpdateDurability(true);

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

        UpdateDurability(false);
    }

    /// <summary>
    /// Handles to update durability of item.
    /// </summary>
    /// <param name="_isActive"></param>
    private void UpdateDurability(bool _isActive)
    {
        if (itemDurabilityBar == null || itemDurability == null) return;

        float durabilityThreshold = .25f;
        if (_isActive)
        {
            InventoryItem inventoryItem = item as InventoryItem;
            itemDurabilityBar.gameObject.SetActive(true);
            itemDurability.fillAmount = inventoryItem.Durability / inventoryItem.MaxDurability;
            if (itemDurability.fillAmount <= durabilityThreshold)
            {
                itemDurability.color = Color.red;
            }
            else
            {
                itemDurability.color = Color.green;
            }
        }
        else
        {
            itemDurabilityBar.gameObject.SetActive(false);
        }
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
            itemTooltip.HideItemTooltip();
        }
    }

    /// <summary>
    /// Handles to show item tooltip.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemTooltip == null || item  == null || item.itemSO == null) return;

        itemTooltip.ShowItemTooltip(item);
    }

    /// <summary>
    /// Handles to hide item tooltip.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (itemTooltip == null || item == null || item.itemSO == null) return;

        itemTooltip.HideItemTooltip();
    }
}
