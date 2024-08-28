using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private Enemy[] enemies;
    private PlayerManager playerManager;

    private void Start()
    {
        enemies = GetComponentsInChildren<Enemy>();
        playerManager = PlayerManager.Instance;

        SetupEnemyLevel();
    }

    /// <summary>
    /// Handles to setup enemies's level
    /// </summary>
    private void SetupEnemyLevel()
    {
        if (playerManager == null) return;

        int playerLevel = playerManager.Level;
        foreach (Enemy enemy in enemies)
        {
            if (enemy.TryGetComponent(out EnemyStats enemyStats))
            {
                int enemyLevel = enemyStats.Level;
                if (playerLevel - enemyLevel > 2)
                {
                    enemyLevel = playerLevel - 2;
                }

                enemyStats.LevelUp(enemyLevel);
            }

            
        }
    }
}
