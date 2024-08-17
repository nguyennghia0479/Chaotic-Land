using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipUI : MonoBehaviour
{
    [Header("Tooltip position info")]
    [SerializeField] protected RectTransform canvasRectTransform;
    [SerializeField] protected RectTransform rectTransform;
    [SerializeField] private Vector2 padding;
    [SerializeField] private float offset;

    protected Vector2 defaultPos;

    protected void Start()
    {
        defaultPos = transform.position;
        gameObject.SetActive(false);
    }

    protected void Update()
    {
        UpdateTooltipPosition();
    }

    /// <summary>
    /// Handles to update tooltip position.
    /// </summary>
    private void UpdateTooltipPosition()
    {
        Vector2 anchoredPoint = Input.mousePosition / canvasRectTransform.localScale.x;
        anchoredPoint.x += padding.x;
        anchoredPoint.y += padding.y;

        if (anchoredPoint.x + rectTransform.rect.width > canvasRectTransform.rect.width - padding.x)
        {
            anchoredPoint.x = canvasRectTransform.rect.width - rectTransform.rect.width - padding.x * offset;
        }

        if (anchoredPoint.y + rectTransform.rect.height > canvasRectTransform.rect.height - padding.y)
        {
            anchoredPoint.y = canvasRectTransform.rect.height - rectTransform.rect.height - padding.y * offset;
        }

        rectTransform.anchoredPosition = anchoredPoint;
    }

    /// <summary>
    /// Handles to set default position.
    /// </summary>
    public void SetDefaultPosition()
    {
        transform.position = defaultPos;
    }
}
