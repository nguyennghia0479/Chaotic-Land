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

        Inventory.Instance.UnequipGear(item.itemSO as GearSO);
        Inventory.Instance.AddInventory(item.itemSO);
        ClearSlot();
    }

    public GearType SlotType
    {
        get { return slotType; }
    }
}
