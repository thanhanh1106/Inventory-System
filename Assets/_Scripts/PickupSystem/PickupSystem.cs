using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSystem : MonoBehaviour
{
    [SerializeField] InventorySO inventoryData;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item item = collision.GetComponent<Item>();
        if (item)
        {
            int remider = inventoryData.AddItem(item.InventoryItem,item.Quantity);
            if(remider == 0)
                item.DestroyItem();
            else
                item.Quantity = remider;
        }
    }
}
