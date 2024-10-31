using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeUI : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider slider;
    [SerializeField] private string param;
    [SerializeField] private float multiplier = 30;
    [SerializeField] private float offset = 0;

    private readonly float defaultVol = .6f;

    private void Start()
    {
        slider.value = PlayerPrefs.GetFloat(param, defaultVol);
        SetSliderValue(slider.value);
        slider.onValueChanged.AddListener(SetSliderValue);
    }

    private void SetSliderValue(float _value)
    {
        float mixerValue = Mathf.Log10(_value) * multiplier + offset;
        audioMixer.SetFloat(param, mixerValue);
        PlayerPrefs.SetFloat(param, _value);
        PlayerPrefs.Save();
    }
}
