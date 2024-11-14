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
    [SerializeField] private GuideUI guideUI;

    private LevelManager levelManager;

    private void Awake()
    {
        continueBtn.onClick.AddListener(() =>
        {
            PlayMenuSound();
            ContinueGame();
        });

        newGameBtn.onClick.AddListener(() =>
        {
            PlayMenuSound();
            guideUI.ShowGuideUI();
        });

        optionsBtn.onClick.AddListener(() =>
        {
            PlayMenuSound();
            optionsUI.ShowOptionsUI();
        });

        quitBtn.onClick.AddListener(() =>
        {
            PlayMenuSound();
            Application.Quit();
        });

    }

    private void Start()
    {
        levelManager = LevelManager.Instance;

        if (SaveManager.Instance != null && !SaveManager.Instance.HasSaveFile())
        {
            continueBtn.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Handles to load game scene.
    /// </summary>
    private void ContinueGame()
    {
        if (levelManager == null)
        {
            levelManager = LevelManager.Instance;
        }

        levelManager.LoadContinueGame();
    }

    /// <summary>
    /// Handles to load new game scene.
    /// </summary>
    public void NewGame()
    {
        if (levelManager == null)
        {
            levelManager = LevelManager.Instance;
        }

        levelManager.LoadNewGame();
    }

    /// <summary>
    /// Handles to play menu sound.
    /// </summary>
    private void PlayMenuSound()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayMenuSound();
        }
    }
}
