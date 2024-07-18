using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    [SerializeField] private Vector2 knockBackPower;
    [SerializeField] private float knockBackDuration;
    [SerializeField] private float knockBackOffset;

    private Rigidbody2D rb;
    private int knockBackDir;
    private bool isKnockBack;
    private bool isBlock;
    private bool isCriticalAttack;
    private readonly float knockBackWithBlock = .8f;
    private readonly float knockBackWithCritical = .5f;

    public bool IsKnockBack
    {
        get { return isKnockBack; }
    }

    /// <summary>
    /// Handles to setup knock back of the characters who've been damaged.
    /// </summary>
    /// <param name="_entity">Value to determine the character who've been damaged.</param>
    /// <param name="_damageDealer">Value to determine who's made damage.</param>
    /// <param name="_isCriticalAttack">Value to determine if is critical attack or not.</param>
    public void SetupKnockBack(Entity _entity, Transform _damageDealer, bool _isCriticalAttack)
    {
        rb = _entity.Rb;
        isBlock = _entity.IsBlocking;
        isCriticalAttack = _isCriticalAttack;
        knockBackDir = _damageDealer.transform.position.x < transform.position.x ? 1 : -1;
        StartCoroutine(KnockBackRoutine());
    }

    /// <summary>
    /// Handles to perform coroutine of knock back.
    /// </summary>
    /// <returns></returns>
    private IEnumerator KnockBackRoutine()
    {
        isKnockBack = true;
        SetKnockBackVelocity();
        yield return new WaitForSeconds(knockBackDuration);
        isKnockBack = false;
        rb.velocity = Vector2.zero;
    }


    /// <summary>
    /// Handles to set knock back velocity of the characters.
    /// </summary>
    /// <remarks>
    /// The value of velocity will depend on whether the character blocks the attack or receives critical damage.
    /// </remarks>
    private void SetKnockBackVelocity()
    {
        float offset = Random.Range(0, knockBackOffset);
        float knockBackX = knockBackPower.x + offset;
        float knockBackY = knockBackPower.y + offset;

        if (isBlock && !isCriticalAttack)
        {
            knockBackX -= knockBackX * knockBackWithBlock;
            knockBackY -= knockBackY * knockBackWithBlock;
        }
        else if(isCriticalAttack)
        {
            knockBackX += knockBackX * knockBackWithCritical;
            knockBackY += knockBackY * knockBackWithCritical;
            knockBackDuration += knockBackDuration * knockBackWithCritical;
        }

        rb.velocity = new Vector2(knockBackX * knockBackDir, knockBackY);
    }
}
