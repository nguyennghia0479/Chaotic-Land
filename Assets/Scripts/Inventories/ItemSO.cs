using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Material, Gear
}

[CreateAssetMenu(menuName = "Data/Item Data")]
public class ItemSO : ScriptableObject
{
    [Header("Item info")]
    public string itemName;
    public Sprite sprite;
    [Range(0f, 100f)]
    public float dropChance;
    public ItemType type;
}
