using UnityEngine;

public class Arena : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    [SerializeField] private BoxCollider2D arenaTrigger;
    [SerializeField] private CameraConfinerController confinerController;

    private bool isBossFight;

    /// <summary>
    /// Handles to setup boss info and arena.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isBossFight) return;

        if (!collision.TryGetComponent(out Player _)) return;

        if (enemy.TryGetComponent(out EnemyStats enemyStats))
        {
            isBossFight = true;
            enemy.BossInfoUI.SetupBossInfo(enemyStats, isBossFight);
            confinerController.EnterArena();
            MusicManager.Instance.TooglePauseMusic();
        }
    }

    /// <summary>
    /// Handles to turn off trigger arena collision when in boss fight.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out Player _)) return;

        arenaTrigger.isTrigger = false;
    }

    /// <summary>
    /// Handles to ending boss fight.
    /// </summary>
    public void BossFightEnd()
    {
        isBossFight = false;
        enemy.BossInfoUI.SetupBossInfo(null, isBossFight);
        confinerController.ExitArena();
        MusicManager.Instance.TooglePauseMusic();
        Destroy(gameObject);
    }
}
