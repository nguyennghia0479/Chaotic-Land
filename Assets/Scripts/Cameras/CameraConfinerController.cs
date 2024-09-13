using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConfinerController : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D cameraConfiner;
    [SerializeField] private PolygonCollider2D arenaConfiner;

    private CinemachineConfiner confiner;

    private void Start()
    {
        confiner = GetComponentInChildren<CinemachineConfiner>();
    }

    public void EnterArena()
    {
        StartCoroutine(DelayRoutine(arenaConfiner));
    }

    public void ExitArena()
    {
        StartCoroutine(DelayRoutine(cameraConfiner));
    }

    private void SetBoundingShap(PolygonCollider2D _collider)
    {
        if (_collider == null) return;

        confiner.m_BoundingShape2D = _collider;
    }

    private IEnumerator DelayRoutine(PolygonCollider2D _collider)
    {
        confiner.m_Damping = 2;
        SetBoundingShap(_collider);
        yield return new WaitForSeconds(.5f);
        confiner.m_Damping = 0;
    }
}
