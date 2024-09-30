using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrikeSkillController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EnemyStats enemy))
        {
            if (enemy.GetComponent<Enemy>().IsDead) return;

            PlayerStats player = PlayerManager.Instance.Player.GetComponent<PlayerStats>();
            player.DoMagicDamage(enemy, AilmentType.None);
            PlayThunderSound();
        }
    }

    /// <summary>
    /// Handles to play thunder sound.
    /// </summary>
    private void PlayThunderSound()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayThunderSound(transform.position);
        }
    }
}
