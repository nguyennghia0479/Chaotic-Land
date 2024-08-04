using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private int numberOfDrops;
    [SerializeField] private List<ItemSO> itemSOs;
    [SerializeField] private Vector2 velocity;

    private List<ItemSO> dropItems;

    private void Start()
    {
        dropItems = new List<ItemSO>();
    }

    /// <summary>
    /// Handles to generate item drop.
    /// </summary>
    public void GenerateItemDrop()
    {
        while (itemSOs.Count > 0)
        {
            ItemSO itemCanDrop = itemSOs[Random.Range(0, itemSOs.Count)];
            if (Utils.RandomChance(itemCanDrop.dropChance))
            {
                dropItems.Add(itemCanDrop);
            }
            itemSOs.Remove(itemCanDrop);

            if (dropItems.Count >= numberOfDrops)
            {
                break;
            }
        }

        DropItem();
    }

    /// <summary>
    /// Handles to drop item.
    /// </summary>
    private void DropItem()
    {
        foreach (ItemSO itemSO in dropItems)
        {
            float velocityX = Random.Range(-velocity.x, velocity.x);
            float velocityY = Random.Range(velocity.y, velocity.y * 2);

            GameObject newItem = Instantiate(itemPrefab, transform.position, Quaternion.identity);
            newItem.GetComponent<Item>().SetupItem(itemSO, new Vector2(velocityX, velocityY));
        }
    }
}
