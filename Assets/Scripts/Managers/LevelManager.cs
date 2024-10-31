using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameScene
{
    OakWoodsS1, OakWoodsS2, OakWoodsSF,
    MedievalCastleS1, MedievalCastleS2, MedievalCastleSF,
    MainMenu, None
}

public class LevelManager : Singleton<LevelManager>, ISaveManager
{
    private Animator animator;
    private SaveManager saveManager;
    private GameScene currentScene;

    private const string START = "Start";

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        saveManager = SaveManager.Instance;
        currentScene = AreaManager.Instance.CurrentScene;
    }

    /// <summary>
    /// Handles to load main menu scene.
    /// </summary>
    public void LoadMainMenu()
    {
        SaveGame();
        LoadScene(GameScene.MainMenu);
        AreaManager.Instance.SetAreaEntranceName(null);
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
        currentScene = GameScene.OakWoodsS1;
        LoadScene(currentScene);
    }

    /// <summary>
    /// Handles to load continue game.
    /// </summary>
    public void LoadContinueGame()
    {
        if (currentScene == GameScene.None || currentScene == GameScene.MainMenu) return;

        LoadScene(currentScene);
    }

    /// <summary>
    /// Handles to start load scene.
    /// </summary>
    /// <param name="_gameScene"></param>
    public void LoadScene(GameScene _gameScene)
    {
        if (_gameScene == GameScene.None) return;

        if (_gameScene != GameScene.MainMenu)
        {
            AreaManager.Instance.SetCurrentScene(_gameScene);
        }

        StartCoroutine(LoadSceneRoutine(_gameScene.ToString()));
    }

    /// <summary>
    /// Handles to load active scene.
    /// </summary>
    public void LoadActiveScene()
    {
        SaveGame();
        Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.name);
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

    /// <summary>
    /// Handles to show and start loading scene.
    /// </summary>
    /// <param name="_sceneName"></param>
    private IEnumerator LoadSceneRoutine(string _sceneName)
    {
        Time.timeScale = 1f;
        animator.SetTrigger(START);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(_sceneName);
    }

    /// <summary>
    /// Handles to save game scene active.
    /// </summary>
    /// <param name="_gameData"></param>
    public void SaveData(ref GameData _gameData)
    {
        Scene activeScene = SceneManager.GetActiveScene();
        if (activeScene.name == GameScene.MainMenu.ToString()) return;

        currentScene = AreaManager.Instance.CurrentScene;
        if (currentScene == GameScene.None || currentScene == GameScene.MainMenu) return;

        _gameData.currentScene = currentScene;
    }

    /// <summary>
    /// Handles to load last game scene active.
    /// </summary>
    /// <param name="_gameData"></param>
    public void LoadData(GameData _gameData)
    {
        if (_gameData.currentScene == GameScene.None || _gameData.currentScene == GameScene.MainMenu) return;

        currentScene = _gameData.currentScene;
    }
}
