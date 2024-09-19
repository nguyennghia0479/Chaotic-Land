using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveManager
{
    public void SaveData(ref GameData _gameData);

    public void LoadData(GameData _gameData);
}
