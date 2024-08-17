using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDes;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Image[] materials;
    [SerializeField] private Button craftBtn;
    [SerializeField] private MaterialTooltipUI materialTooltip;

    private GearSO craftItem;

    private void Awake()
    {
        craftBtn.onClick.AddListener(() =>
        {
            InventoryManager.Instance.CraftItem(craftItem);
        });
    }

    /// <summary>
    /// Handles to clear and update crafting panel.
    /// </summary>
    /// <param name="_item"></param>
    public void SetupCraftUI(GearSO _item)
    {
        ClearRequiredMaterialSlot();

        for (int i = 0; i < _item.craftingMaterials.Count; i++)
        {
            materials[i].color = Color.white;
            ItemSlotUI material =  materials[i].GetComponentInChildren<ItemSlotUI>();
            material.itemIcon.sprite = _item.craftingMaterials[i].itemSO.sprite;
            material.itemIcon.color = Color.white; 
            material.itemText.text = _item.craftingMaterials[i].GetQuantity().ToString();
            material.item.itemSO = _item;
            material.itemTooltip = materialTooltip;
        }

        itemName.text = _item.itemName;
        itemDes.text = _item.GetDescription();
        itemIcon.sprite = _item.sprite;
        itemIcon.color = Color.white;
        craftItem = _item;
    }

    /// <summary>
    /// Handles to clear crafting panel.
    /// </summary>
    public void ClearCraftPanel()
    {
        ClearRequiredMaterialSlot();

        itemName.text = "";
        itemDes.text = "";
        itemIcon.sprite = null;
        itemIcon.color = Color.clear;
        craftItem = null;
    }

    /// <summary>
    /// Handles to clear required material slot.
    /// </summary>
    private void ClearRequiredMaterialSlot()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].color = Color.white;
            ItemSlotUI material = materials[i].GetComponentInChildren<ItemSlotUI>();
            material.itemIcon.color = Color.clear;
            material.itemIcon.sprite = null;
            material.itemText.text = "";
        }
    }
}
