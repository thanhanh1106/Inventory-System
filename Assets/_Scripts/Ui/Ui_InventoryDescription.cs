using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class Ui_InventoryDescription : MonoBehaviour
    {
        [SerializeField] Image itemImage;
        [SerializeField] TMP_Text titleText;
        [SerializeField] TMP_Text descriptionText;
        private void Awake()
        {
            ResetDescription();
        }
        public void ResetDescription()
        {
            itemImage.gameObject.SetActive(false);
            titleText.text = string.Empty;
            descriptionText.text = string.Empty;
        }
        public void SetDescription(Sprite itemIcon, string itemName, string itemDescription)
        {
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = itemIcon;
            titleText.text = itemName;
            descriptionText.text = itemDescription;
        }
    }
}