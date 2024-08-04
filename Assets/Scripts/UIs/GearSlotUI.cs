using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GearSlotUI : ItemSlotUI
{
    [SerializeField] private GearType slotType;

    private void OnValidate()
    {
        gameObject.name = "GearSlotBG - " + slotType.ToString();
    }

    /// <summary>
    /// Handles to unequip gear if inventory item has empty slot.
    /// </summary>
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item == null || item.itemSO == null) return;

        InventoryManager inventory = InventoryManager.Instance;
        if (inventory.CanAddItem())
        {
            inventory.UnequipGear(item.itemSO as GearSO);
            inventory.AddItemToInventory(item as InventoryItem);
            ClearSlot();
        }
    }

    public GearType SlotType
    {
        get { return slotType; }
    }
}
