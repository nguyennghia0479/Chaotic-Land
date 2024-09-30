using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePausedUI : MonoBehaviour
{
    [SerializeField] private Button resumeBtn;
    [SerializeField] private Button mainMenuBtn;
    [SerializeField] private Button optionsBtn;
    [SerializeField] private Button quitBtn;
    [SerializeField] private OptionsUI optionsUI;

    private LevelManager levelManager;

    private void Awake()
    {
        resumeBtn.onClick.AddListener(() =>
        {
            PlayMenuSound();
            GameManager.Instance.ResumeGame();
        });

        mainMenuBtn.onClick.AddListener(() =>
        {
            PlayMenuSound();
            MainMenu();       
        });

        optionsBtn.onClick.AddListener(() =>
        {
            PlayMenuSound();
            HideGamePausedUI();
            optionsUI.ShowOptionsUI();
        });

        quitBtn.onClick.AddListener(() =>
        {
            PlayMenuSound();
            Debug.Log("Quit game!");
            Application.Quit();
        });
    }

    private void Start()
    {
        levelManager = LevelManager.Instance;
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;

        HideGamePausedUI();
    }

    /// <summary>
    /// Handles to load main menu scene.
    /// </summary>
    private void MainMenu()
    {
        if (levelManager == null)
        {
            levelManager = LevelManager.Instance;
        }

        levelManager.LoadMainMenu();
    }

    private void GameManager_OnGamePaused(object sender, GameManager.OnGamePausedEventArgs e)
    {
        if (e.isGamePaused)
        {
            ShowGamePausedUI();
        }
        else
        {
            HideGamePausedUI();
        }
    }

    /// <summary>
    /// Handles to show game pause ui.
    /// </summary>
    public void ShowGamePausedUI()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Handles to hide game pause ui.
    /// </summary>
    private void HideGamePausedUI()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Handles to play menu sounds.
    /// </summary>
    private void PlayMenuSound()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayMenuSound();
        }
    }
}
