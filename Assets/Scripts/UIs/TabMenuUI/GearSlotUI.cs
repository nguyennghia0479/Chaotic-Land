using UnityEngine;

public class GearSlotUI : ItemSlotUI
{
    [SerializeField] private GearType slotType;

    private InventoryItem itemDrop;

    private void OnValidate()
    {
        gameObject.name = slotType.ToString();
    }

    /// <summary>
    /// Handles to unequip gear.
    /// </summary>
    protected override void HandleActionButton()
    {
        if (actionBtn != null)
        {
            actionBtn.onClick.AddListener(() =>
            {
                if (CanUnequipGear())
                {
                    slotSelected.gameObject.SetActive(false);
                    itemAction.gameObject.SetActive(false);
                }
            });
        }
    }

    /// <summary>
    /// Handles to drop gear.
    /// </summary>
    protected override void HandleDropButton()
    {
        if (dropBtn != null)
        {
            dropBtn.onClick.AddListener(() =>
            {
                if (CanUnequipGear())
                {
                    InventoryManager.Instance.RemoveItem(itemDrop);
                    slotSelected.gameObject.SetActive(false);
                    itemAction.gameObject.SetActive(false);
                }
            });
        }
    }

    /// <summary>
    /// Handles to unequip gear.
    /// </summary>
    /// <returns></returns>
    private bool CanUnequipGear()
    {
        if (item == null || item.itemSO == null) return false;

        itemDrop = item as InventoryItem;
        InventoryManager inventory = InventoryManager.Instance;
        if (inventory.CanAddItem())
        {
            inventory.UnequipGear(item.itemSO as GearSO);
            inventory.AddItemToInventory(item as InventoryItem);
            ClearSlot();
            itemTooltip.HideItemTooltip();
            return true;
        }

        return false;
    }

    public GearType SlotType
    {
        get { return slotType; }
    }
}
