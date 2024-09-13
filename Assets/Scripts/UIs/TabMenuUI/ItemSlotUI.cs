using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Item info")]
    public Image itemIcon;
    public TextMeshProUGUI itemText;
    public Inventory item;
    public ItemTooltipUI itemTooltip;
    [SerializeField] protected Image itemDurabilityBar;
    [SerializeField] protected Image itemDurability;
    [Header("Action info")]
    [SerializeField] private Transform[] slotParents;
    [SerializeField] protected Image slotSelected;
    [SerializeField] protected Image itemAction;
    [SerializeField] protected Button actionBtn;
    [SerializeField] protected Button dropBtn;

    protected TabMenuUI tabMenu;
    protected bool isSelected;

    protected virtual void Awake()
    {
        HandleActionButton();
        HandleDropButton();
    }

    protected virtual void Start()
    {
        tabMenu = GetComponentInParent<TabMenuUI>();
    }

    protected void OnDisable()
    {
        HideItemAction();
        isSelected = false;
    }

    #region Item slot
    /// <summary>
    /// Handles to update item slot ui.
    /// </summary>
    public void UpdateItemSlotUI(Inventory _newItem)
    {
        item = _newItem;

        if (item != null)
        {
            itemIcon.color = Color.white;
            itemIcon.sprite = item.itemSO.sprite;
            UpdateDurability(true);

            if (item.GetQuantity() > 1)
            {
                itemText.text = item.GetQuantity().ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
    }

    /// <summary>
    /// Handles to clear slot info.
    /// </summary>
    public void ClearSlot()
    {
        item = null;
        itemIcon.color = Color.clear;
        itemIcon.sprite = null;
        itemText.text = "";

        UpdateDurability(false);
    }

    /// <summary>
    /// Handles to update durability of item.
    /// </summary>
    /// <param name="_isActive"></param>
    private void UpdateDurability(bool _isActive)
    {
        if (itemDurabilityBar == null || itemDurability == null) return;

        float durabilityThreshold = .25f;
        if (_isActive)
        {
            InventoryItem inventoryItem = item as InventoryItem;
            itemDurabilityBar.gameObject.SetActive(true);
            itemDurability.fillAmount = inventoryItem.Durability / inventoryItem.MaxDurability;
            if (itemDurability.fillAmount <= durabilityThreshold)
            {
                itemDurability.color = Color.red;
            }
            else
            {
                itemDurability.color = Color.green;
            }
        }
        else
        {
            itemDurabilityBar.gameObject.SetActive(false);
        }
    }
    #endregion

    #region Pointer
    /// <summary>
    /// Handles to equip gear.
    /// </summary>
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item == null || item.itemSO == null) return;

        HideItemAction();
        ShowItemAction();
        itemTooltip.HideItemTooltip();
    }

    /// <summary>
    /// Handles to show item tooltip.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemTooltip == null || item == null || item.itemSO == null || isSelected) return;

        itemTooltip.ShowItemTooltip(item);
    }

    /// <summary>
    /// Handles to hide item tooltip.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (itemTooltip == null || item == null || item.itemSO == null || isSelected) return;

        itemTooltip.HideItemTooltip();
    }
    #endregion

    #region Item action
    /// <summary>
    /// Handles to hide item action.
    /// </summary>
    private void HideItemAction()
    {
        if (slotParents != null && slotParents.Length > 0)
        {
            foreach (Transform slotParent in slotParents)
            {
                for (int i = 0; i < slotParent.childCount; i++)
                {
                    ItemSlotUI itemSlotUI = slotParent.GetChild(i).GetComponent<ItemSlotUI>();
                    if (itemSlotUI != null && itemSlotUI.slotSelected != null && itemSlotUI.itemAction != null)
                    {
                        itemSlotUI.slotSelected.gameObject.SetActive(false);
                        itemSlotUI.itemAction.gameObject.SetActive(false);
                        itemSlotUI.isSelected = false;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Handles to show item action.
    /// </summary>
    private void ShowItemAction()
    {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            ItemSlotUI itemSlotUI = transform.parent.GetChild(i).GetComponent<ItemSlotUI>();
            if (itemSlotUI != null && itemSlotUI.slotSelected != null && itemSlotUI.itemAction != null && itemSlotUI == this)
            {
                itemSlotUI.slotSelected.gameObject.SetActive(true);
                itemSlotUI.itemAction.gameObject.SetActive(true);
                itemSlotUI.isSelected = true;
            }
        }
    }

    /// <summary>
    /// Handles to equip item.
    /// </summary>
    protected virtual void HandleActionButton()
    {
        if (actionBtn != null)
        {
            actionBtn.onClick.AddListener(() =>
            {
                if (item == null || item.itemSO == null) return;

                if (item.itemSO.type == ItemType.Gear)
                {
                    InventoryManager.Instance.EquipGear(item as InventoryItem);
                    itemTooltip.HideItemTooltip();
                }

                slotSelected.gameObject.SetActive(false);
                itemAction.gameObject.SetActive(false);
            });
        }
    }

    /// <summary>
    /// Handles to drop item.
    /// </summary>
    protected virtual void HandleDropButton()
    {
        if (dropBtn != null)
        {
            dropBtn.onClick.AddListener(() =>
            {
                if (item == null || item.itemSO == null) return;

                InventoryManager.Instance.RemoveItem(item);
                slotSelected.gameObject.SetActive(false);
                itemAction.gameObject.SetActive(false);
            });
        }
    }
    #endregion
}
