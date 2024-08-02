using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Item Effect/Heal Effect")]
public class HealEffectSO : ItemEffectSO
{
    [SerializeField][Range(0f, 1f)] private float healPercentage;

    public override void ExecuteItemEffect(Transform _target)
    {
        PlayerStats playerStats = PlayerManager.Instance.Player.GetComponent<PlayerStats>();

        int healPoint = Mathf.RoundToInt(playerStats.maxHealth.GetValue() * healPercentage);
        playerStats.IncreaseHealth(healPoint);
    }
}
