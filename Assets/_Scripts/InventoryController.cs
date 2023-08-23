using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [HideInInspector] public static event Action ShowInventory;
    [HideInInspector] public static event Action HideIvenntory;
    [SerializeField] Ui_InventoryPage inventoryPage;

    private void Start()
    {
        inventoryPage.InitializeInventoryUI(15);
        inventoryPage.gameObject.SetActive(false);
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(inventoryPage.isActiveAndEnabled ==  false)
                ShowInventory.Invoke();
            else
                HideIvenntory.Invoke();
        }
    }
}
