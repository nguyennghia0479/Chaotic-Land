using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarShrinkUI : MonoBehaviour
{
    [SerializeField] private Image damageBar;

    private float healthBarAmount;
    private float shrinkTimer;
    private readonly float shrinkTimerMax = 1f;

    private void Start()
    {
        healthBarAmount = 1;
        damageBar.fillAmount = 1;
    }

    private void Update()
    {
        ShrinkHealth();
    }

    /// <summary>
    /// Handles to shrink health by time.
    /// </summary>
    private void ShrinkHealth()
    {
        shrinkTimer -= Time.deltaTime;
        if (shrinkTimer < 0)
        {
            if (healthBarAmount < damageBar.fillAmount)
            {
                float shrinkSpeed = 1;
                damageBar.fillAmount -= shrinkSpeed * Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// Handles to decrease health UI.
    /// </summary>
    /// <param name="_healthBarAmount"></param>
    public void DecreaseHealth(float _healthBarAmount)
    {
        shrinkTimer = shrinkTimerMax;
        healthBarAmount = _healthBarAmount;
    }

    /// <summary>
    /// Handles to increase health UI.
    /// </summary>
    /// <param name="_healthBarAmount"></param>
    public void IncreaseHealth(float _healthBarAmount)
    {
        healthBarAmount = _healthBarAmount;
        damageBar.fillAmount = _healthBarAmount;
    }
}
