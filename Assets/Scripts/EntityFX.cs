using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;
    private int blinkCount = 0;
    private readonly int maxBlinkCount = 5;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    /// <summary>
    /// Handles to perform fx of character death.
    /// </summary>
    public void PlayDeathFX()
    {
        InvokeRepeating(nameof(DeathFX), 0, .2f);
    }

    /// <summary>
    /// Handles to peform blink sprite renderer color.
    /// </summary>
    /// <remarks>
    /// If blink count is greater than max blink count will destroy charaters.<br></br>
    /// Will be called by PlayDeathFX() method.
    /// </remarks>
    private void DeathFX()
    {
        float alpha = 100f / 255;
        Color color = new(sr.color.r, sr.color.g, sr.color.b, alpha);

        if (sr.color != Color.white)
        {
            sr.color = Color.white;
        }
        else if (sr.color != color)
        {
            sr.color = color;
        }

        blinkCount++;

        if (blinkCount > maxBlinkCount)
        {
            blinkCount = 0;
            Destroy(gameObject);
        }
    }
}
