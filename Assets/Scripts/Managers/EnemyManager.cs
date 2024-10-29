using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    /// <summary>
    /// Handles to setup enemies's level
    /// </summary>
    public void SetupEnemyLevel()
    {
        PlayerManager playerManager = PlayerManager.Instance;
        if (playerManager == null) return;

        Enemy[] enemies = GetComponentsInChildren<Enemy>();
        int playerLevel = playerManager.CurrentLevel;
        foreach (Enemy enemy in enemies)
        {
            if (enemy.IsCombat) continue;

            if (enemy.TryGetComponent(out EnemyStats enemyStats))
            {
                int enemyLevel = enemyStats.Level;
                if (playerLevel - enemyLevel > 2)
                {
                    enemyLevel = playerLevel - 2;
                }

                if (enemy.IsBoss && enemyStats.Level < playerLevel)
                {
                    enemyLevel = playerLevel;
                }

                enemyStats.LevelUp(enemyLevel);
            }
        }
    }

    /// <summary>
    /// Handles to find boss.
    /// </summary>
    /// <returns></returns>
    public Enemy FindBoss()
    {
        Enemy[] enemies = GetComponentsInChildren<Enemy>();
        Enemy boss = null;

        foreach (Enemy enemy in enemies)
        {
            if (enemy.IsBoss)
            {
                boss = enemy;
                break;
            }
        }

        return boss;
    }
}
