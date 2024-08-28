using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBergSkillController : MonoBehaviour
{
    private Animator animator;
    private PlayerStats playerStats;
    private EnemyStats enemyStats;

    private const string END = "End";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Handles to setup ice berg info.
    /// </summary>
    /// <param name="_playerStats"></param>
    /// <param name="_enemyStats"></param>
    public void SetupIceBerg(PlayerStats _playerStats, EnemyStats _enemyStats)
    {
        playerStats = _playerStats;
        enemyStats = _enemyStats;
    }

    private void AnimationHitTarget()
    {
        if (playerStats == null || enemyStats == null) return;

        playerStats.DoMagicDamage(enemyStats, AilmentType.None);
    }

    private void AnimationIceBergEnd()
    {
        animator.SetTrigger(END);
    }

    private void AnimationSelfDestroy()
    {
        Destroy(transform.parent.gameObject);
    }
}
