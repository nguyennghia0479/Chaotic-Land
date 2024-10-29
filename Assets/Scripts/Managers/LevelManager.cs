using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    private Animator animator;
    private SaveManager saveManager;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        saveManager = SaveManager.Instance;
    }

    /// <summary>
    /// Handles to load main menu scene.
    /// </summary>
    public void LoadMainMenu()
    {
        SaveGame();
        LoadScene("MainMenu");
    }

    /// <summary>
    /// Handles to load new game scene.
    /// </summary>
    public void LoadNewGame()
    {
        if (saveManager == null)
        {
            saveManager = SaveManager.Instance;
        }

        saveManager.DeleteSaveGame();
        LoadScene("OakWoodsSF");
    }

    /// <summary>
    /// Handles to load continue game.
    /// </summary>
    public void LoadContinueGame()
    {
        LoadScene("MedievalCastleSF");
    }

    /// <summary>
    /// Handles to show and start loading scene.
    /// </summary>
    /// <param name="_sceneName"></param>
    public void LoadScene(string _sceneName)
    {
        StartCoroutine(LoadSceneRoutine(_sceneName));
    }

    /// <summary>
    /// Handles to load current scene.
    /// </summary>
    public void LoadCurrentScene()
    {
        SaveGame();
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    /// <summary>
    /// Handles to save game.
    /// </summary>
    public void SaveGame()
    {
        if (saveManager == null)
        {
            saveManager = SaveManager.Instance;
        }

        saveManager.SaveGame();
    }

    private IEnumerator LoadSceneRoutine(string _sceneName)
    {
        Time.timeScale = 1f;
        animator.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(_sceneName);
    }
}
