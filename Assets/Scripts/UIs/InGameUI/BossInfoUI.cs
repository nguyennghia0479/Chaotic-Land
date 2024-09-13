using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossInfoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bossName;
    [SerializeField] private Slider healthSlider;

    private EnemyStats stats;
    private bool isBossFight;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Handles to setup boss info.
    /// </summary>
    /// <param name="_enemyStats"></param>
    /// <param name="_isBossFight"></param>
    public void SetupBossInfo(EnemyStats _enemyStats, bool _isBossFight)
    {
        stats = _enemyStats;
        isBossFight = _isBossFight;

        if (stats != null)
        {
            stats.OnHealthChange += EnemyStats_OnHealthChange;
            healthSlider.maxValue = stats.maxHealth.GetValueWithModify();
            healthSlider.value = healthSlider.maxValue;
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (stats != null)
        {
            stats.OnHealthChange -= EnemyStats_OnHealthChange;
        }
    }

    /// <summary>
    /// Handles to update boss's health.
    /// </summary>
    private void EnemyStats_OnHealthChange(object sender, System.EventArgs e)
    {
        healthSlider.value = stats.CurrentHealth;
        if (healthSlider.value <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public bool IsBossFight
    {
        get { return isBossFight; }
    }
}
