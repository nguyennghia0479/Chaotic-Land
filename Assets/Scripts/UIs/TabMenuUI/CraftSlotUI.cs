using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftSlotUI : ItemSlotUI
{
    protected override void Start()
    {
        base.Start();
    }

    /// <summary>
    /// Handles to craft item.
    /// </summary>
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (tabMenu == null || tabMenu.CraftUI == null) return;

        tabMenu.CraftUI.SetupCraftUI(item.itemSO as GearSO);
        PlayMenuSound();
    }

    /// <summary>
    /// Handles to setup craft item info.
    /// </summary>
    /// <param name="_itemSO"></param>
    public void SetupCraftItem(GearSO _itemSO)
    {
        if (_itemSO == null) return;

        item.itemSO = _itemSO;
        itemIcon.sprite = _itemSO.sprite;
        itemText.text = _itemSO.name;
    }

    /// <summary>
    /// Handles to play menu sound.
    /// </summary>
    private void PlayMenuSound()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayMenuSound();
        }
    }
}
