using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int inventorySlotCount = 30; 
    public int quickAccessSlotCount = 8; 
    public List<InventorySlot> inventorySlots = new List<InventorySlot>();
    public List<InventorySlot> quickAccessSlots = new List<InventorySlot>();

    private void Start()
    {
        InitializeInventory();
    }

    
    private void InitializeInventory()
    {
        for (int i = 0; i < inventorySlotCount; i++)
        {
            inventorySlots.Add(new InventorySlot());
        }

        for (int i = 0; i < quickAccessSlotCount; i++)
        {
            quickAccessSlots.Add(new InventorySlot());
        }
    }

    
    public bool AddItem(BaseItem item, int count)
    {
        foreach (var slot in inventorySlots)
        {
            if (slot.AddItem(item, count))
            {
                return true;
            }
        }

        
        foreach (var slot in inventorySlots) //Try find emoty slot
        {
            if (slot.IsEmpty())
            {
                slot.AddItem(item, count);
                return true;
            }
        }

        return false; // Inventory is full
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