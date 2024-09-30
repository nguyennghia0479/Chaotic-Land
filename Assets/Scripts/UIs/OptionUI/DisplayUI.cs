using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayUI : MonoBehaviour
{
    [SerializeField] private Toggle windowModeToggle;
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private bool isFullScreen;
    private Resolution[] resolutions;
    private List<Resolution> filteredResolution;
    private int currentResolutionIdx;

    private void Awake()
    {
        windowModeToggle.onValueChanged.AddListener(delegate { CheckWindowModeToggle(); });

        resolutionDropdown.onValueChanged.AddListener(UpdateResolution);
    }

    private void Start()
    {
        isFullScreen = false;
        SetupResolutionDropdown();
    }

    /// <summary>
    /// Handles to toggle window mode.
    /// </summary>
    private void CheckWindowModeToggle()
    {
        isFullScreen = !isFullScreen;
        Screen.SetResolution(Screen.width, Screen.height, isFullScreen);
    }

    /// <summary>
    /// Handles to update resolution.
    /// </summary>
    /// <param name="_index"></param>
    private void UpdateResolution(int _index)
    {
        Resolution resolution = filteredResolution[_index];
        Screen.SetResolution(resolution.width, resolution.height, isFullScreen);
    }

    /// <summary>
    /// Handles to setup resolution dropdown.
    /// </summary>
    private void SetupResolutionDropdown()
    {
        resolutionDropdown.ClearOptions();
        resolutions = Screen.resolutions;
        filteredResolution = new List<Resolution>();
        double currentRefreshRate = Screen.currentResolution.refreshRateRatio.value;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].refreshRateRatio.value == currentRefreshRate)
            {
                filteredResolution.Add(resolutions[i]);
            }
        }

        List<string> options = new();
        for (int i = 0; i < filteredResolution.Count; i++)
        {
            string option = filteredResolution[i].width + "x" + filteredResolution[i].height;
            options.Add(option);

            if (filteredResolution[i].width == Screen.width && filteredResolution[i].height == Screen.height)
            {
                currentResolutionIdx = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIdx;
        resolutionDropdown.RefreshShownValue();
    }
}
