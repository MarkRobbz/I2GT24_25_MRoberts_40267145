using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryCanvas;  
    public GameObject slotPrefab;       
    public Transform inventoryGrid;     
    public Transform quickAccessGrid;   

    private Inventory _inventory;        

    void Start()
    {
        //Debug.Log("Inventory UI Start Method Called");
        _inventory = FindObjectOfType<Inventory>();  
        PopulateInventoryUI();
        PopulateQuickAccessUI();
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryCanvas.SetActive(!inventoryCanvas.activeSelf);
        }
    }

    
    void PopulateInventoryUI()
    {
        //Debug.Log("Populating Inventory UI");

        foreach (Transform child in inventoryGrid)
        {
            Destroy(child.gameObject);  // Clear previous slots (if any)
        }

        int slotIndex = 0;
        foreach (InventorySlot slot in _inventory.inventorySlots)
        {
            Debug.Log($"Creating Inventory Slot {slotIndex + 1}");
            GameObject newSlotGO = Instantiate(slotPrefab, inventoryGrid);
            SlotUI slotUI = newSlotGO.GetComponent<SlotUI>();
            slotUI.UpdateSlotUI(slot);
            slotIndex++;
        }

        //Debug.Log("Total Inventory Slots Created: " + slotIndex);
    }

    
    void PopulateQuickAccessUI()
    {
        //Debug.Log("Populating Quick Access UI");

        foreach (Transform child in quickAccessGrid)
        {
            Destroy(child.gameObject);  // Clear previous slots (if any)
        }

        foreach (InventorySlot slot in _inventory.quickAccessSlots)
        {
           // Debug.Log("Creating Quick Access Slot");
            GameObject newSlotGO = Instantiate(slotPrefab, quickAccessGrid);
            SlotUI slotUI = newSlotGO.GetComponent<SlotUI>();
            slotUI.UpdateSlotUI(slot);
        }
    }
}