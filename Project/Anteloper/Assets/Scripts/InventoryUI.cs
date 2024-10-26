using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryUI;
    public GameObject slotPrefab;
    public Transform inventoryGrid;
    public Transform quickAccessGrid;

    private Inventory _inventory;

    private List<SlotUI> inventorySlotUIs = new List<SlotUI>();
    private List<SlotUI> quickAccessSlotUIs = new List<SlotUI>();

    void Start()
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
        _inventory = FindObjectOfType<Inventory>();
        _inventory.onInventoryChanged += UpdateUI;

        InitialiseInventoryUI();
        InitialiseQuickAccessUI();
       
    }
    
    

    private void InitialiseInventoryUI()
    {
        foreach (Transform child in inventoryGrid)
        {
            Destroy(child.gameObject);
        }

        foreach (var slot in _inventory.inventorySlots)
        {
            GameObject newSlotGO = Instantiate(slotPrefab, inventoryGrid);
            SlotUI slotUI = newSlotGO.GetComponent<SlotUI>();
            slotUI.assignedSlot = slot; // Assign the InventorySlot
            inventorySlotUIs.Add(slotUI);
        }



        UpdateInventoryUI();
    }

    private void InitialiseQuickAccessUI()
    {
        foreach (Transform child in quickAccessGrid)
        {
            Destroy(child.gameObject);
        }

        foreach (var slot in _inventory.quickAccessSlots)
        {
            GameObject newSlotGO = Instantiate(slotPrefab, quickAccessGrid); 
            SlotUI slotUI = newSlotGO.GetComponent<SlotUI>();
            slotUI.assignedSlot = slot; // Assign InventorySlot
            quickAccessSlotUIs.Add(slotUI);
        }

        UpdateQuickAccessUI();
    }




    void UpdateUI()
    {
        UpdateInventoryUI();
        UpdateQuickAccessUI();
    }

    void UpdateInventoryUI()
    {
        foreach (var slotUI in inventorySlotUIs)
        {
            slotUI.UpdateSlotUI();
        }
    }

    void UpdateQuickAccessUI()
    {
        foreach (var slotUI in quickAccessSlotUIs)
        {
            slotUI.UpdateSlotUI();
        }
    }
    

    

    void OnDestroy()
    {
        if (_inventory != null)
        {
            _inventory.onInventoryChanged -= UpdateUI;
        }
    }
}