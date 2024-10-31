using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapSelectionUI : MonoBehaviour
{
    [SerializeField] private GameObject mapPrefab;
    [SerializeField] private List<MapSO> mapList;
    [SerializeField] private Transform mapParrent;
    [SerializeField] private Button playBtn;
    [SerializeField] private Button closeBtn;

    private GameScene gameScene;
    private LevelManager levelManager;

    private void Awake()
    {
        if (playBtn != null)
        {
            playBtn.onClick.AddListener(() =>
            {
                PlayMenuSound();
                LoadMap();
            });
        }

        if (closeBtn != null)
        {
            closeBtn.onClick.AddListener(() =>
            {
                GameManager.Instance.CloseMapSelection();
                PlayMenuSound();
                HideMapSelectionUI();
            });
        }
    }

    private void Start()
    {
        levelManager = LevelManager.Instance;
        GameManager.Instance.OnOpenMap += GameManager_OnOpenMap;
        SetupMapSelectionList();
        HideMapSelectionUI();
    }

    private void OnDisable()
    {
        SetMapSceneName(GameScene.None);
    }

    private void GameManager_OnOpenMap(object sender, GameManager.OnOpenMapEventArgs e)
    {
        if (e.isOpenMap)
        {
            ShowMapSelectionUI();
        }
        else
        {
            HideMapSelectionUI();
        }
    }

    /// <summary>
    /// Handles to show map selection ui.
    /// </summary>
    private void ShowMapSelectionUI()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Handles to hide map selection ui.
    /// </summary>
    private void HideMapSelectionUI()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Handles to update play button UI.
    /// </summary>
    private void UpdatePlayButton()
    {
        TextMeshProUGUI playText = playBtn.GetComponentInChildren<TextMeshProUGUI>();

        if (gameScene == GameScene.None)
        {
            float alpha = 100;
            Color color = playText.color;
            playText.color = new Color(color.r, color.g, color.b, alpha / 255);
            playBtn.enabled = false;
        }
        else
        {
            playText.color = Color.white;
            playBtn.enabled = true;
        }
    }

    #region Map
    /// <summary>
    /// Handles to set map scene name.
    /// </summary>
    /// <param name="_gameScene"></param>
    public void SetMapSceneName(GameScene _gameScene)
    {
        gameScene = _gameScene;
        UpdatePlayButton();
    }

    /// <summary>
    /// Handels to setup map selection list.
    /// </summary>
    private void SetupMapSelectionList()
    {
        foreach (MapSO map in mapList)
        {
            GameObject newMap = Instantiate(mapPrefab, mapParrent);
            newMap.GetComponent<MapUI>().SetupMap(map.mapIcon, map.mapName, map.mapRecommendedLevel, map.mapSceneName);
        }
    }

    /// <summary>
    /// Handles to load map scene.
    /// </summary>
    private void LoadMap()
    {
        if (gameScene == GameScene.None) return;

        if (levelManager == null)
        {
            levelManager = LevelManager.Instance;
        }

        levelManager.SaveGame();
        levelManager.LoadScene(gameScene);

    }
    #endregion

    /// <summary>
    /// Handles to play menu sound.
    /// </summary>
    private void PlayMenuSound()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayMenuSound();
        }
    }
}
