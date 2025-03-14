using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel; 
    public GameObject itemPrefab; 

    private Backpack backpack;

    private void Start()
    {
        backpack = FindObjectOfType<Backpack>();
        backpack.onItemDropped.AddListener(UpdateInventoryUI); 
        inventoryPanel.SetActive(true);
    }

    private void UpdateInventoryUI()
    {
        foreach (Transform item in backpack.transform)
        {
            TheItem inventoryItem = item.GetComponent<TheItem>();
            if (inventoryItem != null && inventoryItem.itemConfig != null)
            {
                CreateInventoryItem(inventoryItem); 
            }
        }
    }

    public void CreateInventoryItem(TheItem inventoryItem)
    {
        GameObject itemButton = Instantiate(itemPrefab, inventoryPanel.transform);
        itemButton.GetComponentInChildren<TextMeshProUGUI>().text = inventoryItem.itemConfig.itemName; 

        itemButton.GetComponent<Button>().onClick.AddListener(() => OnItemClicked(inventoryItem, itemButton));
    }

    private void OnItemClicked(TheItem inventoryItem, GameObject itemButton)
    {
        inventoryItem.GetComponent<DraggableItem>().MakeAvailable();
       
        Backpack backpack = FindObjectOfType<Backpack>();
        backpack.RemoveItemFromBackpack(inventoryItem);

        InventoryServerRequest serverRequest = FindObjectOfType<InventoryServerRequest>();
        serverRequest.SendInventoryEvent(inventoryItem.itemConfig.itemId, "remove");

        Destroy(itemButton);
    }
}
