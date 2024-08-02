using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    [SerializeField] private Transform materialSlotParent;
    private ItemSlotUI[] materialSlots;
    [SerializeField] private List<Inventory> materials;
    private Dictionary<ItemSO, Inventory> materialDictionaries;

    [SerializeField] private Transform itemSlotParent;
    private ItemSlotUI[] itemSlots;
    [SerializeField] private List<Inventory> items;

    [SerializeField] private Transform gearSlotParent;
    private GearSlotUI[] gearSlots;
    [SerializeField] private List<Inventory> gears;
    private Dictionary<GearSO, Inventory> gearDictionaries;

    private float lastTimeUseFlask;
    private float flaskCooldown;
    private float lastTimeUseArmor;
    private float armorCooldown;

    private void Start()
    {
        materialSlots = materialSlotParent.GetComponentsInChildren<ItemSlotUI>();
        itemSlots = itemSlotParent.GetComponentsInChildren<ItemSlotUI>();
        gearSlots = gearSlotParent.GetComponentsInChildren<GearSlotUI>();

        materials = new List<Inventory>();
        materialDictionaries = new Dictionary<ItemSO, Inventory>();

        items = new List<Inventory>();
    
        gears = new List<Inventory>();
        gearDictionaries = new Dictionary<GearSO, Inventory>();
    }

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
            }
        }
    }

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

    public GearSO GetGearByGearType(GearType _gearType)
    {
        GearSO gear = null;

        foreach (KeyValuePair<GearSO, Inventory> item in gearDictionaries)
        {
            if (item.Key.gearType == _gearType)
            {
                gear = item.Key;
            }
        }

        return gear;
    }

    #region Craft item
    public void CraftItem(GearSO _itemToCraft)
    {
        if (!CanCraft(_itemToCraft) || !CanAddItem()) return;

        foreach (Inventory requiredMaterial in _itemToCraft.craftingMaterials)
        {
            for (int i = 0; i < requiredMaterial.GetQuantity(); i++)
            {
                RemoveInventory(requiredMaterial.itemSO);
            }
        }

        AddInventory(_itemToCraft);
        UpdateItemSlotUI(_itemToCraft);
    }

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
    public void EquipGear(ItemSO _itemSO)
    {
        GearSO newEquip = _itemSO as GearSO;
        GearSO oldEquip = null;

        foreach (KeyValuePair<GearSO, Inventory> pair in gearDictionaries)
        {
            if (pair.Key.gearType == newEquip.gearType)
            {
                oldEquip = pair.Key;
            }
        }

        if (oldEquip != null)
        {
            UnequipGear(oldEquip);
            AddItemToInventory(oldEquip);
        }
        
        Inventory newGear = new(newEquip);
        gears.Add(newGear);
        gearDictionaries.Add(newEquip, newGear);
        RemoveInventory(newEquip);
        UpdateItemSlotUI(_itemSO);
        newEquip.AddModifiy();
    }

    public void UnequipGear(GearSO _oldEquip)
    {
        if (_oldEquip == null) return;

        if (gearDictionaries.TryGetValue(_oldEquip, out Inventory item))
        {
            gears.Remove(item);
            gearDictionaries.Remove(_oldEquip);
            _oldEquip.RemoveModifiy();
        }
    }
    #endregion

    #region Add item
    public void AddInventory(ItemSO _itemSO)
    {
        if (_itemSO.type == ItemType.Material)
        {
            AddMaterialToInventory(_itemSO);
        }
        else
        {
            AddItemToInventory(_itemSO);
        }

        UpdateItemSlotUI(_itemSO);
    }

    private void AddMaterialToInventory(ItemSO _itemSO)
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
    }

    private void AddItemToInventory(ItemSO _itemSO)
    {
        Inventory newItem = new(_itemSO);
        items.Add(newItem);
    }

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

    public bool CanAddItem()
    {
        return items.Count < itemSlots.Length;
    }
    #endregion

    #region Remove item
    public void RemoveInventory(ItemSO _itemSO)
    {
        if (_itemSO.type == ItemType.Material)
        {
            RemoveMaterialFromInventory(_itemSO);
        }
        else
        {
            RemoveItemFromInventory(_itemSO);
        }

        UpdateItemSlotUI(_itemSO);
    }

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
        }
    }

    private void RemoveItemFromInventory(ItemSO _itemSO)
    {
        Inventory removeItem = null;

        foreach (Inventory item in items)
        {
            if (item.itemSO.Equals(_itemSO))
            {
                removeItem = item;
                break;
            }
        }
        items.Remove(removeItem);
    }
    #endregion

    #region Clear and update slot
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

        for (int i = 0; i < gearSlots.Length; i++)
        {
            foreach (KeyValuePair<GearSO, Inventory> pair in gearDictionaries)
            {
                if (pair.Key.gearType == gearSlots[i].SlotType)
                {
                    gearSlots[i].UpdateItemSlotUI(pair.Value);
                    break;
                }
            }
        }
    }

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

        for (int i = 0; i < gearSlots.Length; i++)
        {
            gearSlots[i].ClearSlot();
        }
    }
    #endregion
}
