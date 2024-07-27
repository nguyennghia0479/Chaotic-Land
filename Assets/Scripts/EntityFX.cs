using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    #region Variables
    [Header("FlashFX info")]
    [SerializeField] private Material flashFXMat;
    [SerializeField] private float flashDuration = .2f;

    [Header("DeathFX info")]
    [SerializeField] private float repeatRate = .2f;
    [SerializeField] private float blinkAlpha = 100;
    [SerializeField] private int maxBlinkCount = 5;

    [Header("IgniteFX info")]
    [SerializeField] private Color[] igniteColors;
    [SerializeField] private float ignitedRepeatRate = .15f;

    [Header("ChilledFX info")]
    [SerializeField] private Color[] chilledColors;
    [SerializeField] private float chilledRepeatRate = .15f;

    private SpriteRenderer sr;
    private Material defaultMat;
    private int blinkCount = 0;
    private int colorCount = 0;
    #endregion

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        defaultMat = sr.material;
    }

    #region DeathFX
    /// <summary>
    /// Handles to perform fx of character death.
    /// </summary>
    public void PlayDeathFX()
    {
        CancelInvoke();
        InvokeRepeating(nameof(DeathFX), 0, repeatRate);
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
        float alpha = blinkAlpha / 255;
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
    #endregion

    #region FlashFX
    /// <summary>
    /// Handles to perform flash when character's be hit.
    /// </summary>
    public void PlayFlashFX()
    {
        StartCoroutine(FlashFXRoutine());
    }

    /// <summary>
    /// Handles to change srpite material in duration when be hit.
    /// </summary>
    private IEnumerator FlashFXRoutine()
    {
        sr.material = flashFXMat;
        yield return new WaitForSeconds(flashDuration);
        sr.material = defaultMat;
    }
    #endregion

    #region AilementFX
    /// <summary>
    /// Handles to perform ignited FX in duration when be ignited.
    /// </summary>
    /// <param name="_ignitedDuration">Time of character be ignited.</param>
    public void PlayIgnitedFX(float _ignitedDuration)
    {
        if (igniteColors.Length <= 0) return;

        InvokeRepeating(nameof(IgniteFX), 0, ignitedRepeatRate);
        StopColorChange(_ignitedDuration);
    }

    /// <summary>
    /// Handles to change sprite color.
    /// </summary>
    private void IgniteFX()
    {
        if (sr.color == igniteColors[0] || colorCount < igniteColors.Length - 1)
        {
            colorCount++;
        }
        else
        {
            colorCount = 0;
        }

        sr.color = igniteColors[colorCount];
    }

    /// <summary>
    /// Handles to perform chilled FX in duration when be chilled.
    /// </summary>
    /// <param name="_chilledDuration">Time of character be chilled.</param>
    public void PlayChilledFX(float _chilledDuration)
    {
        if (chilledColors.Length <= 0) return;

        InvokeRepeating(nameof(ChilledFx), 0, chilledRepeatRate);
        StopColorChange(_chilledDuration);
    }

    /// <summary>
    /// Handles to change sprite color.
    /// </summary>
    private void ChilledFx()
    {
        if (sr.color == chilledColors[0] || colorCount < chilledColors.Length - 1)
        {
            colorCount++;
        }
        else
        {
            colorCount = 0;
        }

        sr.color = chilledColors[colorCount];
    }
    #endregion

    /// <summary>
    /// Handles to stop color chanage
    /// </summary>
    /// <param name="_duration">Time to stop color chanage</param>
    private void StopColorChange(float _duration)
    {
        Invoke(nameof(ResetDefaultColor), _duration);
    }

    /// <summary>
    /// Handles to reset default color of character and cancel invoke.
    /// </summary>
    private void ResetDefaultColor()
    {
        CancelInvoke();
        sr.color = Color.white;
    }
}
