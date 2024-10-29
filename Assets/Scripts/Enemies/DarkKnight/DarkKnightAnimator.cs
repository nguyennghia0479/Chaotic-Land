using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkKnightAnimator : EnemyAnimator
{
    private DarkKnight darkKnight;

    protected override void Awake()
    {
        base.Awake();

        darkKnight = GetComponentInParent<DarkKnight>();
    }

    private void AnimationHealing()
    {
        if (darkKnight.TryGetComponent(out EnemyStats enemyStats))
        {
            enemyStats.IncreaseHealth(Mathf.RoundToInt(enemyStats.maxHealth.GetValueWithModify() * darkKnight.PercentHealing));
        }
    }
}
