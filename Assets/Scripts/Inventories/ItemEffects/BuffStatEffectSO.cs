using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Item Effect/Buff Stat Effect")]
public class BuffStatEffectSO : ItemEffectSO
{
    [SerializeField] private BuffType buffType;
    [SerializeField] private int modify;
    [SerializeField] private float duration = 15;

    public override void ExecuteItemEffect(Transform _target)
    {
        PlayerStats player = PlayerManager.Instance.Player.GetComponent<PlayerStats>();
        Stat statToBuff = player.GetStatByBuffType(buffType);

        if (statToBuff != null)
        {
            player.BuffStat(statToBuff, modify, duration);
        }
    }
}
