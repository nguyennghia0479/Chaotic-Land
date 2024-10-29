using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransitionController : MonoBehaviour
{
    private float defaultCamSize;
    private CinemachineVirtualCamera vcam;
    private CinemachineConfiner confiner;
    private Coroutine currentTransition;
    private readonly float transitionDuration = 1f;
    private readonly float camSize = 5f;

    private void Start()
    {
        defaultCamSize = Camera.main.orthographicSize;
        vcam = FindObjectOfType<CinemachineVirtualCamera>();
        confiner = FindObjectOfType<CinemachineConfiner>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (vcam == null || !collision.TryGetComponent(out Player _)) return;

        StartTransition(camSize);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (vcam == null || !collision.TryGetComponent(out Player _)) return;

        StartTransition(defaultCamSize);
    }

    private void StartTransition(float _targetSize)
    {
        if (currentTransition != null)
        {
            StopCoroutine(currentTransition);
        }
        currentTransition = StartCoroutine(SmoothTransitionRoutine(_targetSize));
    }

    private IEnumerator SmoothTransitionRoutine(float _targetSize)
    {
        float startSize = vcam.m_Lens.OrthographicSize;
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float newSize = Mathf.Lerp(startSize, _targetSize, elapsedTime / transitionDuration);
            vcam.m_Lens.OrthographicSize = newSize;
            yield return null;
        }

        vcam.m_Lens.OrthographicSize = _targetSize;
        currentTransition = null;
    }
}
