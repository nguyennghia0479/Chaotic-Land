using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupTextUI : MonoBehaviour
{
    [SerializeField] private float lifeTime = 1;
    [SerializeField] private float speed = 1;
    [SerializeField] private float colorLossingSpeed = 1;

    private TextMeshPro popupText;
    private float lifeTimer;

    private void Start()
    {
        popupText = GetComponent<TextMeshPro>();
        lifeTimer = lifeTime;
    }

    private void Update()
    {
        ShowPopupText();
    }

    /// <summary>
    /// Handles to show popup text.
    /// </summary>
    private void ShowPopupText()
    {
        lifeTimer -= Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y + 1), speed * Time.deltaTime);

        if (lifeTimer <= 0)
        {
            float alpha = popupText.color.a - (colorLossingSpeed * Time.deltaTime);
            popupText.color = new Color(popupText.color.r, popupText.color.g, popupText.color.b, alpha);

            if (popupText.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
