using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private GameObject tooltip;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI burnText;
    [SerializeField] private TextMeshProUGUI unBurnText;

    private Animator animator;
    private bool isBurning;
    private bool isNearCheckpoint;
    private const string BURN = "Burn";

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        icon.gameObject.SetActive(false);
        tooltip.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Active();
        }
    }

    /// <summary>
    /// Handles to show checkpoint tooltip.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isNearCheckpoint = true;
        tooltip.SetActive(true);
    }

    /// <summary>
    /// Handles to hide checkpoint tooltip.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        isNearCheckpoint = false;
        tooltip.SetActive(false);
    }

    /// <summary>
    /// Handles to active checkpoint when boss died.
    /// </summary>
    public void Active()
    {
        if (animator == null) return;

        isBurning = true;
        animator.SetTrigger(BURN);
        icon.gameObject.SetActive(true);
        burnText.gameObject.SetActive(true);
        unBurnText.gameObject.SetActive(false);
    }

    public bool IsBurning
    {
        get { return isBurning; }
    }

    public bool IsNearCheckpoint
    {
        get { return isNearCheckpoint; }
    }
}
