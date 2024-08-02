using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpinSkillController : MonoBehaviour
{
    [SerializeField] private GameObject brightFirePrefab;
    [SerializeField] private float brightFireCooldown = .3f;
    [SerializeField] private AilementType ailementType;

    private Player player;
    private float moveTime;
    private float moveSpeed;
    private float maxGrowSize;
    private float scaleSpeed;
    private float spinHitCooldown;
    private Vector3 moveDir;
    private bool canMove;
    private bool canGrow;
    private bool canShrink;
    private bool isSpawnBrightFire;
    private bool isTriggered;
    private float spinHitTimer;
    private readonly float epsilon = .01f;

    private void Update()
    {
        MoveFireSpin();
        GrowFireSpin();
        ShirnkFireSpin();
        FireSpinHit();
    }

    #region Fire spin skill
    /// <summary>
    /// Handles to set up fire spin info.
    /// </summary>
    /// <param name="_fireSpin">The value to store info from fire spin skill.</param>
    public void SetupFireSpin(FireSpinSkill _fireSpin)
    {
        canMove = true;
        moveDir = new(1 * _fireSpin.Player.FacingDir, 0);

        player = _fireSpin.Player;
        moveTime = _fireSpin.MoveTime;
        moveSpeed = _fireSpin.MoveSpeed;
        maxGrowSize = _fireSpin.MaxGrowSize;
        scaleSpeed = _fireSpin.ScaleSpeed;
        spinHitCooldown = _fireSpin.SpinHitCooldown;
    }

    /// <summary>
    /// Handles to trigger fire spin.
    /// </summary>
    public void TriggerFireSpinGrow()
    {
        if (isTriggered || canShrink) return;

        isTriggered = true;
        canMove = false;
        canGrow = true;
    }

    /// <summary>
    /// Handles to move fire spin.
    /// </summary>
    private void MoveFireSpin()
    {
        if (canMove)
        {
            transform.position += moveSpeed * Time.deltaTime * moveDir;
        }

        moveTime -= Time.deltaTime;
        if (moveTime < 0 && canMove)
        {
            canMove = false;
            canGrow = true;
        }
    }

    /// <summary>
    /// Handles to grow fire spin.
    /// </summary>
    /// <remarks>
    /// After grow to max size will start to shrink and spawn bright fire.
    /// </remarks>
    private void GrowFireSpin()
    {
        if (canGrow)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(maxGrowSize, maxGrowSize), scaleSpeed * Time.deltaTime);

            if (transform.localScale.x > maxGrowSize - epsilon || transform.localScale.y > maxGrowSize - epsilon)
            {
                canGrow = false;
                canShrink = true;
            }

            SpawnBrightFire(transform.localScale.x);
        }
    }

    /// <summary>
    /// Handles to shrink fire spin.
    /// </summary>
    /// <remarks>
    /// After fire spin scale to close zero will destroy.
    /// </remarks>
    private void ShirnkFireSpin()
    {
        if (canShrink)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, scaleSpeed * Time.deltaTime);
            if (transform.localScale.x < .5f || transform.localScale.y < .5f)
            {
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// Handles to hit enemy by fire spin.
    /// </summary>
    private void FireSpinHit()
    {
        spinHitTimer -= Time.deltaTime;
        if (spinHitTimer < 0)
        {
            spinHitTimer = spinHitCooldown;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, maxGrowSize);
            foreach (Collider2D collider in colliders)
            {
                if (collider.TryGetComponent(out EnemyStats enemy))
                {
                    if (enemy.GetComponent<Enemy>().IsDead) return;

                    player.Stats.DoMagicDamage(enemy, ailementType);
                }
            }
        }
    }
    #endregion

    #region Spawn bright fire
    /// <summary>
    /// Handles to spawn bright fire.
    /// </summary>
    /// <param name="_localScaleX">The value to determine size of fire spin.</param>
    private void SpawnBrightFire(float _localScaleX)
    {
        if (isSpawnBrightFire) return;

        isSpawnBrightFire = true;
        StartCoroutine(SpawnBrightFireRoutine(_localScaleX));
    }

    /// <summary>
    /// Handles to spawn bright fire by coroutine if fire spin is not shrink.
    /// </summary>
    /// <param name="_localScaleX">The value to determine size of fire spin.</param>
    private IEnumerator SpawnBrightFireRoutine(float _localScaleX)
    {
        while (!canShrink)
        {
            yield return new WaitForSeconds(brightFireCooldown);

            GameObject newBrightFire = Instantiate(brightFirePrefab, transform.position, transform.rotation);
            newBrightFire.GetComponent<BrightFireSkillController>().SetupBrightFire(_localScaleX);
        }
    }
    #endregion
}
