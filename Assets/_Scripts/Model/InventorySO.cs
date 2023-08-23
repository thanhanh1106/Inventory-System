using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class InventorySO : ScriptableObject
{
    [SerializeField] List<InventoryItem> inventoryItems;
    [field: SerializeField] public int Size { get; private set; } = 15;
    
    public void Intitialize()
    {
        inventoryItems = new List<InventoryItem>();
        for(int index = 0; index < Size; index++)
        {
            inventoryItems.Add(InventoryItem.GetEmptyItem());
        }
    }
    public void AddItem(ItemSO item,int quatity)
    {
        for(int index = 0; index < inventoryItems.Count; index++)
        {
            if (inventoryItems[index].IsEmpty)
            {
                inventoryItems[index] = new InventoryItem
                {
                    Item = item,
                    Quantity = quatity
                };
            }
        }
    }
    public Dictionary<int, InventoryItem> GetCurrenInventoryState()
    {
        Dictionary<int,InventoryItem> result = new Dictionary<int, InventoryItem>();
        for(int index = 0; index < inventoryItems.Count; index++)
        {
            if (inventoryItems[index].IsEmpty) continue;
            result[index] = inventoryItems[index];
        }
        return result;
    }
}

// lý do dùng struck là vì bảo mật dữ liệu, liên quan đến tính giá trị của struck
[System.Serializable]
public struct InventoryItem
{
    public int Quantity;
    public ItemSO Item;
    public bool IsEmpty => Item == null;
    public InventoryItem ChangeQuantity(int newQuantity) => new InventoryItem 
    {
        Item = this.Item,
        Quantity = newQuantity 
    };

    public static InventoryItem GetEmptyItem() => new InventoryItem
    {
        Item = null,
        Quantity = 0
    };
}

