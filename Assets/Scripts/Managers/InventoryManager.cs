using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    #region Variables
    [Header("Material info")]
    [SerializeField] private Transform materialSlotParent;
    private ItemSlotUI[] materialSlots;
    [SerializeField] private List<Inventory> materials;
    private Dictionary<ItemSO, Inventory> materialDictionaries;

    [Header("Item info")]
    [SerializeField] private Transform itemSlotParent;
    private ItemSlotUI[] itemSlots;
    [SerializeField] private List<InventoryItem> items;
    private Dictionary<string, InventoryItem> itemDictionaries;

    [Header("Gear info")]
    [SerializeField] private Transform[] gearSlotParents;
    private List<GearSlotUI> gearSlots;
    [SerializeField] private List<InventoryItem> gears;
    private Dictionary<GearSO, InventoryItem> gearDictionaries;

    [Header("Stats info")]
    [SerializeField] private GameObject inventoryStats;
    private StatUI[] statUIs;

    private float lastTimeUseFlask;
    private float flaskCooldown;
    private float lastTimeUseArmor;
    private float armorCooldown;
    #endregion

    private void Start()
    {
        SetupGearSlotsUI();

        materialSlots = materialSlotParent.GetComponentsInChildren<ItemSlotUI>();
        itemSlots = itemSlotParent.GetComponentsInChildren<ItemSlotUI>();
        statUIs = inventoryStats.GetComponentsInChildren<StatUI>();

        materials = new List<Inventory>();
        materialDictionaries = new Dictionary<ItemSO, Inventory>();

        items = new List<InventoryItem>();
        itemDictionaries = new Dictionary<string, InventoryItem>();

        gears = new List<InventoryItem>();
        gearDictionaries = new Dictionary<GearSO, InventoryItem>();
    }

    /// <summary>
    /// Handles to setup gear slots ui.
    /// </summary>
    private void SetupGearSlotsUI()
    {
        gearSlots = new List<GearSlotUI>();
        foreach (Transform gearSlotParent in gearSlotParents)
        {
            foreach (GearSlotUI gearSlot in gearSlotParent.GetComponentsInChildren<GearSlotUI>())
            {
                gearSlots.Add(gearSlot);
            }
        }
    }

    #region Gear durability
    /// <summary>
    /// Handles to get gear by gear type.
    /// </summary>
    /// <param name="_gearType"></param>
    /// <returns>GearSO if found. Null if not.</returns>
    public GearSO GetGearByGearType(GearType _gearType)
    {
        GearSO gear = null;

        foreach (KeyValuePair<GearSO, InventoryItem> item in gearDictionaries)
        {
            if (item.Key.gearType == _gearType)
            {
                gear = item.Key;
            }
        }

        return gear;
    }

    /// <summary>
    /// Handles to decrease durability of gear.
    /// </summary>
    /// <param name="_gearType"></param>
    public void DecreaseGearDurability(GearType _gearType)
    {
        GearSO gear = GetGearByGearType(_gearType);

        if (gear == null) return;

        if (gearDictionaries.TryGetValue(gear, out InventoryItem item))
        {
            item.DecreaseDurability(gear.loseConditionSpeed);

            if (item.Durability <= 0)
            {
                UnequipGear(gear);
            }
        }
        UpdateItemSlotUI(gear);
    }
    #endregion

    #region Use Flask and armor effect
    /// <summary>
    /// Handles to use flask.
    /// </summary>
    public void UseFlask()
    {
        GearSO flaskGear = GetGearByGearType(GearType.Flask);
        if (flaskGear != null)
        {
            if (Time.time > lastTimeUseFlask + flaskCooldown)
            {
                lastTimeUseFlask = Time.time;
                flaskCooldown = flaskGear.cooldown;
                flaskGear.ExecuteItemEffects(null);

                DecreaseGearDurability(GearType.Flask);
            }
        }
    }

    /// <summary>
    /// Handles to use armor effect.
    /// </summary>
    /// <returns>True if time is greater than cooldown. False if not.</returns>
    public bool CanUseArmorEffect()
    {
        GearSO armorGear = GetGearByGearType(GearType.Armor);
        if (armorGear != null)
        {
            if (Time.time > lastTimeUseArmor + armorCooldown)
            {
                lastTimeUseArmor = Time.time;
                armorCooldown = armorGear.cooldown;
                return true;
            }
        }

        return false;
    }
    #endregion

    #region Craft item
    /// <summary>
    /// Handles to craft item.
    /// </summary>
    /// <param name="_itemToCraft"></param>
    public void CraftItem(GearSO _itemToCraft)
    {
        if (!CanAddItem() || !CanCraft(_itemToCraft)) return;

        foreach (Inventory requiredMaterial in _itemToCraft.craftingMaterials)
        {
            for (int i = 0; i < requiredMaterial.GetQuantity(); i++)
            {
                RemoveMaterialFromInventory(requiredMaterial.itemSO);
            }
        }

        InventoryItem newItem = new(_itemToCraft);
        items.Add(newItem);
        itemDictionaries.Add(newItem.ItemId, newItem);
        UpdateItemSlotUI(_itemToCraft);
    }

    /// <summary>
    /// Handles to check can craft item.
    /// </summary>
    /// <param name="_itemToCraft"></param>
    /// <returns>True if found enough required material. False if not.</returns>
    private bool CanCraft(GearSO _itemToCraft)
    {
        if (_itemToCraft == null)
            return false;

        foreach (Inventory requiredMaterial in _itemToCraft.craftingMaterials)
        {
            if (materialDictionaries.TryGetValue(requiredMaterial.itemSO, out Inventory material))
            {
                if (material.GetQuantity() < requiredMaterial.GetQuantity())
                {
                    Debug.Log("Not enough material");
                    return false;
                }
            }
            else
            {
                Debug.Log("Not found material");
                return false;
            }
        }

        return true;
    }
    #endregion

    #region Equip and Unequip gear
    /// <summary>
    /// Handles to equip item.
    /// </summary>
    /// <param name="_itemToEquip"></param>
    /// <remarks>
    /// Unequip old item if has exists then equip new item.
    /// </remarks>
    public void EquipGear(InventoryItem _itemToEquip)
    {
        GearSO itemToEquip = _itemToEquip.itemSO as GearSO;
        InventoryItem itemToUnequip = null;

        foreach (KeyValuePair<GearSO, InventoryItem> pair in gearDictionaries)
        {
            if (pair.Key.gearType == itemToEquip.gearType)
            {
                itemToUnequip = pair.Value;
            }
        }

        RemoveItemFromInventory(_itemToEquip);
        if (itemToUnequip != null)
        {
            UnequipGear(itemToUnequip.itemSO as GearSO);
            AddItemToInventory(itemToUnequip);
        }

        InventoryItem newGear = new(itemToEquip, _itemToEquip.Durability, _itemToEquip.ItemId);
        gears.Add(newGear);
        gearDictionaries.Add(itemToEquip, newGear);
        itemToEquip.AddModifiy();

        UpdateItemSlotUI(_itemToEquip.itemSO);
    }

    /// <summary>
    /// Handles to unequip item.
    /// </summary>
    /// <param name="_itemToUnequip"></param>
    public void UnequipGear(GearSO _itemToUnequip)
    {
        if (_itemToUnequip == null) return;

        if (gearDictionaries.TryGetValue(_itemToUnequip, out InventoryItem item))
        {
            _itemToUnequip.RemoveModifiy();
            gears.Remove(item);
            gearDictionaries.Remove(_itemToUnequip);
        }
    }
    #endregion

    #region Add item
    /// <summary>
    /// Handles to add material to inventory.
    /// </summary>
    /// <param name="_itemSO"></param>
    /// <remarks>
    /// If has the same material in inventory then add quantity. If not then add new in inventory.
    /// </remarks>
    public void AddMaterialToInventory(ItemSO _itemSO)
    {
        if (materialDictionaries.TryGetValue(_itemSO, out Inventory item))
        {
            item.AddQuantity();
        }
        else
        {
            Inventory newItem = new(_itemSO);
            materials.Add(newItem);
            materialDictionaries.Add(_itemSO, newItem);
        }

        UpdateItemSlotUI(_itemSO);
    }

    /// <summary>
    /// Handles to add item to inventory.
    /// </summary>
    /// <param name="_item"></param>
    public void AddItemToInventory(InventoryItem _item)
    {
        InventoryItem item = new(_item.itemSO, _item.Durability, _item.ItemId);
        items.Add(item);
        itemDictionaries.Add(item.ItemId, item);

        UpdateItemSlotUI(_item.itemSO);
    }

    /// <summary>
    /// Handles to check can add material.
    /// </summary>
    /// <param name="_itemSO"></param>
    /// <returns>True if material number is less than material slot or has a same material in inventory. False if not.</returns>
    public bool CanAddMaterial(ItemSO _itemSO)
    {
        if (materialDictionaries.Count < materialSlots.Length)
        {
            return true;
        }
        else
        {
            if (materialDictionaries.TryGetValue(_itemSO, out Inventory _))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Handles to check can item.
    /// </summary>
    /// <returns>True if item number is less than item slot number. False if not</returns>
    public bool CanAddItem()
    {
        return itemDictionaries.Count < itemSlots.Length;
    }
    #endregion

    #region Remove item
    /// <summary>
    /// Handles to remove material from inventory.
    /// </summary>
    /// <param name="_itemSO"></param>
    private void RemoveMaterialFromInventory(ItemSO _itemSO)
    {
        if (materialDictionaries.TryGetValue(_itemSO, out Inventory item))
        {
            item.RemoveQuantity();

            if (item.GetQuantity() <= 0)
            {
                materialDictionaries.Remove(_itemSO);
                materials.Remove(item);
            }

            UpdateItemSlotUI(_itemSO);
        }
    }

    /// <summary>
    /// Handles to remove item from inventory.
    /// </summary>
    /// <param name="_item"></param>
    private void RemoveItemFromInventory(InventoryItem _item)
    {
        if (itemDictionaries.TryGetValue(_item.ItemId, out InventoryItem item))
        {
            items.Remove(item);
            itemDictionaries.Remove(item.ItemId);

            UpdateItemSlotUI(_item.itemSO);
        }
    }
    #endregion

    #region Clear and update slot
    /// <summary>
    /// Handles to clear and update item slot ui.
    /// </summary>
    /// <param name="_itemSO"></param>
    private void UpdateItemSlotUI(ItemSO _itemSO)
    {
        if (_itemSO == null) return;
        ClearItemSlotUI(_itemSO);

        if (_itemSO.type == ItemType.Material)
        {
            for (int i = 0; i < materials.Count; i++)
            {
                materialSlots[i].UpdateItemSlotUI(materials[i]);
            }
        }
        else
        {
            for (int i = 0; i < items.Count; i++)
            {
                itemSlots[i].UpdateItemSlotUI(items[i]);
            }
        }

        for (int i = 0; i < gearSlots.Count; i++)
        {
            foreach (KeyValuePair<GearSO, InventoryItem> pair in gearDictionaries)
            {
                if (pair.Key.gearType == gearSlots[i].SlotType)
                {
                    gearSlots[i].UpdateItemSlotUI(pair.Value);
                    break;
                }
            }
        }

        UpdateStatUIs();
    }

    /// <summary>
    /// Handles to clear item slot ui.
    /// </summary>
    /// <param name="_itemSO"></param>
    private void ClearItemSlotUI(ItemSO _itemSO)
    {
        if (_itemSO == null) return;

        if (_itemSO.type == ItemType.Material)
        {
            for (int i = 0; i < materialSlots.Length; i++)
            {
                materialSlots[i].ClearSlot();
            }
        }
        else
        {
            for (int i = 0; i < itemSlots.Length; i++)
            {
                itemSlots[i].ClearSlot();
            }
        }

        for (int i = 0; i < gearSlots.Count; i++)
        {
            gearSlots[i].ClearSlot();
        }
    }

    /// <summary>
    /// Handles to update stats ui.
    /// </summary>
    public void UpdateStatUIs()
    {
        foreach (StatUI stat in statUIs)
        {
            stat.UpdateStatUI();
        }
    }
    #endregion
}
