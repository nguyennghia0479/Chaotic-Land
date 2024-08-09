using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public event EventHandler<OnTabEventArgs> OnTab;
    public class OnTabEventArgs : EventArgs
    {
        public bool isOpenTab;
    }

    private PlayerController playerController;
    private bool isOpenTab;
    private bool isGamePaused;

    private void OnEnable()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        if (playerController != null)
        {
            playerController.OnTabAction += PlayerController_OnTabAction;
        }
    }

    private void OnDestroy()
    {
        if (playerController != null)
        {
            playerController.OnTabAction -= PlayerController_OnTabAction;
        }
    }

    private void PlayerController_OnTabAction(object sender, EventArgs e)
    {
        isOpenTab = !isOpenTab;
        TogglePauseGamed(isOpenTab);

        OnTab?.Invoke(this, new OnTabEventArgs
        {
            isOpenTab = isOpenTab,
        });
    }

    private void TogglePauseGamed(bool _isGamePaused)
    {
        isGamePaused = _isGamePaused;
        if (_isGamePaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public bool IsGamePaused
    {
        get { return isGamePaused; }
    }
}
