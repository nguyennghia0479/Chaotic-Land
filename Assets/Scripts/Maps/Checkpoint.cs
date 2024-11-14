using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Checkpoint : MonoBehaviour
{
    [Header("Checkpoint info")]
    [SerializeField] private string id;
    [SerializeField] private bool isBurning;

    [SerializeField] private GameObject tooltip;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI burnText;
    [SerializeField] private TextMeshProUGUI unBurnText;

    private Animator animator;
    private bool isNearCheckpoint;
    private const string BURN = "Burn";

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        tooltip.SetActive(false);

        if (isBurning) return;
        icon.gameObject.SetActive(false);
    }

    /// <summary>
    /// Handles to generate uuid.
    /// </summary>
    [ContextMenu("Generate UUID")]
    private void GenerateUUID()
    {
        id = System.Guid.NewGuid().ToString();
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
    public void Activate()
    {
        if (animator == null) return;

        isBurning = true;
        animator.SetTrigger(BURN);
        icon.gameObject.SetActive(true);
        burnText.gameObject.SetActive(true);
        unBurnText.gameObject.SetActive(false);
        PlayLightTorchSound();
    }

    /// <summary>
    /// Handles to play light torch sound.
    /// </summary>
    private void PlayLightTorchSound()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayLightTorchSound(transform.position);
        }
    }

    public string Id
    {
        get { return id; }
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
