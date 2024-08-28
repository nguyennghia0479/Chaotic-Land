using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatsUI : MonoBehaviour
{
    [SerializeField] private Entity entity;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Slider healthSlider;

    private EnemyStats stats;

    /// <summary>
    /// Handles to subscribe events.
    /// </summary>
    private void OnEnable()
    {
        if (entity != null)
        {
            entity.OnFlipped += Entity_OnFlipped;

            if (entity.TryGetComponent(out EnemyStats stats))
            {
                this.stats = stats;
                UpdateLevelUI();
                stats.OnInitHealth += EntityStats_OnInitHealth;
                stats.OnHealthChange += EntityStats_OnHealthChange;
            }
        }
    }


    /// <summary>
    /// Handles to unsubscribe events.
    /// </summary>
    private void OnDestroy()
    {
        if (entity != null)
        {
            entity.OnFlipped -= Entity_OnFlipped;
        }

        if (stats != null)
        {
            stats.OnInitHealth -= EntityStats_OnInitHealth;
            stats.OnHealthChange -= EntityStats_OnHealthChange;
        }
    }

    /// <summary>
    /// Handles to update enemy's level.
    /// </summary>
    public void UpdateLevelUI()
    {
        levelText.text = stats.Level.ToString();
    }

    /// <summary>
    /// Handles to flip UI when character flipped.
    /// </summary>
    private void Entity_OnFlipped(object sender, System.EventArgs e)
    {
        rectTransform.Rotate(0, -180, 0);
    }

    /// <summary>
    /// Handles to initial health of the character.
    /// </summary>
    private void EntityStats_OnInitHealth(object sender, System.EventArgs e)
    {
        healthSlider.maxValue = stats.CurrentHealth;
        healthSlider.value = stats.CurrentHealth;
    }

    /// <summary>
    /// Handles to update health of the character.
    /// </summary>
    private void EntityStats_OnHealthChange(object sender, System.EventArgs e)
    {
        healthSlider.value = stats.CurrentHealth;
        if (healthSlider.value <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
