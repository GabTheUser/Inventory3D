using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySwitcher : MonoBehaviour
{
    public GameObject inventoryPanel;
    private void OnMouseEnter()
    {
        inventoryPanel.SetActive(true); // Показываем панель при наведении
    }

    private void OnMouseExit()
    {
        inventoryPanel.SetActive(false); // Скрываем панель при уходе мыши
    }
}
