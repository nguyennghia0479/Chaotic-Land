using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    #region Variables
    [Header("FlashFX info")]
    [SerializeField] protected Material flashFXMat;
    [SerializeField] protected float flashDuration = .2f;
    [SerializeField] protected float flashRepeatRate = .2f;

    [Header("DeathFX info")]
    [SerializeField] protected float repeatRate = .2f;
    [SerializeField] protected float blinkAlpha = 100;

    [Header("IgniteFX info")]
    [SerializeField] protected Color[] igniteColors;
    [SerializeField] protected float ignitedRepeatRate = .15f;

    [Header("ChilledFX info")]
    [SerializeField] protected Color[] chilledColors;
    [SerializeField] protected float chilledRepeatRate = .15f;

    [Header("HitFX info")]
    [SerializeField] protected GameObject hitFXPrefab;
    [SerializeField] protected GameObject criticalHitFXPrefab;
    [SerializeField] protected float minOffset = -.5f;
    [SerializeField] protected float maxOffset = .5f;
    [SerializeField] protected float minRotate = -90f;
    [SerializeField] protected float maxRotate = 90f;
    [SerializeField] protected float criticalScale = 1.5f;
    [SerializeField] protected float lifeTime = .5f;

    [Header("AilmentFX info")]
    [SerializeField] protected ParticleSystem igniteFX;
    [SerializeField] protected ParticleSystem chilledFX;

    [Header("Popup text info")]
    [SerializeField] private GameObject popupTextPrefab;

    protected SpriteRenderer sr;
    protected Material defaultMat;
    protected int colorCount = 0;
    protected bool isFlashing;
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
    protected void DeathFX()
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
    protected IEnumerator FlashFXRoutine()
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

        if (igniteFX != null)
        {
            igniteFX.Play();
        }

        InvokeRepeating(nameof(IgniteFX), 0, ignitedRepeatRate);
        StopColorChange(_ignitedDuration);
    }

    /// <summary>
    /// Handles to change sprite color.
    /// </summary>
    protected void IgniteFX()
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

        if (chilledFX != null)
        {
            chilledFX.Play();
        }

        InvokeRepeating(nameof(ChilledFx), 0, chilledRepeatRate);
        StopColorChange(_chilledDuration);
    }

    /// <summary>
    /// Handles to change sprite color.
    /// </summary>
    protected void ChilledFx()
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

    #region StunnedFX
    public void PlayStunnedFX()
    {
        CancelInvoke();
        InvokeRepeating(nameof(StunnedFX), 0, flashRepeatRate);
    }

    private void StunnedFX()
    {
        if (sr == null) return;

        if (!isFlashing)
        {
            isFlashing = true;
            sr.material = flashFXMat;
        }
        else
        {
            isFlashing = false;
            sr.material = defaultMat;
        }
    }
    #endregion

    /// <summary>
    /// Handles to play hit FX.
    /// </summary>
    /// <param name="_isCriticalHit"></param>
    public void PlayHitFX(bool _isCriticalHit)
    {
        float xPosition = Random.Range(minOffset, maxOffset);
        float yPosition = Random.Range(minOffset, maxOffset);
        float zRotation = Random.Range(-minRotate, maxRotate);
        Vector3 hitRotation = new(0, 0, zRotation);
        Vector3 hitScale = Vector3.one;

        GameObject hitFX = hitFXPrefab;
        if (_isCriticalHit)
        {
            hitFX = criticalHitFXPrefab;
            hitScale = new Vector3(criticalScale, criticalScale, criticalScale);
        }

        GameObject newHitFX = Instantiate(hitFX, transform.position + new Vector3(xPosition, yPosition), Quaternion.identity);
        newHitFX.transform.Rotate(hitRotation);
        newHitFX.transform.localScale = hitScale;
        Destroy(newHitFX, lifeTime);
    }

    #region Popup text
    /// <summary>
    /// Handles to play popup text.
    /// </summary>
    /// <param name="_text"></param>
    public void PlayPopupText(string _text)
    {
        PopupTextUI popupText = FindObjectOfType<PopupTextUI>();
        if (popupText != null)
        {
            Destroy(popupText.gameObject);
        }

        Vector3 offset = new(0, Random.Range(1f, 3f));
        GameObject newPopupText = Instantiate(popupTextPrefab, transform.position + offset, Quaternion.identity);
        newPopupText.GetComponent<TextMeshPro>().text = _text;
    }

    /// <summary>
    /// Handles to play popup damage text.
    /// </summary>
    /// <param name="_damageText"></param>
    /// <param name="_isCriticalHit"></param>
    public void PlayPopupDamageText(string _damageText, bool _isCriticalHit)
    {
        float xPosition = Random.Range(-2f, 2f);
        float yPosition = Random.Range(1f, 3f);
        Vector3 offset = new(xPosition, yPosition);

        GameObject newPopupText = Instantiate(popupTextPrefab, transform.position + offset, Quaternion.identity);
        newPopupText.GetComponent<TextMeshPro>().text = _damageText;
        if (_isCriticalHit )
        {
            newPopupText.GetComponent<TextMeshPro>().color = Color.red;
        }
        else
        {
            newPopupText.GetComponent<TextMeshPro>().color = Color.yellow;
        }
    }
    #endregion

    /// <summary>
    /// Handles to stop color chanage
    /// </summary>
    /// <param name="_duration">Time to stop color chanage</param>
    protected void StopColorChange(float _duration)
    {
        Invoke(nameof(ResetDefaultColor), _duration);
    }

    /// <summary>
    /// Handles to reset default color of character and cancel invoke.
    /// </summary>
    public void ResetDefaultColor()
    {
        if (igniteFX != null)
        {
            igniteFX.Stop();
        }

        if (chilledFX != null)
        {
            chilledFX.Stop();
        }

        CancelInvoke();
        sr.color = Color.white;
    }
}
