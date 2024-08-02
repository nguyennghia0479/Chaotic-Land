using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory
{
    public ItemSO itemSO;
    [SerializeField] private int quantity;

    public Inventory(ItemSO _itemSO)
    {
        itemSO = _itemSO;
        AddQuantity();
    }

    public void AddQuantity() => quantity++;

    public void RemoveQuantity() => quantity--;

    public int GetQuantity() => quantity;
}
