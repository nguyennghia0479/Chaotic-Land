using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Map Data")]
public class MapSO : ScriptableObject
{
    public Sprite mapIcon;
    public string mapName;
    public int mapRecommendedLevel;
    public string mapSceneName;
}
