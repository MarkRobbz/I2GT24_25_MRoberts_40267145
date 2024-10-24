using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Inventory : MonoBehaviour
{
    public int inventorySlotCount = 30; 
    public int quickAccessSlotCount = 8; 
    public List<InventorySlot> inventorySlots = new List<InventorySlot>();
    public List<InventorySlot> quickAccessSlots = new List<InventorySlot>();
    
    public event Action onInventoryChanged;

    private void Awake()
    {
        InitialiseInventory();
    }

    
    
    private void InitialiseInventory()
    {
        for (int i = 0; i < inventorySlotCount; i++)
        {
            inventorySlots.Add(new InventorySlot());
        }
        //Debug.Log("Inventory Slots Initialised: " + inventorySlots.Count);

        for (int i = 0; i < quickAccessSlotCount; i++)
        {
            quickAccessSlots.Add(new InventorySlot());
        }
        //debug.Log("Quick Access Slots Initialised: " + quickAccessSlots.Count);
    }

    
    public bool AddItem(BaseItem item, int count)
    {
        
        foreach (var slot in inventorySlots) //First try stack item
        {
            if (!slot.IsEmpty() && slot.item == item && slot.itemCount < item.maxStackSize)
            {
                int availableSpace = item.maxStackSize - slot.itemCount;
                int amountToAdd = Mathf.Min(count, availableSpace);
                slot.itemCount += amountToAdd;
                count -= amountToAdd;
                Debug.Log($"Stacked {amountToAdd} of {item.itemName} to existing slot.");
                onInventoryChanged?.Invoke();
                if (count <= 0)
                {
                    return true;
                }
            }
        }

        
        foreach (var slot in inventorySlots)
        {
            if (slot.IsEmpty())
            {
                int amountToAdd = Mathf.Min(count, item.maxStackSize);
                slot.item = item;
                slot.itemCount = amountToAdd;
                count -= amountToAdd;
                Debug.Log($"Added {amountToAdd} of {item.itemName} to new slot.");
                onInventoryChanged?.Invoke();
                if (count <= 0)
                {
                    return true;
                }
            }
        }

        Debug.Log("Inventory is full!");
        return false;
    }

    
    public bool AddToQuickAccess(BaseItem item, int count)
    {
        foreach (var slot in quickAccessSlots)
        {
            if (slot.AddItem(item, count))
            {
                return true;
            }
        }

        foreach (var slot in quickAccessSlots)
        {
            if (slot.IsEmpty())
            {
                slot.AddItem(item, count);
                return true;
            }
        }

        return false; // Quick-access bar full
    }
}