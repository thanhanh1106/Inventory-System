using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class InventorySO : ScriptableObject
    {
        [SerializeField] List<InventoryItem> inventoryItems;

        [field: SerializeField] public int Size { get; private set; } = 15;

        public event Action<Dictionary<int, InventoryItem>> OnInventoryUpdate;

        public void Intitialize()
        {
            // tạo những Item rỗng và cho vào list
            inventoryItems = new List<InventoryItem>();
            for (int index = 0; index < Size; index++)
            {
                inventoryItems.Add(InventoryItem.GetEmptyItem());
            }
        }
        // duyệt qua list các inventory item nếu ô item nào còn chỗ trống thì thêm item đó vào
        public int AddItem(ItemSO item, int quatity)
        {
            if (!item.IsStakable)
            {
                for (int index = 0; index < inventoryItems.Count; index++)
                {
                    
                    while(quatity > 0 && !IsInventoryFull())
                    {
                        quatity -= AddItemToFistFreeSlot(item, 1);
                    }
                    InformAboutChange();
                    return quatity;
                }
            }
            quatity = AddStackableItem(item,quatity);
            InformAboutChange();
            return quatity;
        }

        // thêm item không được xếp chồng vào inventory
        private int AddItemToFistFreeSlot(ItemSO item, int quantity)
        {
            InventoryItem newInventoryItem = new InventoryItem() 
            { 
                Item = item,
                Quantity = quantity,
            };
            // tìm ô item trống và thêm item đó vào
            for(int index = 0;index  < inventoryItems.Count; index++)
            {
                if (inventoryItems[index].IsEmpty)
                {
                    inventoryItems[index] = newInventoryItem;
                    return quantity;
                }
            }
            return 0;
        }

        private bool IsInventoryFull() => inventoryItems.All(item => !item.IsEmpty);

        private int AddStackableItem(ItemSO item, int quatity)
        {
            for(int index = 0;index < inventoryItems.Count; index++)
            {
                if (inventoryItems[index].IsEmpty) continue;

                // nếu mà cùng loại item
                if (inventoryItems[index].Item.Id == item.Id)
                {
                    int amountPossibleToTake = // số stackSize còn lại
                        inventoryItems[index].Item.StackSize - inventoryItems[index].Quantity;
                    
                    if(quatity > amountPossibleToTake)
                    {
                        inventoryItems[index] =
                            inventoryItems[index].ChangeQuantity(inventoryItems[index].Item.StackSize);
                        quatity -= amountPossibleToTake;
                    }
                    else
                    {
                        inventoryItems[index] = 
                            inventoryItems[index].ChangeQuantity(inventoryItems[index].Quantity + quatity);
                        InformAboutChange();
                        return 0;
                    }
                }
            }
            while(quatity > 0 && !IsInventoryFull())
            {
                int newQuantity = Mathf.Clamp(quatity, 0, item.StackSize);
                quatity -= newQuantity;
                AddItemToFistFreeSlot(item, newQuantity);
            }
            return quatity;
        }

        public Dictionary<int, InventoryItem> GetCurrenInventoryState()
        {
            Dictionary<int, InventoryItem> result = new Dictionary<int, InventoryItem>();
            for (int index = 0; index < inventoryItems.Count; index++)
            {
                if (inventoryItems[index].IsEmpty) continue;
                result[index] = inventoryItems[index];
            }
            return result;
        }

        internal InventoryItem GetItemAt(int itemIndex)
        {
            return inventoryItems[itemIndex];
        }

        public void AddItem(InventoryItem item)
        {
            AddItem(item.Item, item.Quantity);
        }

        public void SwapItems(int itemIndex1, int itemIndex2)
        {
            InventoryItem itemTemp = inventoryItems[itemIndex1];
            inventoryItems[itemIndex1] = inventoryItems[itemIndex2];
            inventoryItems[itemIndex2] = itemTemp;
            InformAboutChange(); // gọi sự kiện khi có thay đổi của invetory diễn ra
        }

        private void InformAboutChange()
        {
            OnInventoryUpdate?.Invoke(GetCurrenInventoryState());
        }

        public bool RemoveItem(int itemIndex, int amount)
        {
            if(itemIndex < inventoryItems.Count)
            {
                if (inventoryItems[itemIndex].IsEmpty) return false;
                int remider = inventoryItems[itemIndex].Quantity - amount;
                if (remider < 0)
                    return false;
                else if(remider == 0)
                    inventoryItems[itemIndex] = InventoryItem.GetEmptyItem();
                else
                    inventoryItems[itemIndex] = inventoryItems[itemIndex].ChangeQuantity(remider);

                InformAboutChange();
            }
            return true;
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
}


