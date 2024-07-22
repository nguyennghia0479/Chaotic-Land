using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkillController : MonoBehaviour
{
    [SerializeField] private float checkRadius = 5;

    private SpriteRenderer sr;
    private Animator animator;
    private Transform closestTarget;
    private float cloneDuration;
    private float losingSpeed;
    private float chanceToMulti;
    private int facingDir = 1;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        StartCoroutine(FacingClosestEnemy());
    }

    private void Update()
    {
        cloneDuration -= Time.deltaTime;

        CloneTransparent();
    }
    /// <summary>
    /// Handles to set up clone info.
    /// </summary>
    /// <param name="_cloneSkill">The value to store clone skill info.</param>
    /// <param name="_clonePos">The value to determine position of clone.</param>
    /// <param name="_offset">The value to determine offset position of clone.</param>
    public void SetupClone(CloneSkill _cloneSkill, Transform _clonePos, Vector3 _offset)
    {
        cloneDuration = _cloneSkill.CloneDuration;
        losingSpeed = _cloneSkill.LosingSpeed;
        chanceToMulti = _cloneSkill.ChanceToMulti;
     
        transform.position = _clonePos.position + _offset;
        animator.SetInteger("Attack", Random.Range(1, 4));
    }

    /// <summary>
    /// Handles to make clone transparent.
    /// </summary>
    /// <remarks>
    /// Starting to transparent when clone duration out.<br></br>
    /// Destroy clone if transparent completely.
    /// </remarks>
    private void CloneTransparent()
    {
        if (cloneDuration < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * losingSpeed));

            if (sr.color.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// Handles to clone facing the target.
    /// </summary>
    private IEnumerator FacingClosestEnemy()
    {
        yield return null;

        FindClosestEnemy();

        if (closestTarget != null)
        {
            if (transform.position.x > closestTarget.position.x)
            {
                facingDir = -1;
                transform.Rotate(0, -180, 0);
            }
        }
    }

    /// <summary>
    /// Handles to find closest enemy of clone.
    /// </summary>
    private void FindClosestEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, checkRadius);
        float closestDistance = Mathf.Infinity;
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out Enemy enemy))
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = enemy.transform;
                }
            }
        }
    }

    public float ChanceToMulti
    {
        get { return chanceToMulti; }
    }

    public int FacingDir
    {
        get { return facingDir; }
    }
}
