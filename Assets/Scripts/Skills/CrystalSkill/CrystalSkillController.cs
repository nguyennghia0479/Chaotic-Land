using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSkillController : MonoBehaviour
{
    private Animator animator;
    private CircleCollider2D circleCollider;
    private float lifeTime;
    private bool canMove;
    private float moveSpeed;
    private float checkRadius;
    private LayerMask enemyLayerMask;
    private bool isExplore;
    private float lifeTimer;
    private Transform enemyTarget;

    private const string EXPLORE = "Explore";

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer < 0)
        {
            Destroy(gameObject);
        }

        FindRandomTarget();
        MoveCrystalToEnemy();
    }

    /// <summary>
    /// Handles to explore crystal.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Enemy enemy))
        {
            if (!isExplore)
            {
                circleCollider.radius *= 2;
                animator.SetTrigger(EXPLORE);
                isExplore = true;
                canMove = false;
                lifeTimer = lifeTime;
            }

            enemy.SetupKnockBack(transform, false);
        }
    }

    /// <summary>
    /// Handles to set up crystal info.
    /// </summary>
    /// <param name="_crystalSkill">The value to store info from crystal skill.</param>
    public void SetupCrystal(CrystalSkill _crystalSkill)
    {
        lifeTime = _crystalSkill.LifeTime;
        canMove = _crystalSkill.CanMove;
        moveSpeed = _crystalSkill.MoveSpeed;
        checkRadius = _crystalSkill.CheckRadius;
        enemyLayerMask = _crystalSkill.EnemyLayerMask;
        lifeTimer = lifeTime;
    }

    /// <summary>
    /// Handles to find random enemy in radius range.
    /// </summary>
    private void FindRandomTarget()
    {
        if (enemyTarget == null)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, checkRadius, enemyLayerMask);
            if (colliders.Length > 0)
            {
                enemyTarget = colliders[Random.Range(0, colliders.Length)].transform;
            }
        }
    }

    /// <summary>
    /// Handles to move crystal to enemy.
    /// </summary>
    private void MoveCrystalToEnemy()
    {
        if (canMove && enemyTarget != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, enemyTarget.position, moveSpeed * Time.deltaTime);
        }
    }
}
