using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerFX : EntityFX
{
    [Header("After image FX info")]
    [SerializeField] private GameObject afterImagePrefab;
    [SerializeField] private float colorLossingSpeed;
    [SerializeField] private float afterImageCooldown;

    private float afterImageCooldownTimer;

    private void Update()
    {
        afterImageCooldownTimer -= Time.deltaTime;
    }

    /// <summary>
    /// Handles to play after image.
    /// </summary>
    public void PlayAfterImage()
    {
        if (afterImageCooldownTimer < 0)
        {
            afterImageCooldownTimer = afterImageCooldown;
            GameObject newAfterImagePrefab = Instantiate(afterImagePrefab, transform.position, transform.rotation);
            newAfterImagePrefab.GetComponent<AfterImageFX>().SetupAfterImageFX(sr.sprite, colorLossingSpeed);
        }
    }
}
