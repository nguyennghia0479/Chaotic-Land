using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    public string itemId;

    private void OnValidate()
    {
#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);
        itemId = AssetDatabase.AssetPathToGUID(path);
#endif
    }
}
