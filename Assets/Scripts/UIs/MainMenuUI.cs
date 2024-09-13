using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button continueBtn;
    [SerializeField] private Button newGameBtn;
    [SerializeField] private Button optionsBtn;
    [SerializeField] private Button quitBtn;
    [SerializeField] private OptionsUI optionsUI;

    private LevelManager levelManager;

    private void Awake()
    {
        continueBtn.onClick.AddListener(() =>
        {
            Debug.Log("Continue");
        });

        newGameBtn.onClick.AddListener(() =>
        {
            NewGame();
        });

        optionsBtn.onClick.AddListener(() =>
        {
            optionsUI.ShowOptionsUI();
        });

        quitBtn.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    private void Start()
    {
        levelManager = LevelManager.Instance;
    }

    /// <summary>
    /// Handles to load new game scene.
    /// </summary>
    private void NewGame()
    {
        if (levelManager == null)
        {
            levelManager = LevelManager.Instance;
        }

        levelManager.LoadNewGame();
    }
}
