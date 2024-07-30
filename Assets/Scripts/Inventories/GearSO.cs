using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GearType
{
    Weapon, Armor, Amulet, Flask
}

[CreateAssetMenu(menuName = "Data/Gear Data")]
public class GearSO : ItemSO
{
    [Header("Equipment info")]
    public GearType EquipType;
    [Range(0f, 100f)]
    public float condition;
    public List<InventoryItem> craftingMaterials;

    [Header("Attribute stats info")]
    public int vitality;
    public int endurance;
    public int strength;
    public int dexterity;
    public int intelligence;
    public int agility;

    [Header("General stats info")]
    public int maxHealth;
    public int stamina;

    [Header("Offensive stats info")]
    public int physicsDamage;
    public int critChance;
    public int critPower;
    public int magicDamage;

    [Header("Defensive stats info")]
    public int evasion;
    public int armor;
    public int resistance;

    public void AddModifiy()
    {
        PlayerStats playerStats = PlayerManager.Instance.Player.GetComponent<PlayerStats>();

        playerStats.vitality.AddModify(vitality);
        playerStats.endurance.AddModify(endurance);
        playerStats.strength.AddModify(strength);
        playerStats.dexterity.AddModify(dexterity);
        playerStats.intelligence.AddModify(intelligence);
        playerStats.agility.AddModify(agility);
        playerStats.physicsDamage.AddModify(physicsDamage);
        playerStats.critChance.AddModify(critChance);
        playerStats.critPower.AddModify(critPower);
        playerStats.magicDamage.AddModify(magicDamage);
        playerStats.evasion.AddModify(evasion);
        playerStats.armor.AddModify(armor);
        playerStats.resistance.AddModify(resistance);
    }

    public void RemoveModifiy()
    {
        PlayerStats playerStats = PlayerManager.Instance.Player.GetComponent<PlayerStats>();

        playerStats.vitality.RemoveModify(vitality);
        playerStats.endurance.RemoveModify(endurance);
        playerStats.strength.RemoveModify(strength);
        playerStats.dexterity.RemoveModify(dexterity);
        playerStats.intelligence.RemoveModify(intelligence);
        playerStats.agility.RemoveModify(agility);
        playerStats.physicsDamage.RemoveModify(physicsDamage);
        playerStats.critChance.RemoveModify(critChance);
        playerStats.critPower.RemoveModify(critPower);
        playerStats.magicDamage.RemoveModify(magicDamage);
        playerStats.evasion.RemoveModify(evasion);
        playerStats.armor.RemoveModify(armor);
        playerStats.resistance.RemoveModify(resistance);
    }
}
