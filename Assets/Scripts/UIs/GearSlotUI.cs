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

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item == null || item.itemSO == null) return;

        InventoryManager inventory = InventoryManager.Instance;
        if (inventory.CanAddItem())
        {
            inventory.UnequipGear(item.itemSO as GearSO);
            inventory.AddInventory(item.itemSO);
            ClearSlot();
        }
    }

    public GearType SlotType
    {
        get { return slotType; }
    }
}
