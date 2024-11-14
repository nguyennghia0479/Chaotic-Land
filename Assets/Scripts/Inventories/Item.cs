using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;
    private ItemSO itemSO;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Handles to pickup item.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            if (InventoryManager.Instance != null && InventoryManager.Instance.CanAddMaterial(itemSO))
            {
                InventoryManager.Instance.AddMaterialToInventory(itemSO);
                PlayPickupItemSound(player.transform.position);
                player.FX.PlayPopupPickItemText("+ 1 " + itemSO.itemName);
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// Handles to setup item info.
    /// </summary>
    /// <param name="_itemSO"></param>
    /// <param name="_velocity"></param>
    public void SetupItem(ItemSO _itemSO, Vector2 _velocity)
    {
        if (_itemSO == null) return;

        itemSO = _itemSO;
        spriteRenderer.sprite = itemSO.sprite;
        gameObject.name = "Item - " + itemSO.name;
        rb.velocity = _velocity;
    }

    /// <summary>
    /// Handles to ply pickup item sound.
    /// </summary>
    /// <param name="_position"></param>
    private void PlayPickupItemSound(Vector3 _position)
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayPickupItemSound(_position);
        }
    }
}
