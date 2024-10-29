using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private float trapDamage = 20;

    private PlayerStats playerStats;
    private bool isTrapped;
    private float hitTimer;
    private readonly float hitTimerMax = 1;

    private void Update()
    {
        DamageByTrap();
    }

    /// <summary>
    /// Handles to make damage by trap to player.
    /// </summary>
    private void DamageByTrap()
    {
        if (isTrapped)
        {
            hitTimer -= Time.deltaTime;
            if (hitTimer < 0)
            {
                hitTimer = hitTimerMax;
                playerStats.DecreaseHealth(trapDamage);
            }
        }
    }

    /// <summary>
    /// Handles to check collision enter trap.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isTrapped) return;

        if (collision.transform.TryGetComponent(out PlayerStats _playerStats))
        {
            isTrapped = true;
            hitTimer = 0;
            playerStats = _playerStats;
        }
        else if (collision.transform.TryGetComponent(out Enemy enemy))
        {
            enemy.InstantDeath();
        }
        else
        {
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isTrapped = false;
    }
}
