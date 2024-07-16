using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraBounds : MonoBehaviour
{
    private PolygonCollider2D polygonCollider;
    private Vector2 minBound;
    private Vector2 maxBound;

    private void Start()
    {
        polygonCollider = FindObjectOfType<PolygonCollider2D>();

        if (polygonCollider != null)
        {
            minBound = polygonCollider.bounds.min;
            maxBound = polygonCollider.bounds.max;
        }
    }

    private void LateUpdate()
    {
        Vector2 playerPos = transform.position;
        playerPos.x = Mathf.Clamp(playerPos.x, minBound.x, maxBound.x);
        playerPos.y = Mathf.Clamp(playerPos.y, minBound.y, maxBound.y);
        transform.position = playerPos;
    }
}
