using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public enum GearType
{
    Weapon, Armor, Amulet, Flask
}

[CreateAssetMenu(menuName = "Data/Gear Data")]
public class GearSO : ItemSO
{
    [Header("Equipment info")]
    public GearType gearType;
    public List<Inventory> craftingMaterials;
    [Range(0f, 1f)] public float loseConditionSpeed;
    protected StringBuilder sb = new();

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

    [Header("Effect info")]
    public List<ItemEffectSO> effects;
    public float cooldown;

    #region Modify stats

    /// <summary>
    /// Handles to add modify stats when equip gear.
    /// </summary>
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

    /// <summary>
    /// Handles to remove modify stats when unequip gear.
    /// </summary>
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
    #endregion

    /// <summary>
    /// Handles to execute effect of item.
    /// </summary>
    /// <param name="_target"></param>
    public void ExecuteItemEffects(Transform _target)
    {
        foreach (ItemEffectSO effect in effects)
        {
            effect.ExecuteItemEffect(_target);
        }
    }

    /// <summary>
    /// Handles to get item description.
    /// </summary>
    /// <returns>Item description</returns>
    public string GetDescription()
    {
        sb.Clear();

        AddDescription(vitality, "Vitality");
        AddDescription(endurance, "Endurance");
        AddDescription(strength, "Strength");
        AddDescription(dexterity, "Dexterity");
        AddDescription(intelligence, "Intelligence");
        AddDescription(agility, "Agility");
        AddDescription(maxHealth, "Max health");
        AddDescription(stamina, "Stamina");
        AddDescription(physicsDamage, "Physics damage");
        AddDescription(critChance, "Critical chance");
        AddDescription(critPower, "Critical power");
        AddDescription(armor, "Armor");
        AddDescription(evasion, "Evasion");
        AddDescription(resistance, "Resistance");

        sb.AppendLine();
        foreach (ItemEffectSO effect in effects)
        {
            sb.AppendLine(effect.itemDes);
        }

        return sb.ToString();
    }

    /// <summary>
    /// Handles to add description.
    /// </summary>
    /// <param name="stat"></param>
    /// <param name="des"></param>
    private void AddDescription(int stat, string des)
    {
        if (stat > 0)
        {
            sb.Append("+ ").Append(stat).Append(" ").Append(des).AppendLine();
        }
    }
}
