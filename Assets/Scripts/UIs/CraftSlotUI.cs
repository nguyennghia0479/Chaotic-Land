using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftSlotUI : ItemSlotUI
{
    [SerializeField] private GearSO gearSO;

    private void OnValidate()
    {
        image.sprite = gearSO.sprite;
    }

    /// <summary>
    /// Handles to craft item.
    /// </summary>
    public override void OnPointerDown(PointerEventData eventData)
    {
        InventoryManager.Instance.CraftItem(gearSO);
    }
}
