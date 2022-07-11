using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerInventory : MonoBehaviour
{
    [NonReorderable]
    [SerializeField] private List<InventoryItem> inventoryItems = new List<InventoryItem>();

    public void AddItem(int id)
    {
        inventoryItems.ForEach(item =>
        {
            if (item.itemId == id)
            {
                item.itemQuantity++;
            }
        });
    }
}
