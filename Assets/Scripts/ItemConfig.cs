using UnityEngine;

[CreateAssetMenu(fileName = "NewItemConfig", menuName = "Inventory/ItemConfig")]
public class ItemConfig : ScriptableObject
{
    public string itemName;  
    public float weight;     
    public string itemId;    
    public ItemType itemType; 
}

public enum ItemType
{
    Weapon,
    Armor,
    Consumable
}
