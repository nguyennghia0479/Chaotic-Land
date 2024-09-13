using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapUI : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image map;
    [SerializeField] private Image mapIcon;
    [SerializeField] private TextMeshProUGUI mapName;
    [SerializeField] private TextMeshProUGUI mapRecommendedLevel;

    private string mapSceneName;
    private MapSelectionUI mapSelectionUI;

    private void Start()
    {
        mapSelectionUI = GetComponentInParent<MapSelectionUI>();
    }

    private void OnDisable()
    {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            MapUI mapUI = transform.parent.GetChild(i).GetComponent<MapUI>();
            mapUI.map.color = new Color(map.color.r, map.color.g, map.color.b, 0f);
        }
    }

    /// <summary>
    /// Handles to setup map scene name when selected.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            MapUI mapUI = transform.parent.GetChild(i).GetComponent<MapUI>();
            Color color = map.color;
            float alpha = 0;
            if (mapUI == this)
            {
                alpha = 1;
            }
            mapUI.map.color = new Color(color.r, color.g, color.b, alpha);
        }

        mapSelectionUI.SetMapSceneName(mapSceneName);
    }

    /// <summary>
    /// Handles to setup map info.
    /// </summary>
    /// <param name="_sprite"></param>
    /// <param name="_name"></param>
    /// <param name="_recommendedLevel"></param>
    /// <param name="_mapSceneName"></param>
    public void SetupMap(Sprite _sprite, string _name, int _recommendedLevel, string _mapSceneName)
    {
        mapIcon.sprite = _sprite;
        mapName.text = _name;
        mapRecommendedLevel.text = _recommendedLevel.ToString();
        mapSceneName = _mapSceneName;
    }
}
