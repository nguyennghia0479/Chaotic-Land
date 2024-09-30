using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeUI : MonoBehaviour, ISaveManager
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider slider;
    [SerializeField] private string param;
    [SerializeField] private float multiplier = 30;
    [SerializeField] private float offset = 0;

    private void Start()
    {
        slider.onValueChanged.AddListener(SetSliderValue);
    }

    private void SetSliderValue(float _value)
    {
        audioMixer.SetFloat(param, Mathf.Log10(_value) * multiplier + offset);

        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.SaveGame(this);
        }
    }

    public void SaveData(ref GameData _gameData)
    {
        if (_gameData == null) return;

        if (_gameData.volumes.TryGetValue(param, out var _))
        {
            _gameData.volumes.Remove(param);
            _gameData.volumes.Add(param, slider.value);
        }
        else
        {
            _gameData.volumes.Add(param, slider.value);
        }
    }

    public void LoadData(GameData _gameData)
    {
        if (_gameData == null) return;

        if (_gameData.volumes.TryGetValue(param, out var value))
        {
            slider.value = value;
        }
    }
}
