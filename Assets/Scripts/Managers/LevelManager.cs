using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private LoadingUI loadingUI;
    [SerializeField] private Slider loadingSlider;
    
    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Handles to load main menu scene.
    /// </summary>
    public void LoadMainMenu()
    {
        LoadScene("MainMenu");
    }

    /// <summary>
    /// Handles to load new game scene.
    /// </summary>
    public void LoadNewGame()
    {
        LoadScene("OakWoodsMap2");
    }

    /// <summary>
    /// Handles to show and start loading scene.
    /// </summary>
    /// <param name="_sceneName"></param>
    public void LoadScene(string _sceneName)
    {
        loadingUI.ShowLoadingUI();
        StartCoroutine(LoadSceneAsyncRoutine(_sceneName));
    }

    /// <summary>
    /// Handles to load current scene.
    /// </summary>
    public void LoadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    /// <summary>
    /// Handles to load scene async.
    /// </summary>
    /// <param name="_sceneName"></param>
    /// <returns></returns>
    private IEnumerator LoadSceneAsyncRoutine(string _sceneName)
    {
        Time.timeScale = 1.0f;
        float maxProgress = .9f;
        loadingSlider.value = 0f;
        
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(_sceneName);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            loadingSlider.value = Mathf.Clamp01(asyncOperation.progress / maxProgress);

            if (asyncOperation.progress >= maxProgress )
            {
                
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }

        loadingSlider.value = 1;
        loadingUI.HideLoadingUI();
    }
}
