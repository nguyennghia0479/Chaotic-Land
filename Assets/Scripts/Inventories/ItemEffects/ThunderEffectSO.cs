using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Item Effect/Thunder Effect")]
public class ThunderEffectSO : ItemEffectSO
{
    [SerializeField] private GameObject thunderPrefab;

    public override void ExecuteItemEffect(Transform _target)
    {
        GameObject newThunder = Instantiate(thunderPrefab, _target.position, Quaternion.identity);
        newThunder.GetComponent<ThunderStrikeSkill>().SetupThunder(_target);
    }
}
