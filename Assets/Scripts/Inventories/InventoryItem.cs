using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    public ItemSO itemSO;
    [SerializeField] private int quantity;

    public InventoryItem(ItemSO _itemSO)
    {
        itemSO = _itemSO;
        AddQuantity();
    }

    public InventoryItem(ItemSO _itemSO, int _quantity)
    {
        itemSO = _itemSO;
        quantity = _quantity;
    }

    public void AddQuantity() => quantity++;

    public void RemoveQuantity() => quantity--;

    public int GetQuantity() => quantity;
}
