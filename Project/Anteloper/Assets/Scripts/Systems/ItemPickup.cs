using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour, IUsable
{
    public BaseItem item;  

    public void Use(bool isHold)
    {
        Inventory inventory = FindObjectOfType<Inventory>();

        if (inventory == null)
        {
            Debug.LogError("Inventory not found in scene.");
            return;
        }

        if (inventory.IsInventoryFull())
        {
            Debug.LogWarning("Cannot pick up item; inventory is full.");
            return;
        }

        
        if (item is ConsumableItem consumableItem && isHold)
        {
            
            Debug.Log($"Consuming item: {consumableItem.itemName}");
            consumableItem.Consume();
        }
        else if (item is EdibleItem edibleItem && isHold)
        {
            // Consume edible item if held down
            Debug.Log($"Consuming item: {edibleItem.itemName}");
            edibleItem.Consume();
        }
        else
        {
            // Pickup item and add to inventory
            inventory.AddItem(item, 1);
            Debug.Log($"Picked up {item.itemName}");
        }
        
        Destroy(gameObject);
    }
}