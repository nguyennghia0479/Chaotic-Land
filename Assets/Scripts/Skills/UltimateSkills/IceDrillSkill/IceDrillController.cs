using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceDrillController : MonoBehaviour
{
    [SerializeField] private GameObject iceBergPrefab;

    private Animator animator;
    private PlayerStats playerStats;
    private bool canMove;
    private EnemyStats enemyStats;
    private int moveSpeed;
    private float freezingDuration;

    private const string HIT = "Hit";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        IceDrillMovement();
    }

    #region Ice drill skill
    /// <summary>
    /// Handles to setup ice drill info.
    /// </summary>
    /// <param name="_iceDrill"></param>
    /// <param name="_target"></param>
    public void SetupIceDrill(IceDrillSkill _iceDrill, Transform _target)
    {
        moveSpeed = _iceDrill.MoveSpeed;
        freezingDuration = _iceDrill.FreezingDuration;
        playerStats = _iceDrill.Player.Stats as PlayerStats;
        canMove = true;
        enemyStats = _target.GetComponent<EnemyStats>();
    }

    /// <summary>
    /// Handles to make movement of ice drill.
    /// </summary>
    private void IceDrillMovement()
    {
        if (canMove && enemyStats != null)
        {
            float moveDelta = moveSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, enemyStats.transform.position, moveDelta);
            transform.right = transform.position - enemyStats.transform.position;

            if (Vector2.Distance(transform.position, enemyStats.transform.position) < 1f)
            {
                HitTarget();
            }
        }
    }

    /// <summary>
    /// Handles animator and make damage when hit target.
    /// </summary>
    private void HitTarget()
    {
        canMove = false;
        animator.SetTrigger(HIT);
        playerStats.DoMagicDamage(enemyStats, AilmentType.None);
        enemyStats.GetComponent<Enemy>().FreezingEffect(freezingDuration);
        PlayIceDrillHitSound();
    }
    #endregion

    #region Animation methods
    /// <summary>
    /// Handles to destroy when finish animation.
    /// </summary>
    private void AnimationSelfDestroy()
    {
        Destroy(transform.parent.gameObject);
    }

    /// <summary>
    /// Handles to create ice berg after hit target.
    /// </summary>
    private void AnimationSpawnIceBerg()
    {
        Transform spawnPos = enemyStats.GetComponent<Enemy>().GroundCheck.transform;
        GameObject newIceBerg = Instantiate(iceBergPrefab, spawnPos.position, Quaternion.identity);
        newIceBerg.GetComponentInChildren<IceBergSkillController>().SetupIceBerg(playerStats, enemyStats);
        PlayIceBergeSound();
    }
    #endregion

    #region Play sound
    /// <summary>
    /// Handles to play ice drill hit  sound.
    /// </summary>
    private void PlayIceDrillHitSound()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayIceDrillHitSound(transform.position);
        }
    }

    /// <summary>
    /// Handles to play ice berge sound.
    /// </summary>
    private void PlayIceBergeSound()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayIceBergeSound(transform.position);
        }
    }
    #endregion
}
