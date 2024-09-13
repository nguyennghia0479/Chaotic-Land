using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private InGameUI inGameUI;
    [SerializeField] private BossInfoUI bossInfoUI;
    [SerializeField] private TooltipUI[] tooltipUIs;
    [SerializeField] private Checkpoint[] checkpoints;
    [SerializeField] private MapSelectionUI mapSelectionUI;

    private PlayerController playerController;
    private bool isOpenTab;
    private bool isGamePaused;
    private bool isOpenMap;

    #region Events
    public event EventHandler<OnOpenTabEventArgs> OnOpenTab;
    public class OnOpenTabEventArgs : EventArgs
    {
        public bool isOpenTab;
    }

    public event EventHandler<OnGamePausedEventArgs> OnGamePaused;
    public class OnGamePausedEventArgs : EventArgs
    {
        public bool isGamePaused;
    }

    public event EventHandler<OnOpenMapEventArgs> OnOpenMap;
    public class OnOpenMapEventArgs : EventArgs
    {
        public bool isOpenMap;
    }
    #endregion

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.OnTabAction += PlayerController_OnTabAction;
            playerController.OnPauseAction += PlayerController_OnPauseAction;
            playerController.OnInteractAction += PlayerController_OnInteractAction;
        }
    }

    private void OnDestroy()
    {
        if (playerController != null)
        {
            playerController.OnTabAction -= PlayerController_OnTabAction;
            playerController.OnPauseAction -= PlayerController_OnPauseAction;
            playerController.OnInteractAction -= PlayerController_OnInteractAction;
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

        if (isOpenMap)
        {
            InvokeOpenMap();
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
    /// Handles to invoke onOpenTab event.
    /// </summary>
    private void InvokeOpenTab()
    {
        isOpenTab = !isOpenTab;
        OnOpenTab?.Invoke(this, new OnOpenTabEventArgs
        {
            isOpenTab = isOpenTab,
        });
    }
    #endregion

    #region Map selection.
    /// <summary>
    /// Handles to close map selection.
    /// </summary>
    public void CloseMapSelection()
    {
        isOpenMap = false;
        ResumeGame();
    }

    /// <summary>
    /// Handles to open map selection
    /// </summary>
    private void PlayerController_OnInteractAction(object sender, EventArgs e)
    {
        if ((isGamePaused && !isOpenMap) || isOpenMap) return;

        InvokeOpenMap();
        TogglePauseGamed(isOpenMap);
    }

    /// <summary>
    /// Handles to invoke onOpenMap event.
    /// </summary>
    private void InvokeOpenMap()
    {
        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (checkpoint.IsNearCheckpoint && checkpoint.IsBurning)
            {
                isOpenMap = !isOpenMap;
                OnOpenMap?.Invoke(this, new OnOpenMapEventArgs
                {
                    isOpenMap = isOpenMap,
                });
            }
        }
    }
    #endregion

    /// <summary>
    /// Handles to toggle pause game.
    /// </summary>
    /// <param name="_isGamePaused"></param>
    private void TogglePauseGamed(bool _isGamePaused)
    {
        if (inGameUI == null || bossInfoUI == null) return;

        isGamePaused = _isGamePaused;
        if (_isGamePaused)
        {
            Time.timeScale = 0;
            inGameUI.gameObject.SetActive(false);
            bossInfoUI.gameObject.SetActive(false);
        }
        else
        {
            Time.timeScale = 1;
            inGameUI.gameObject.SetActive(true);
            bossInfoUI.gameObject.SetActive(bossInfoUI.IsBossFight);
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
