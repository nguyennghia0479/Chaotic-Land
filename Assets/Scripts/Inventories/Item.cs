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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player _))
        {
            if (InventoryManager.Instance.CanAddMaterial(itemSO))
            {
                InventoryManager.Instance.AddInventory(itemSO);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Full slot");
            }

        }
    }

    public void SetupItem(ItemSO _itemSO, Vector2 _velocity)
    {
        if (_itemSO == null) return;

        itemSO = _itemSO;
        spriteRenderer.sprite = itemSO.sprite;
        gameObject.name = "Item - " + itemSO.name;
        rb.velocity = _velocity;
    }
}
