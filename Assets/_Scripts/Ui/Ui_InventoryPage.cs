using Inventory;
using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class Ui_InventoryPage : MonoBehaviour
    {
        [SerializeField] Ui_InvetoryItem itemPrefab;
        [SerializeField] RectTransform contentPanel;
        [SerializeField] Ui_InventoryDescription inventoryDescription;
        [SerializeField] MouseFollowerItem mouseFollowerItem;

        List<Ui_InvetoryItem> UI_Items = new List<Ui_InvetoryItem>();

        int currentDragItemIndex = -1;

        public event Action<int> OnDescriptionRequested;
        public event Action<int> OnItemActionRequested;
        public event Action<int> OnStartDragging;

        public event Action<int, int> OnSwapItems;

        private void Awake()
        {
            InventoryController.ShowInventory += Show;
            InventoryController.HideIvenntory += Hide;
            mouseFollowerItem.Togge(false);
            inventoryDescription.ResetDescription();
        }
        public void InitializeInventoryUI(int size)
        {
            for (int index = 0; index < size; index++)
            {
                Ui_InvetoryItem UI_Item = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
                UI_Item.transform.parent = contentPanel;
                //UI_Item.transform.SetParent(contentPanel); // giống nhau, SetParent có thêm lựa chọn điều chỉnh vị trí
                UI_Items.Add(UI_Item);
                // gán các sự kiện tương tác của UI Item vào
                UI_Item.OnItemClicked += HandleItemSelection;
                UI_Item.OnItemBeginDrag += HandleBeginDrag;
                UI_Item.OnItemDropOn += HandleSwap;
                UI_Item.OnItemEndDrag += HandleEndDrag;
                UI_Item.OnRightMouseBtnClick += HandleShowItemActions;
            }
        }

        // set dữ liệu cho item
        public void UpdateData(int indexItem, Sprite ItemImage, int ItemQuatity)
        {
            if (UI_Items.Count > indexItem)
            {
                UI_Items[indexItem].SetData(ItemImage, ItemQuatity);
            }
        }

        private void HandleShowItemActions(Ui_InvetoryItem inventoryItemUi)
        {
            int index = UI_Items.IndexOf(inventoryItemUi);
            if (index == -1) return;
            OnItemActionRequested?.Invoke(index);
        }

        private void HandleEndDrag(Ui_InvetoryItem inventoryItemUi)
        {
            ResetDraggtedItem();
        }

        private void HandleSwap(Ui_InvetoryItem inventoryItemUi)
        {
            int index = UI_Items.IndexOf(inventoryItemUi);
            if (index == -1) return;
            OnSwapItems?.Invoke(currentDragItemIndex, index);
            HandleItemSelection(inventoryItemUi);
        }

        private void ResetDraggtedItem()
        {
            mouseFollowerItem.Togge(false);
            currentDragItemIndex = -1;
        }

        private void HandleBeginDrag(Ui_InvetoryItem inventoryItemUi)
        {
            int index = UI_Items.IndexOf(inventoryItemUi);
            if (index == -1) return;
            currentDragItemIndex = index;
            HandleItemSelection(inventoryItemUi);
            OnStartDragging?.Invoke(index);
        }
        public void CreateDraggedItem(Sprite sprite, int quantity)
        {
            mouseFollowerItem.Togge(true);
            mouseFollowerItem.SetData(sprite, quantity);
        }

        private void HandleItemSelection(Ui_InvetoryItem inventoryItemUi)
        {
            int index = UI_Items.IndexOf(inventoryItemUi);
            if (index == -1) return;
            OnDescriptionRequested?.Invoke(index);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            ResetSelection();
        }

        public void ResetSelection()
        {
            inventoryDescription.ResetDescription();
            DeselectAllItems();
        }

        private void DeselectAllItems()
        {
            foreach (var item in UI_Items)
            {
                item.Deselect();
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            ResetDraggtedItem();
        }

        internal void UpdateDescription(int itemIndex, string name, Sprite itemImage, string description)
        {
            DeselectAllItems();
            inventoryDescription.SetDescription(itemImage, name, description);
            UI_Items[itemIndex].Select();
        }

        internal void ResetAllItem()
        {
            foreach(var item in UI_Items)
            {
                item.ResetData();
                item.Deselect();
            }
        }
    }
}