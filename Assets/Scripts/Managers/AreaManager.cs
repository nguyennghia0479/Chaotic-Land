using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AreaGate
{
    WestGate, EastGate
}

public class AreaManager : Singleton<AreaManager>
{
    private string areaSpawn;
    private GameScene currentScene;

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Handles to set area entrance name.
    /// </summary>
    /// <param name="_entrance"></param>
    public void SetAreaEntranceName(string _entrance)
    {
        areaSpawn = _entrance;
    }

    /// <summary>
    /// Handles to set current scene.
    /// </summary>
    /// <param name="_currentScene"></param>
    public void SetCurrentScene(GameScene _currentScene)
    {
        currentScene = _currentScene;
    }

    public string AreaSpawn
    {
        get { return areaSpawn; }
    }

    public GameScene CurrentScene
    {
        get { return currentScene; }
    }
}
