using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrightFireSkillController : MonoBehaviour
{
    private Rigidbody2D rb;
    private readonly float timeToDestroy = 5;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        transform.up = -rb.velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Enemy enemy))
        {
            enemy.SetupKnockBack(transform, false);
        }

        Destroy(gameObject);
    }

    /// <summary>
    /// Handles to set up bright fire info.
    /// </summary>
    /// <param name="_fireSpinSize">The value to determine size of fire spin.</param>
    public void SetupBrightFire(float _fireSpinSize)
    {
        rb.velocity = new Vector3(Random.Range(-_fireSpinSize, _fireSpinSize), -_fireSpinSize);
        Destroy(gameObject, timeToDestroy);
    }
}
