using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFlicker : MonoBehaviour
{
    [SerializeField] private float minIntensity = .5f;
    [SerializeField] private float maxIntensity = 1f;
    [SerializeField] private float minOuter = 3f;
    [SerializeField] private float maxOuter = 4f;
    [SerializeField] private float flickerInterval = .1f;

    private Light2D candleLight;

    private void Start()
    {
        candleLight = GetComponentInChildren<Light2D>();
        if (candleLight != null)
        {
            InvokeRepeating(nameof(Flicker), 0, flickerInterval);
        }
    }

    private void Flicker()
    {
        float targetIntensity = Random.Range(minIntensity, maxIntensity);
        float normalized = (targetIntensity - minIntensity) / (maxIntensity - minIntensity);
        float targetOuterRadius = Mathf.Lerp(minOuter, maxOuter, normalized);
        StartCoroutine(ChangeInstensityRoutine(candleLight.intensity, targetIntensity, candleLight.pointLightOuterRadius, targetOuterRadius));
    }

    private IEnumerator ChangeInstensityRoutine(float _currentIntensity, float _targetIntensity, float _currentOuterRadius, float _targetOuterRadius)
    {
        float elapseTime = 0;
        while (elapseTime < flickerInterval)
        {
            elapseTime += Time.deltaTime;
            candleLight.intensity = Mathf.Lerp(_currentIntensity, _targetIntensity, elapseTime / flickerInterval);
            candleLight.pointLightOuterRadius = Mathf.Lerp(_currentOuterRadius, _targetOuterRadius, elapseTime / flickerInterval);
            yield return null;
        }

        candleLight.intensity = _targetIntensity;
        candleLight.pointLightOuterRadius = _targetOuterRadius;
    }
}
