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

    private void Awake()
    {
        resumeBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.ResumeGame();
        });

        mainMenuBtn.onClick.AddListener(() =>
        {
            Debug.Log("Return to main menu");
        });

        optionsBtn.onClick.AddListener(() =>
        {
            HideGamePausedUI();
            optionsUI.ShowOptionsUI();
        });

        quitBtn.onClick.AddListener(() =>
        {
            Debug.Log("Quit game!");
            Application.Quit();
        });
    }

    private void Start()
    {
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;

        HideGamePausedUI();
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

    public void ShowGamePausedUI()
    {
        gameObject.SetActive(true);
    }

    private void HideGamePausedUI()
    {
        gameObject.SetActive(false);
    }
}
