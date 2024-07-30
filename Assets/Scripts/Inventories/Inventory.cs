using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Singleton<Inventory>
{
    [SerializeField] private Transform materialSlotParent;
    private ItemSlotUI[] materialSlots;
    [SerializeField] private List<InventoryItem> materials;
    private Dictionary<ItemSO, InventoryItem> materialDictionaries;

    [SerializeField] private Transform itemSlotParent;
    private ItemSlotUI[] itemSlots;
    [SerializeField] private List<InventoryItem> items;
    /*private Dictionary<ItemSO, InventoryItem> itemDictionaries;*/

    [SerializeField] private Transform gearSlotParent;
    private GearSlotUI[] gearSlots;
    [SerializeField] private List<InventoryItem> gears;
    private Dictionary<GearSO, InventoryItem> gearDictionaries;

    private void Start()
    {
        materialSlots = materialSlotParent.GetComponentsInChildren<ItemSlotUI>();
        itemSlots = itemSlotParent.GetComponentsInChildren<ItemSlotUI>();
        gearSlots = gearSlotParent.GetComponentsInChildren<GearSlotUI>();

        materials = new List<InventoryItem>();
        materialDictionaries = new Dictionary<ItemSO, InventoryItem>();
        items = new List<InventoryItem>();
        /* itemDictionaries = new Dictionary<ItemSO, InventoryItem>();*/
        gears = new List<InventoryItem>();
        gearDictionaries = new Dictionary<GearSO, InventoryItem>();
    }

    #region Craft item
    public void CraftItem(GearSO _itemToCraft)
    {
        if (!CanCraft(_itemToCraft)) return;

        foreach (InventoryItem requiredMaterial in _itemToCraft.craftingMaterials)
        {
            for (int i = 0; i < requiredMaterial.GetQuantity(); i++)
            {
                RemoveInventory(requiredMaterial.itemSO);
            }
        }

        AddInventory(_itemToCraft);
    }

    private bool CanCraft(GearSO _itemToCraft)
    {
        if (_itemToCraft == null)
            return false;

        foreach (InventoryItem requiredMaterial in _itemToCraft.craftingMaterials)
        {
            if (materialDictionaries.TryGetValue(requiredMaterial.itemSO, out InventoryItem material))
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

        foreach (KeyValuePair<GearSO, InventoryItem> pair in gearDictionaries)
        {
            if (pair.Key.EquipType == newEquip.EquipType)
            {
                oldEquip = pair.Key;
            }
        }

        if (oldEquip != null)
        {
            UnequipGear(oldEquip);
            AddInventory(oldEquip);
        }

        InventoryItem newGear = new(newEquip);
        gears.Add(newGear);
        gearDictionaries.Add(newEquip, newGear);
        RemoveInventory(newEquip);
        UpdateItemSlotUI(_itemSO);
        newEquip.AddModifiy();
    }

    public void UnequipGear(GearSO _oldEquip)
    {
        if (_oldEquip == null) return;

        if (gearDictionaries.TryGetValue(_oldEquip, out InventoryItem item))
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
        if (materialDictionaries.TryGetValue(_itemSO, out InventoryItem item))
        {
            item.AddQuantity();
        }
        else
        {
            InventoryItem newItem = new(_itemSO);
            materials.Add(newItem);
            materialDictionaries.Add(_itemSO, newItem);
        }
    }

    private void AddItemToInventory(ItemSO _itemSO)
    {
        InventoryItem newItem = new(_itemSO);
        items.Add(newItem);
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
        if (materialDictionaries.TryGetValue(_itemSO, out InventoryItem item))
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
        InventoryItem removeItem = null;

        foreach (InventoryItem item in items)
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
            foreach (KeyValuePair<GearSO, InventoryItem> pair in gearDictionaries)
            {
                if (pair.Key.EquipType == gearSlots[i].SlotType)
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
    }
}
