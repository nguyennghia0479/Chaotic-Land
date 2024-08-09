using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItem : Inventory
{
    [SerializeField] private float durability;
    [SerializeField] private string itemId;

    private readonly float maxDurability = 100;

    /// <summary>
    /// Handles to create from craft.
    /// </summary>
    /// <param name="_itemSO"></param>
    public InventoryItem(ItemSO _itemSO) : base(_itemSO)
    {
        durability = maxDurability;
        itemId = Guid.NewGuid().ToString();
    }

    /// <summary>
    /// Handles to transfer data.
    /// </summary>
    /// <param name="_itemSO"></param>
    /// <param name="_condition"></param>
    /// <param name="_itemId"></param>
    public InventoryItem(ItemSO _itemSO, float _condition, string _itemId) : base(_itemSO)
    {
        durability = _condition;
        itemId = _itemId;
    }

    /// <summary>
    /// Handles to decrease condition by lose condition speed.
    /// </summary>
    /// <param name="_loseConditionSpeed"></param>
    public void DecreaseDurability(float _loseConditionSpeed)
    {
        durability -= (maxDurability * _loseConditionSpeed);
    }

    public float Condition
    {
        get { return durability; }
    }

    public string ItemId
    {
        get { return itemId; }
    }
}
