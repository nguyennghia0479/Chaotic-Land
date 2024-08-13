using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] InGameUI inGameUI;

    private PlayerController playerController;
    private bool isOpenTab;
    private bool isGamePaused;

    public event EventHandler<OnTabEventArgs> OnTab;
    public class OnTabEventArgs : EventArgs
    {
        public bool isOpenTab;
    }

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
            inGameUI.gameObject.SetActive(false);
        }
        else
        {
            Time.timeScale = 1;
            inGameUI.gameObject.SetActive(true);
        }
    }

    public bool IsGamePaused
    {
        get { return isGamePaused; }
    }
}
