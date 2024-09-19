using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    [SerializeField] private string fileName;

    private GameData gameData;
    private DataHandler dataHandler;
    private List<ISaveManager> saveManagers;

    private void Start()
    {
        dataHandler = new DataHandler(Application.persistentDataPath, fileName);
        saveManagers = FindAllSaveManagers(); ;
        LoadGame();
    }

    /// <summary>
    /// Handles to create new game.
    /// </summary>
    private void NewGame()
    {
        gameData = new GameData();
    }

    /// <summary>
    /// Handles to load game.
    /// </summary>
    private void LoadGame()
    {
        gameData = dataHandler.LoadData();

        if (gameData == null)
        {
            Debug.Log("No save game found.");
            NewGame();
        }

        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }
    }

    /// <summary>
    /// Handles to save game.
    /// </summary>
    public void SaveGame()
    {
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }

        dataHandler.SaveData(gameData);
    }

    /// <summary>
    /// Handles to delete save game.
    /// </summary>
    [ContextMenu("Delete save file")]
    public void DeleteSaveGame()
    {
        dataHandler = new DataHandler(Application.persistentDataPath, fileName);
        dataHandler.DeleteData();
    }

    /// <summary>
    /// Handles to check save file existed.
    /// </summary>
    /// <returns></returns>
    public bool HasSaveFile()
    {
        dataHandler = new DataHandler(Application.persistentDataPath, fileName);

        return dataHandler.LoadData() != null;
    }

    /// <summary>
    /// Handles to save game when exited game.
    /// </summary>
    private void OnApplicationQuit()
    {
        SaveGame();
    }

    /// <summary>
    /// Handles to find all save managers.
    /// </summary>
    /// <returns></returns>
    private List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveManager>();

        return new List<ISaveManager>(saveManagers);
    }
}
