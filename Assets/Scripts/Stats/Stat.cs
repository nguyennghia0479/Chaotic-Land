using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] private int baseValue;
    [SerializeField] private List<int> modifiers;

    public void AddModify(int _modifier)
    {
        if (_modifier <= 0) return;

        modifiers.Add(_modifier);
    }

    public void RemoveModify(int _modifier)
    {
        modifiers.Remove(_modifier);
    }

    public int GetValue()
    {
        int finalValue = baseValue;

        foreach (int modifier in modifiers)
        {
            finalValue += modifier;
        }

        return finalValue;
    }
}
