using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryUI; //disable/enable object
    public KeyCode inventoryInputKey = KeyCode.Tab;
    public int inventorySlotCount = 30; 
    public int quickAccessSlotCount = 8; 
    public List<InventorySlot> inventorySlots = new List<InventorySlot>();
    public List<InventorySlot> quickAccessSlots = new List<InventorySlot>();
    
    public event Action onInventoryChanged;


    private void Awake()
    {
        InitialiseInventory();
    }
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _inventoryUI.SetActive(!_inventoryUI.activeSelf);
            UpdateCursorState();
        }
    }
    
    private void UpdateCursorState()
    {
        if (_inventoryUI.activeSelf)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
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
        // try to add to quick access slots first
        int remainingCount = count;
        remainingCount = AddToQuickAccess(item, remainingCount);

        // If any reaming add to inventory
        if (remainingCount > 0)
        {
            remainingCount = AddToInventorySlots(item, remainingCount);
        }
        
        if (remainingCount > 0)
        {
            Debug.Log("Inventory is full!");
            return false;
        }

        onInventoryChanged?.Invoke();
        return true;
    }

    
private int AddToQuickAccess(BaseItem item, int count)
{
    int remainingCount = count;

    
    foreach (var slot in quickAccessSlots) //Try to stack first
    {
        if (!slot.IsEmpty() && slot.item == item && slot.itemCount < item.maxStackSize)
        {
            int spaceLeft = item.maxStackSize - slot.itemCount;
            int amountToAdd = Mathf.Min(remainingCount, spaceLeft);
            slot.itemCount += amountToAdd;
            remainingCount -= amountToAdd;

            if (remainingCount <= 0)
            {
                return 0;
            }
        }
    }

    
    foreach (var slot in quickAccessSlots) //Add to empty slot
    {
        if (slot.IsEmpty())
        {
            int amountToAdd = Mathf.Min(remainingCount, item.maxStackSize);
            slot.item = item;
            slot.itemCount = amountToAdd;
            remainingCount -= amountToAdd;

            if (remainingCount <= 0)
            {
                return 0;
            }
        }
    }

    return remainingCount;
}

private int AddToInventorySlots(BaseItem item, int count)
    {
    int remainingCount = count;

    
    foreach (var slot in inventorySlots) //Try to stack
    {
        if (!slot.IsEmpty() && slot.item == item && slot.itemCount < item.maxStackSize)
        {
            int spaceLeft = item.maxStackSize - slot.itemCount;
            int amountToAdd = Mathf.Min(remainingCount, spaceLeft);
            slot.itemCount += amountToAdd;
            remainingCount -= amountToAdd;

            if (remainingCount <= 0)
            {
                return 0;
            }
        }
    }

    
    foreach (var slot in inventorySlots) //Add empty slots
    {
        if (slot.IsEmpty())
        {
            int amountToAdd = Mathf.Min(remainingCount, item.maxStackSize);
            slot.item = item;
            slot.itemCount = amountToAdd;
            remainingCount -= amountToAdd;

            if (remainingCount <= 0)
            {
                return 0;
            }
        }
    }

    return remainingCount;
    }

    public bool IsInventoryFull()
    {
        
        foreach (var slot in quickAccessSlots)
        {
            if (slot.IsEmpty() || (slot.itemCount < slot.item.maxStackSize))
            {
                return false; // free space
            }
        }

        
        foreach (var slot in inventorySlots)
        {
            if (slot.IsEmpty() || (slot.itemCount < slot.item.maxStackSize))
            {
                return false; //free space
            }
        }

        // Both slots are full
        return true;
    }


}