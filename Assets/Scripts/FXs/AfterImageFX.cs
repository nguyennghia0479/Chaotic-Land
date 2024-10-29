using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageFX : MonoBehaviour
{
    private SpriteRenderer sr;
    private float colorLossingSpeed;

    private void Update()
    {
        DecreaseColor();
    }

    /// <summary>
    /// Handles to decreasing color after image.
    /// </summary>
    private void DecreaseColor()
    {
        float alpha = sr.color.a - (colorLossingSpeed * Time.deltaTime);
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);

        if (sr.color.a <= 0)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Handles to setup after image fx.
    /// </summary>
    /// <param name="_sprite"></param>
    /// <param name="_colorLossingSpeed"></param>
    public void SetupAfterImageFX(Sprite _sprite, float _colorLossingSpeed)
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = _sprite;
        colorLossingSpeed = _colorLossingSpeed;
    }
}
