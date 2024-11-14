using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] private float baseValue;
    [SerializeField] private List<float> modifiers;

    private float defaultValue;
    private bool isSetDefaultVal;

    public void AddModify(int _modifier)
    {
        if (_modifier <= 0) return;

        baseValue += _modifier;
        defaultValue += _modifier;
    }

    public void RemoveModify(int _modifier)
    {
        if (_modifier <= 0) return;

        baseValue -= _modifier;
        defaultValue -= _modifier;
    }

    /// <summary>
    /// Handles to get value with modify of stat.
    /// </summary>
    /// <returns></returns>
    public float GetValueWithModify()
    {
        float finalValue = baseValue;

        foreach (float modifier in modifiers)
        {
            finalValue += modifier;
        }

        return finalValue;
    }

    public void UpdateBaseValue(float _value)
    {
        if (!isSetDefaultVal)
        {
            isSetDefaultVal = true;
            defaultValue = baseValue;
        }

        baseValue = _value;
    }

    /// <summary>
    /// Handles to get value without modify when increase stat.
    /// </summary>
    /// <param name="_value"></param>
    /// <returns></returns>
    public float GetValueWithoutModify(float _value)
    {
        float value = _value;
        foreach (float modifier in modifiers)
        {
            value -= modifier;
        }

        return value;
    }

    public float DefaultValue
    {
        get { return defaultValue; }
    }
}
