using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour, IUsable
{
    public BaseItem item; 

    public void Use(bool isHold)
    {
        if (isHold && item is ConsumableItem)
        {
            ConsumeItem((ConsumableItem)item);
        }
        else
        {
            PickupItem();
        }
    }

    private void PickupItem()
    {
        Inventory inventory = FindObjectOfType<Inventory>();
        if (inventory != null)
        {
            if (inventory.AddItem(item, 1))
            {
                Destroy(gameObject);
                Debug.Log($"Picked up {item.itemName}.");
            }
            else
            {
                Debug.Log("Inventory is full!");
            }
        }
        else
        {
            Debug.LogError("Inventory not found in scene.");
        }
    }

    private void ConsumeItem(ConsumableItem consumableItem)
    {
        Debug.Log("Consuming item: " + consumableItem.itemName);
        consumableItem.Consume();
        Destroy(gameObject);
    }
}
    


