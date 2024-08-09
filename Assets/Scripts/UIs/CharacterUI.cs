using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUI : MonoBehaviour
{
    private StatUI[] statUIs;

    private void Start()
    {
        statUIs = GetComponentsInChildren<StatUI>();
    }

    public void UpdateCharacterStats()
    {
        foreach(StatUI stat in statUIs)
        {
            stat.UpdateStatUI();
        }
    }
}
