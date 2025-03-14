using UnityEngine;
using UnityEngine.Events;

public class Backpack : MonoBehaviour
{
    public UnityEvent onItemDropped; 
    public InventoryServerRequest serverRequest; 

    public Transform weaponSlot;
    public Transform armorSlot;  
    public Transform consumableSlot; 
    public InventoryUI inventoryUI;
    public GameObject inventoryPanel; 

    private string currenItemId;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            DraggableItem draggableItem = other.GetComponent<DraggableItem>();

            if (draggableItem != null)
            {
                currenItemId = other.gameObject.GetComponent<TheItem>().itemConfig.itemId;
         
                onItemDropped?.Invoke();

                AssignSlotToItem(draggableItem);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            DraggableItem draggableItem = other.GetComponent<DraggableItem>();

            if (draggableItem != null)
            {
                currenItemId = null;
                other.transform.SetParent(null); 
            }
        }
    }

    public void AssignSlotToItem(DraggableItem item)
    {
        switch (item.itemType)
        {
            case ItemType.Weapon:
                item.SetTargetPosition(weaponSlot); 
                break;
            case ItemType.Armor:
                item.SetTargetPosition(armorSlot); 
                break;
            case ItemType.Consumable:
                item.SetTargetPosition(consumableSlot);
                break;
        }
    }

    public void AddToInventory(TheItem item)
    {
        inventoryUI.CreateInventoryItem(item);
        serverRequest.SendInventoryEvent(currenItemId, "add");
        currenItemId = null;
    }
    public void RemoveItemFromBackpack(TheItem inventoryItem)
    {
        // Перемещаем предмет из рюкзака в мир
        Transform itemTransform = inventoryItem.transform;
        itemTransform.SetParent(null);
    }

}
