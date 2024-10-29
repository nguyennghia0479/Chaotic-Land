using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : Singleton<AreaManager>
{
    private string areaSpawn;

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

    public string AreaSpawn
    {
        get { return areaSpawn; }
    }
}
