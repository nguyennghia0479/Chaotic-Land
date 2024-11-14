using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideUI : MonoBehaviour
{
    [Header("Main Menu")]
    [SerializeField] private Button startBtn;
    [SerializeField] private MainMenuUI mainMenuUI;

    [Header("Game Scene")]
    [SerializeField] private Button closeBtn;
    [SerializeField] private GamePausedUI gamePausedUI;

    private void Awake()
    {
        if (startBtn != null)
        {
            startBtn.onClick.AddListener(() =>
            {
                PlayMenuSound();
                HideGuideUI();

                if (mainMenuUI != null) {
                    mainMenuUI.NewGame();
                }
            });
        }

        if (closeBtn != null)
        {
            closeBtn.onClick.AddListener(() =>
            {
                PlayMenuSound();
                HideGuideUI();

                if (gamePausedUI != null)
                {
                    gamePausedUI.ShowGamePausedUI();
                }
            });
        }
    }

    private void Start()
    {
        HideGuideUI();
    }

    public void ShowGuideUI()
    {
        gameObject.SetActive(true);
    }

    private void HideGuideUI()
    {
        gameObject.SetActive(false);
    }

    private void PlayMenuSound()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayMenuSound();
        }
    }
}
