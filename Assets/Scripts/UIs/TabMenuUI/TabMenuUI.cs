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
    [Header("Header menu info")]
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
            SwitchToMenuTab(TabMenu.Inventory);
            InventoryManager.Instance.UpdateStatUIs();
        });

        craftBtn.onClick.AddListener(() =>
        {
            ShowCraftUI();
        });

        skillTreeBtn.onClick.AddListener(() =>
        {
            SwitchToMenuTab(TabMenu.SkillTree);
        });
    }

    private void Start()
    {
        GameManager.Instance.OnTab += GameManager_OnTab;

        tabBtns = tabHeader.GetComponentsInChildren<Button>();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Handles to open/close tab.
    /// </summary>
    private void GameManager_OnTab(object sender, GameManager.OnTabEventArgs e)
    {
        if (e.isOpenTab)
        {
            gameObject.SetActive(true);
            ShowCharacterUI();
        }
        else
        {
            gameObject.SetActive(false);
            UpdateCharacterUI();
        }
    }

    /// <summary>
    /// Handles to switch to menu tab.
    /// </summary>
    /// <param name="_tabMenu"></param>
    private void SwitchToMenuTab(TabMenu _tabMenu)
    {
        foreach (GameObject go in tabMenus)
        {
            go.SetActive(false);
        }

        tabId = GetMenuTabId(_tabMenu);
        tabMenus[tabId].SetActive(true);

        UpdateMenuHeader(tabId);
    }

    /// <summary>
    /// Handles to update menu header.
    /// </summary>
    /// <param name="_tabId"></param>
    private void UpdateMenuHeader(int _tabId)
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

    /// <summary>
    /// Handles to get menu tab id.
    /// </summary>
    /// <param name="_tabMenu"></param>
    /// <returns></returns>
    private int GetMenuTabId(TabMenu _tabMenu)
    {
        return _tabMenu switch
        {
            TabMenu.Inventory => 1,
            TabMenu.Craft => 2,
            TabMenu.SkillTree => 3,
            _ => 0,
        };
    }

    /// <summary>
    /// Handles to show character ui.
    /// </summary>
    private void ShowCharacterUI()
    {
        SwitchToMenuTab(TabMenu.Character);
        UpdateCharacterUI();
    }

    /// <summary>
    /// Handles to show craft ui.
    /// </summary>
    private void ShowCraftUI()
    {
        SwitchToMenuTab(TabMenu.Craft);
        craftUI.ClearCraftPanel();
        CraftListUI craftList = GetComponentInChildren<CraftListUI>();
        craftList.SetDefaultMenuCraft();
    }

    /// <summary>
    /// Handles to update character ui.
    /// </summary>
    private void UpdateCharacterUI()
    {
        foreach (GameObject go in tabMenus)
        {
            if (go.TryGetComponent(out CharacterUI ui))
            {
                ui.UpdateCharacter();
                break;
            }
        }
    }

    public CraftUI CraftUI
    {
        get { return craftUI; }
    }
}
