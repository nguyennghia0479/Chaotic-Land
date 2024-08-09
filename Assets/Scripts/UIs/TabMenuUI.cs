using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum TabMenu
{
    Character, Inventory, Craft, SkillTree
}

public class TabMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject[] tabMenus;
    [SerializeField] private Color inactiveColor;
    [SerializeField] private Transform tabHeader;
    [SerializeField] private float inactiveFontSize, activeFontSize;
    [SerializeField] private Button characterBtn;
    [SerializeField] private Button inventoryBtn;
    [SerializeField] private Button craftBtn;
    [SerializeField] private Button skillTreeBtn;
    [SerializeField] private CraftUI craftUI;

    private Button[] tabBtns;
    private int tabId;

    private void Awake()
    {
        characterBtn.onClick.AddListener(() =>
        {
            ShowCharacterUI();
        });

        inventoryBtn.onClick.AddListener(() =>
        {
            SwitchToTab(TabMenu.Inventory);
        });

        craftBtn.onClick.AddListener(() =>
        {
            ShowCraftUI();
        });

        skillTreeBtn.onClick.AddListener(() =>
        {
            SwitchToTab(TabMenu.SkillTree);
        });
    }

    private void Start()
    {
        GameManager.Instance.OnTab += GameManager_OnTab;

        tabBtns = tabHeader.GetComponentsInChildren<Button>();
        gameObject.SetActive(false);
    }

    private void GameManager_OnTab(object sender, GameManager.OnTabEventArgs e)
    {
        if (e.isOpenTab)
        {
            gameObject.SetActive(true);
            SwitchToTab(TabMenu.Character);
        }
        else
        {
            gameObject.SetActive(false);
            UpdateCharacterUI();
        }
    }

    private void SwitchToTab(TabMenu _tabMenu)
    {
        foreach (GameObject go in tabMenus)
        {
            go.SetActive(false);
        }

        tabId = GetTabMenuId(_tabMenu);
        tabMenus[tabId].SetActive(true);

        UpdateTabButton(tabId);
    }

    private void UpdateTabButton(int _tabId)
    {
        if (_tabId < 0 || _tabId >= tabBtns.Length) return;

        foreach (Button button in tabBtns)
        {
            button.image.color = inactiveColor;
            TextMeshProUGUI btnText = button.GetComponentInChildren<TextMeshProUGUI>();
            if (btnText != null)
            {
                btnText.fontSize = inactiveFontSize;
            }
        }

        if (tabBtns[_tabId].GetComponentInChildren<TextMeshProUGUI>() != null)
        {
            tabBtns[_tabId].image.color = Color.white;
            TextMeshProUGUI activeText = tabBtns[_tabId].GetComponentInChildren<TextMeshProUGUI>();
            activeText.color = Color.black;
            activeText.fontSize = activeFontSize;
        }
    }

    private int GetTabMenuId(TabMenu _tabMenu)
    {
        return _tabMenu switch
        {
            TabMenu.Inventory => 1,
            TabMenu.Craft => 2,
            TabMenu.SkillTree => 3,
            _ => 0,
        };
    }

    private void ShowCharacterUI()
    {
        SwitchToTab(TabMenu.Character);
        UpdateCharacterUI();
    }

    private void ShowCraftUI()
    {
        SwitchToTab(TabMenu.Craft);
        craftUI.ClearCraftPanel();
        CraftListUI craftList = GetComponentInChildren<CraftListUI>();
        craftList.SetDefaultMenuCraft();
    }

    private void UpdateCharacterUI()
    {
        GameObject characterUI = tabMenus[0];
        if (characterUI.TryGetComponent(out CharacterUI ui))
        {
            ui.UpdateCharacterStats();
        }
    }

    public CraftUI CraftUI
    {
        get { return craftUI; }
    }
}
