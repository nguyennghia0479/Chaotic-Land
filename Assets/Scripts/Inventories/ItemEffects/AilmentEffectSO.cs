using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Item Effect/Ailment Effect")]
public class AilmentEffectSO : ItemEffectSO
{
    [SerializeField] private AilmentType ailmentType;

    /// <summary>
    /// Handles to execute item effect.
    /// </summary>
    /// <param name="_target"></param>
    public override void ExecuteItemEffect(Transform _target)
    {
        Player player = PlayerManager.Instance.Player;
        int attackCombo = player.AttackState.AttackCombo;
        int finalCombo = 2;

        if (attackCombo == finalCombo && _target.TryGetComponent(out EnemyStats enemy))
        {
            player.GetComponent<PlayerStats>().DoMagicDamage(enemy, ailmentType);
        }
    }
}
