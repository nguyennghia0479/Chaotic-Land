using System.Collections;
using UnityEngine;

public class Arena : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    [SerializeField] private GameObject arenaTrigger;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private BoxCollider2D arenaTriggerCollider;
    [SerializeField] private CameraConfinerController confinerController;

    private bool isBossFight;

    /// <summary>
    /// Handles to setup boss info and arena.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isBossFight ||!collision.TryGetComponent(out Player _)) return;

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
        if (!arenaTriggerCollider.isTrigger || groundLayerMask.value == 0 || !collision.TryGetComponent(out Player _)) return;

        int layer = (int)Mathf.Log(groundLayerMask.value, 2);
        arenaTrigger.layer = layer;
        arenaTriggerCollider.isTrigger = false;
    }

    /// <summary>
    /// Handles to ending boss fight.
    /// </summary>
    public void BossFightEnd()
    {
        enemy.BossInfoUI.SetupBossInfo(null, isBossFight);
        StartCoroutine(ExitArenaRoutine());
    }

    private IEnumerator ExitArenaRoutine()
    {
        yield return new WaitForSeconds(3);

        isBossFight = false;
        confinerController.ExitArena();
        MusicManager.Instance.TooglePauseMusic();
        Destroy(gameObject);
    }
}
