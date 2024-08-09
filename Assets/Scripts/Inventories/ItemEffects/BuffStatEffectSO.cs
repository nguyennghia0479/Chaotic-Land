using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType
{
    PhysicsDamage, MagicDamage, Armor, Resistance
}

[CreateAssetMenu(menuName = "Data/Item Effect/Buff Stat Effect")]
public class BuffStatEffectSO : ItemEffectSO
{
    [SerializeField] private BuffType buffType;
    [SerializeField] private int modify;
    [SerializeField] private float duration = 15;

    private PlayerStats playerStats;

    /// <summary>
    /// Handles to execute item effect.
    /// </summary>
    /// <param name="_target"></param>
    public override void ExecuteItemEffect(Transform _target)
    {
        playerStats = PlayerManager.Instance.Player.GetComponent<PlayerStats>();
        Stat statToBuff = GetStatByBuffType(buffType);

        if (statToBuff != null)
        {
            playerStats.BuffStat(statToBuff, modify, duration);
        }
    }

    /// <summary>
    /// Handles to get stat to buff.
    /// </summary>
    /// <param name="_buffType"></param>
    /// <returns></returns>
    private Stat GetStatByBuffType(BuffType _buffType)
    {
        return _buffType switch
        {
            BuffType.PhysicsDamage => playerStats.physicsDamage,
            BuffType.MagicDamage => playerStats.magicDamage,
            BuffType.Armor => playerStats.armor,
            BuffType.Resistance => playerStats.resistance,
            _ => null,
        };
    }
}
