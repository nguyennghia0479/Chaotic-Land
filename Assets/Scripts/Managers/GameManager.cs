using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] InGameUI inGameUI;
    [SerializeField] TooltipUI[] tooltipUIs;

    private PlayerController playerController;
    private bool isOpenTab;
    private bool isGamePaused;

    #region Events
    public event EventHandler<OnTabEventArgs> OnTab;
    public class OnTabEventArgs : EventArgs
    {
        public bool isOpenTab;
    }

    public event EventHandler<OnGamePausedEventArgs> OnGamePaused;
    public class OnGamePausedEventArgs : EventArgs
    {
        public bool isGamePaused;
    }
    #endregion

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.OnTabAction += PlayerController_OnTabAction;
            playerController.OnPauseAction += PlayerController_OnPauseAction;
        }
    }

    private void OnDestroy()
    {
        if (playerController != null)
        {
            playerController.OnTabAction -= PlayerController_OnTabAction;
            playerController.OnPauseAction -= PlayerController_OnPauseAction;
        }
    }

    #region Game paused
    /// <summary>
    /// Handles to resume game.
    /// </summary>
    public void ResumeGame()
    {
        isGamePaused = false;
        InvokeGamePaused();
    }

    /// <summary>
    /// Handles to toggle game paused.
    /// </summary>
    private void PlayerController_OnPauseAction(object sender, EventArgs e)
    {
        isGamePaused = !isGamePaused;
        InvokeGamePaused();

        if (isOpenTab)
        {
            InvokeOpenTab();
        }
    }

    /// <summary>
    /// Handles to invoke game paused event.
    /// </summary>
    private void InvokeGamePaused()
    {
        TogglePauseGamed(isGamePaused);

        OnGamePaused?.Invoke(this, new OnGamePausedEventArgs
        {
            isGamePaused = isGamePaused,
        });
    }
    #endregion

    #region Tab info
    /// <summary>
    /// Handles to toggle open tab.
    /// </summary>
    private void PlayerController_OnTabAction(object sender, EventArgs e)
    {
        if (isGamePaused && !isOpenTab) return;

        InvokeOpenTab();
        TogglePauseGamed(isOpenTab);
    }

    /// <summary>
    /// Handles to invoke onTab event.
    /// </summary>
    private void InvokeOpenTab()
    {
        isOpenTab = !isOpenTab;
        OnTab?.Invoke(this, new OnTabEventArgs
        {
            isOpenTab = isOpenTab,
        });
    }
    #endregion

    /// <summary>
    /// Handles to toggle pause game.
    /// </summary>
    /// <param name="_isGamePaused"></param>
    private void TogglePauseGamed(bool _isGamePaused)
    {
        isGamePaused = _isGamePaused;
        if (_isGamePaused)
        {
            Time.timeScale = 0;
            inGameUI.gameObject.SetActive(false);
        }
        else
        {
            Time.timeScale = 1;
            inGameUI.gameObject.SetActive(true);
            HideAllTooltip();
        }
    }

    /// <summary>
    /// Handles to hide all tooltip.
    /// </summary>
    private void HideAllTooltip()
    {
        foreach (TooltipUI tooltipUI in tooltipUIs)
        {
            tooltipUI.SetDefaultPosition();
            tooltipUI.gameObject.SetActive(false);
        }
    }

    public bool IsGamePaused
    {
        get { return isGamePaused; }
    }

    public InGameUI InGameUI
    {
        get { return inGameUI; }
    }
}
