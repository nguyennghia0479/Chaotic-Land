using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum OptionMenu
{
    Audio, Input, Others
}

public class OptionsUI : MonoBehaviour
{
    [Header("Switch option menu info")]
    [SerializeField] private GameObject[] optionMenus;
    [SerializeField] private Transform options;
    [SerializeField] private Button audioBtn;
    [SerializeField] private Button inputBtn;
    [SerializeField] private Button othersBtn;
    [SerializeField] private Color inactiveColor;

    [Space]
    [SerializeField] private Button closeBtn;
    [SerializeField] private GamePausedUI gamePausedUI;

    private Button[] optionsBtn;

    private void Awake()
    {
        audioBtn.onClick.AddListener(() =>
        {
            SwitchToOptionMenu(OptionMenu.Audio);
        });

        inputBtn.onClick.AddListener(() =>
        {
            SwitchToOptionMenu(OptionMenu.Input);
        });

        othersBtn.onClick.AddListener(() =>
        {
            SwitchToOptionMenu(OptionMenu.Others);
        });

        closeBtn.onClick.AddListener(() =>
        {
            HideOptionsUI();
            gamePausedUI.ShowGamePausedUI();
        });
    }

    private void Start()
    {
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        optionsBtn = options.GetComponentsInChildren<Button>();
        HideOptionsUI();
    }

    #region Options menu
    /// <summary>
    /// Handles to switch option menu.
    /// </summary>
    /// <param name="_option"></param>
    private void SwitchToOptionMenu(OptionMenu _option)
    {
        foreach (GameObject go in optionMenus)
        {
            go.SetActive(false);
        }

        int menuId = GetOptionMenu(_option);
        optionMenus[menuId].SetActive(true);

        UpdateOptionMenu(menuId);
    }

    /// <summary>
    /// Handles to update option menu.
    /// </summary>
    /// <param name="_menuId"></param>
    private void UpdateOptionMenu(int _menuId)
    {
        if (_menuId < 0 || _menuId >= optionsBtn.Length) return;

        foreach (Button button in optionsBtn)
        {
            button.image.color = inactiveColor;
            button.GetComponentInChildren<TextMeshProUGUI>().color = inactiveColor;
        }

        optionsBtn[_menuId].image.color = Color.white;
        optionsBtn[_menuId].GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
    }

    /// <summary>
    /// Handles to get option menu id.
    /// </summary>
    /// <param name="_option"></param>
    /// <returns></returns>
    private int GetOptionMenu(OptionMenu _option)
    {
        return _option switch
        {
            OptionMenu.Input => 1,
            OptionMenu.Others => 2,
            _ => 0
        };
    }
    #endregion

    #region Show, hide options ui
    private void GameManager_OnGamePaused(object sender, GameManager.OnGamePausedEventArgs e)
    {
        if (!e.isGamePaused)
        {
            HideOptionsUI();
        }
    }

    /// <summary>
    /// Handles to show options ui.
    /// </summary>
    public void ShowOptionsUI()
    {
        SwitchToOptionMenu(OptionMenu.Audio);
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Handles to hide options ui.
    /// </summary>
    private void HideOptionsUI()
    {
        gameObject.SetActive(false);
    }
    #endregion
}
