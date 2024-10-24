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
        _inventory.onInventoryChanged += UpdateUI; 
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
        foreach (Transform child in inventoryGrid)
        {
            Destroy(child.gameObject);  // Clear previous slots
        }

        int slotIndex = 0;
        foreach (InventorySlot slot in _inventory.inventorySlots)
        {
            GameObject newSlotGO = Instantiate(slotPrefab, inventoryGrid);
            SlotUI slotUI = newSlotGO.GetComponent<SlotUI>();
            slotUI.UpdateSlotUI(slot);
            slotIndex++;
        }

        Debug.Log("Total Inventory Slots Created: " + slotIndex);
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
    
    void UpdateUI()
    {
        Debug.Log("Updating Inventory UI.");
        PopulateInventoryUI();
        PopulateQuickAccessUI();
    }

    void OnDestroy()
    {
        if (_inventory != null)
        {
            _inventory.onInventoryChanged -= UpdateUI; 
        }
    }
}