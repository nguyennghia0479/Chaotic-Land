using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory
{
    public ItemSO itemSO;
    [SerializeField] private int quantity;

    /// <summary>
    /// Handles for add inventory.
    /// </summary>
    /// <param name="_itemSO"></param>
    public Inventory(ItemSO _itemSO)
    {
        itemSO = _itemSO;
        AddQuantity();
    }

    /// <summary>
    /// Handles for add inventory when load game.
    /// </summary>
    /// <param name="_itemSO"></param>
    /// <param name="_quantity"></param>
    public Inventory(ItemSO _itemSO, int _quantity)
    {
        itemSO = _itemSO;
        quantity = _quantity;
    }

    public void AddQuantity() => quantity++;

    public void RemoveQuantity() => quantity--;

    public int GetQuantity() => quantity;
}
