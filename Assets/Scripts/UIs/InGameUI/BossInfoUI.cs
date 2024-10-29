using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossInfoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bossName;
    [SerializeField] private Slider healthSlider;

    private EnemyStats enemyStats;
    private HealthBarShrinkUI healthBarShrinkUI;
    private bool isBossFight;

    private void Start()
    {
        healthBarShrinkUI = GetComponentInChildren<HealthBarShrinkUI>();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Handles to setup boss info.
    /// </summary>
    /// <param name="_enemyStats"></param>
    /// <param name="_isBossFight"></param>
    public void SetupBossInfo(EnemyStats _enemyStats, bool _isBossFight)
    {
        enemyStats = _enemyStats;
        isBossFight = _isBossFight;

        if (enemyStats != null)
        {
            enemyStats.OnHealthChange += EnemyStats_OnHealthChange;
            bossName.text = enemyStats.GetComponent<Enemy>().BossName;
            healthSlider.maxValue = enemyStats.maxHealth.GetValueWithModify();
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
        if (enemyStats != null)
        {
            enemyStats.OnHealthChange -= EnemyStats_OnHealthChange;
        }
    }

    /// <summary>
    /// Handles to update boss's health.
    /// </summary>
    private void EnemyStats_OnHealthChange(object sender, System.EventArgs e)
    {
        if (healthBarShrinkUI == null) return;

        float healthBarFillAmount = enemyStats.CurrentHealth / healthSlider.maxValue;
        healthBarShrinkUI.DecreaseHealth(healthBarFillAmount);
        healthSlider.value = enemyStats.CurrentHealth;
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
