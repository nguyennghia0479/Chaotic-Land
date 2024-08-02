using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Item Effect/Freeze Effect")]
public class FreezeEffectSO : ItemEffectSO
{
    [SerializeField] private float radius = 10;
    [SerializeField] private float percentage = .15f;
    [SerializeField] private float freezeDuration = 2;
    [SerializeField] private float slowPercentage = .3f;

    public override void ExecuteItemEffect(Transform _target)
    {
        PlayerStats player = PlayerManager.Instance.Player.GetComponent<PlayerStats>();
        int chanceToFreeze = Mathf.RoundToInt(player.maxHealth.GetValue() * percentage);

        if (player.CurrentHealth < chanceToFreeze && InventoryManager.Instance.CanUseArmorEffect())
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(player.transform.position, radius);

            foreach (Collider2D collider in colliders)
            {
                if (collider.TryGetComponent(out Enemy enemy))
                {
                    enemy.FX.PlayChilledFX(freezeDuration);
                    enemy.SlowEntityEffect(slowPercentage, freezeDuration);
                }
            }
        }
    }
}
