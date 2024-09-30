using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData
{
    public string id;
    public string itemId;
    public float durability;

    public ItemData(string _id, string _itemId, float _durability)
    {
        id = _id;
        itemId = _itemId;
        durability = _durability;
    }
}

public class GameData
{
    [Header("Player manager info")]
    public int currentLevel;
    public int currentExp;
    public int currentPoint;

    [Header("Player stats info")]
    public float vitality;
    public float endurance;
    public float strength;
    public float dexterity;
    public float intelligence;
    public float agility;

    [Header("Inventory manager info")]
    public Dictionary<string, int> materials;
    public List<ItemData> items;
    public List<ItemData> gears;

    [Header("Skill tree info")]
    public Dictionary<string, bool> skillTrees;
    public int swordIdSelected;
    public Dictionary<int, SwordType> swordTypes;
    public int ultimateIdSelected;
    public Dictionary<int, UltimateType> ultimateTypes;

    [Header("Setting info")]
    public Dictionary<string, float> volumes;

    public GameData()
    {
        currentLevel = 1;
        currentExp = 0;
        currentPoint = 0;
        vitality = 0;
        endurance = 0;
        strength = 0;
        dexterity = 0;
        intelligence = 0;
        agility = 0;
        materials = new Dictionary<string, int>();
        items = new List<ItemData>();
        gears = new List<ItemData>();
        skillTrees = new Dictionary<string, bool>();
        swordIdSelected = 0;
        swordTypes = new Dictionary<int, SwordType>();
        ultimateIdSelected = 0;
        ultimateTypes = new Dictionary<int, UltimateType>();
        volumes = new Dictionary<string, float>();
    }
}
