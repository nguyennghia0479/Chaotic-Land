using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] protected Image image;
    [SerializeField] protected TextMeshProUGUI quantity;

    protected Inventory item;

    public void UpdateItemSlotUI(Inventory _newItem)
    {
        item = _newItem;

        if (item != null)
        {
            image.color = Color.white;
            image.sprite = item.itemSO.sprite;

            if (item.GetQuantity() > 1)
            {
                quantity.text = item.GetQuantity().ToString();
            }
            else
            {
                quantity.text = "";
            }
        }   
    }

    public void ClearSlot()
    {
        item = null;
        image.color = Color.clear;
        image.sprite = null;
        quantity.text = "";
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item == null || item.itemSO == null) return;

        if (item.itemSO.type == ItemType.Gear)
        {
            InventoryManager.Instance.EquipGear(item.itemSO);
        }

    }
}
