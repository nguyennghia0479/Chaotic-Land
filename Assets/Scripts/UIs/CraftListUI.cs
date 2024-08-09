using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftListUI : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject itemCraftPrefab;
    [SerializeField] private Transform craftListParent;
    [SerializeField] private List<GearSO> itemToCraftList;
    [SerializeField] private Image menuBG;
    [SerializeField] private Image icon;
    [SerializeField] private Color inactiveColor;

    private TabMenuUI tabMenu;

    private void Start()
    {
        tabMenu = GetComponentInParent<TabMenuUI>();
    }

    public void SetDefaultMenuCraft()
    {
        menuBG.color = Color.white;
        icon.color = Color.white;
        transform.parent.GetChild(0).GetComponent<CraftListUI>().SetupCraftList();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        menuBG.color = Color.white;
        icon.color = Color.white;
        tabMenu.CraftUI.ClearCraftPanel();
        SetupCraftList();
    }

    private void SetupCraftList()
    {
        UpdateMenu();

        for (int i = 0; i < craftListParent.childCount; i++)
        {
            Destroy(craftListParent.GetChild(i).gameObject);
        }

        foreach (GearSO itemCraft in itemToCraftList)
        {
            GameObject newItem = Instantiate(itemCraftPrefab, craftListParent);
            newItem.GetComponent<CraftSlotUI>().SetupCraftItem(itemCraft);
        }
    }

    private void UpdateMenu()
    {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            CraftListUI craftList = transform.parent.GetChild(i).GetComponent<CraftListUI>();
            if (craftList == this)
                continue;

            craftList.menuBG.color = inactiveColor;
            craftList.icon.color = inactiveColor;
        }
    }
}
