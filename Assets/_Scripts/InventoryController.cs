using Inventory.UI;
using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [HideInInspector] public static event Action ShowInventory;
        [HideInInspector] public static event Action HideIvenntory;
        [SerializeField] Ui_InventoryPage inventoryPage;
        [SerializeField] InventorySO inventoryData;
        [SerializeField] GameObject Player;

        public List<InventoryItem> initialItems = new List<InventoryItem>();
        private void Start()
        {
            PrepareUI(); // khởi tạo UI
            PrepareInventory();// khởi tạo dữ liệu cho inventory
            inventoryPage.gameObject.SetActive(false);
        }

        private void PrepareInventory()
        {
            inventoryData.Intitialize(); // khởi tạo những item rỗng
            inventoryData.OnInventoryUpdate += UpdateInventoryUI; // gán event khi inventory có thay đổi
            foreach (var item in initialItems)
            {
                if(item.IsEmpty) continue;
                inventoryData.AddItem(item);
            }
        }

        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            // thực hiện reset, làm mới lại các UI item cũ và đổi mới
            inventoryPage.ResetAllItem();
            foreach (var item in inventoryState)
            {
                inventoryPage.UpdateData(item.Key,item.Value.Item.ItemImage,item.Value.Quantity);
            }
        }

        private void PrepareUI()
        {
            inventoryPage.InitializeInventoryUI(inventoryData.Size);// khởi tạo UI 
            inventoryPage.OnDescriptionRequested += HandleDescriptionRequested;
            inventoryPage.OnSwapItems += HandleSwapItems;
            inventoryPage.OnItemActionRequested += HandleItemActionRequested;
            inventoryPage.OnStartDragging += HandleDragging;
        }

        private void HandleDragging(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty) return;
            ItemSO item = inventoryItem.Item;
            inventoryPage.CreateDraggedItem(item.ItemImage, inventoryItem.Quantity);
        }

        private void HandleItemActionRequested(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty) return;
            //IItemAction itemAction = inventoryItem.Item as IItemAction;
            bool isEnough = false;
            if (inventoryItem.Item is IDestroyableItem)
            {
               isEnough = inventoryData.RemoveItem(itemIndex, 1);
            }
            if (inventoryItem.Item is IItemAction itemAction && isEnough)
            {
                itemAction.PerformAction(Player);
            }
            
        }

        private void HandleSwapItems(int itemIndex1, int itemIndex2)
        {
            inventoryData.SwapItems(itemIndex1, itemIndex2);
        }

        private void HandleDescriptionRequested(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                inventoryPage.ResetSelection();
                return;
            }
            ItemSO item = inventoryItem.Item;
            inventoryPage.UpdateDescription(itemIndex, item.name, item.ItemImage, item.Description);
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (inventoryPage.isActiveAndEnabled == false)
                {
                    ShowInventory?.Invoke();
                    foreach (var item in inventoryData.GetCurrenInventoryState())
                    {
                        inventoryPage.UpdateData(item.Key, item.Value.Item.ItemImage, item.Value.Quantity);
                    }
                }
                else
                    HideIvenntory?.Invoke();
            }
        }
    }
}